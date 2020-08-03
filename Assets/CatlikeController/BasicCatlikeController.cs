using UnityEngine;

/// <summary>
/// this was taken from the wonderful tutorial found here:
/// https://catlikecoding.com/unity/tutorials/movement/
/// 
/// basic will remove swimming (and maybe climbing?)
/// 
/// </summary>

public class BasicCatlikeController : MonoBehaviour
{
	[SerializeField]
	Transform playerInputSpace = default;

	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f, maxClimbSpeed = 4f;

	[SerializeField, Range(0f, 100f)]
	float
		maxAcceleration = 10f,
		maxAirAcceleration = 1f,
		maxClimbAcceleration = 40f;

	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;

	[SerializeField, Range(0, 5)]
	int maxAirJumps = 0;

	[SerializeField, Range(0, 90)]
	float maxGroundAngle = 25f, maxStairsAngle = 50f;

	[SerializeField, Range(90, 170)]
	float maxClimbAngle = 140f;

	[SerializeField, Range(0f, 100f)]
	float maxSnapSpeed = 100f;

	[SerializeField, Min(0f)]
	float probeDistance = 1f;

	[SerializeField]
	LayerMask probeMask = -1, stairsMask = -1, climbMask = -1;

	Rigidbody body, connectedBody, previousConnectedBody;

	Vector3 playerInput;

	Vector3 velocity, connectionVelocity;

	Vector3 connectionWorldPosition, connectionLocalPosition;

	Vector3 upAxis, rightAxis, forwardAxis;

	bool desiredJump, desiresClimbing;

	Vector3 contactNormal, steepNormal, climbNormal, lastClimbNormal;

	int groundContactCount, steepContactCount, climbContactCount;

	bool OnGround => groundContactCount > 0;

	bool OnSteep => steepContactCount > 0;

	bool Climbing => climbContactCount > 0 && stepsSinceLastJump > 2;

	int jumpPhase;

	float minGroundDotProduct, minStairsDotProduct, minClimbDotProduct;

	int stepsSinceLastGrounded, stepsSinceLastJump;

	public void PreventSnapToGround()
	{
		stepsSinceLastJump = -1;
	}

	void OnValidate()
	{
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
		minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
		minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
	}

	void Awake()
	{
		body = GetComponent<Rigidbody>();
		body.useGravity = false;
		OnValidate();
	}

	void Update()
	{
		playerInput.y = InputBridgeBase.instance.MoveAxis;
		playerInput.x = InputBridgeBase.instance.StrafeAxis;
		playerInput = Vector3.ClampMagnitude(playerInput, 1f);

		if (playerInputSpace)
		{
			rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
			forwardAxis =
				ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
		}
		else
		{
			rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
			forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
		}


		desiredJump |= InputBridgeBase.instance.Jump;
		desiresClimbing = InputBridgeBase.instance.Climb;
	}

	void FixedUpdate()
	{
		Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);
		UpdateState();
		AdjustVelocity();

		if (desiredJump)
		{
			desiredJump = false;
			Jump(gravity);
		}

		if (Climbing)
		{
			velocity -=
				contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
		}
		else if (desiresClimbing && OnGround)
		{
			velocity +=
				(gravity - contactNormal * (maxClimbAcceleration * 0.9f)) *
				Time.deltaTime;
		}
		else if (OnGround && velocity.sqrMagnitude < 0.01f)
		{
			velocity +=
				contactNormal *
				(Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
		}
		else
		{
			velocity += gravity * Time.deltaTime;
		}
		body.velocity = velocity;
		ClearState();
	}

	void ClearState()
	{
		groundContactCount = steepContactCount = climbContactCount = 0;
		contactNormal = steepNormal = climbNormal = Vector3.zero;
		connectionVelocity = Vector3.zero;
		previousConnectedBody = connectedBody;
		connectedBody = null;
	}

	void UpdateState()
	{
		stepsSinceLastGrounded += 1;
		stepsSinceLastJump += 1;
		velocity = body.velocity;
		if (
			CheckClimbing() ||
			OnGround || SnapToGround() || CheckSteepContacts()
		)
		{
			stepsSinceLastGrounded = 0;
			if (stepsSinceLastJump > 1)
			{
				jumpPhase = 0;
			}
			if (groundContactCount > 1)
			{
				contactNormal.Normalize();
			}
		}
		else
		{
			contactNormal = upAxis;
		}

		if (connectedBody)
		{
			if (connectedBody.isKinematic || connectedBody.mass >= body.mass)
			{
				UpdateConnectionState();
			}
		}
	}

	void UpdateConnectionState()
	{
		if (connectedBody == previousConnectedBody)
		{
			Vector3 connectionMovement =
				connectedBody.transform.TransformPoint(connectionLocalPosition) -
				connectionWorldPosition;
			connectionVelocity = connectionMovement / Time.deltaTime;
		}
		connectionWorldPosition = body.position;
		connectionLocalPosition = connectedBody.transform.InverseTransformPoint(
			connectionWorldPosition
		);
	}

	bool CheckClimbing()
	{
		if (Climbing)
		{
			if (climbContactCount > 1)
			{
				climbNormal.Normalize();
				float upDot = Vector3.Dot(upAxis, climbNormal);
				if (upDot >= minGroundDotProduct)
				{
					climbNormal = lastClimbNormal;
				}
			}
			groundContactCount = 1;
			contactNormal = climbNormal;
			return true;
		}
		return false;
	}

	bool SnapToGround()
	{
		if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
		{
			return false;
		}
		float speed = velocity.magnitude;
		if (speed > maxSnapSpeed)
		{
			return false;
		}
		if (!Physics.Raycast(
			body.position, -upAxis, out RaycastHit hit,
			probeDistance, probeMask, QueryTriggerInteraction.Ignore
		))
		{
			return false;
		}

		float upDot = Vector3.Dot(upAxis, hit.normal);
		if (upDot < GetMinDot(hit.collider.gameObject.layer))
		{
			return false;
		}

		groundContactCount = 1;
		contactNormal = hit.normal;
		float dot = Vector3.Dot(velocity, hit.normal);
		if (dot > 0f)
		{
			velocity = (velocity - hit.normal * dot).normalized * speed;
		}
		connectedBody = hit.rigidbody;
		return true;
	}

	bool CheckSteepContacts()
	{
		if (steepContactCount > 1)
		{
			steepNormal.Normalize();
			float upDot = Vector3.Dot(upAxis, steepNormal);
			if (upDot >= minGroundDotProduct)
			{
				steepContactCount = 0;
				groundContactCount = 1;
				contactNormal = steepNormal;
				return true;
			}
		}
		return false;
	}

	void AdjustVelocity()
	{
		float acceleration, speed;
		Vector3 xAxis, zAxis;
		if (Climbing)
		{
			acceleration = maxClimbAcceleration;
			speed = maxClimbSpeed;
			xAxis = Vector3.Cross(contactNormal, upAxis);
			zAxis = upAxis;
		}
		else
		{
			acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
			speed = OnGround && desiresClimbing ? maxClimbSpeed : maxSpeed;
			xAxis = rightAxis;
			zAxis = forwardAxis;
		}
		xAxis = ProjectDirectionOnPlane(xAxis, contactNormal);
		zAxis = ProjectDirectionOnPlane(zAxis, contactNormal);

		Vector3 relativeVelocity = velocity - connectionVelocity;
		float currentX = Vector3.Dot(relativeVelocity, xAxis);
		float currentZ = Vector3.Dot(relativeVelocity, zAxis);

		float maxSpeedChange = acceleration * Time.deltaTime;

		float newX =
			Mathf.MoveTowards(currentX, playerInput.x * speed, maxSpeedChange);
		float newZ =
			Mathf.MoveTowards(currentZ, playerInput.y * speed, maxSpeedChange);

		velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
	}

	void Jump(Vector3 gravity)
	{
		Vector3 jumpDirection;
		if (OnGround)
		{
			jumpDirection = contactNormal;
		}
		else if (OnSteep)
		{
			jumpDirection = steepNormal;
			jumpPhase = 0;
		}
		else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps)
		{
			if (jumpPhase == 0)
			{
				jumpPhase = 1;
			}
			jumpDirection = contactNormal;
		}
		else
		{
			return;
		}

		stepsSinceLastJump = 0;
		jumpPhase += 1;
		float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
		jumpDirection = (jumpDirection + upAxis).normalized;
		float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
		if (alignedSpeed > 0f)
		{
			jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
		}
		velocity += jumpDirection * jumpSpeed;
	}

	void OnCollisionEnter(Collision collision)
	{
		EvaluateCollision(collision);
	}

	void OnCollisionStay(Collision collision)
	{
		EvaluateCollision(collision);
	}

	void EvaluateCollision(Collision collision)
	{
		int layer = collision.gameObject.layer;
		float minDot = GetMinDot(layer);
		for (int i = 0; i < collision.contactCount; i++)
		{
			Vector3 normal = collision.GetContact(i).normal;
			float upDot = Vector3.Dot(upAxis, normal);
			if (upDot >= minDot)
			{
				groundContactCount += 1;
				contactNormal += normal;
				connectedBody = collision.rigidbody;
			}
			else
			{
				if (upDot > -0.01f)
				{
					steepContactCount += 1;
					steepNormal += normal;
					if (groundContactCount == 0)
					{
						connectedBody = collision.rigidbody;
					}
				}
				if (
					desiresClimbing && upDot >= minClimbDotProduct &&
					(climbMask & (1 << layer)) != 0
				)
				{
					climbContactCount += 1;
					climbNormal += normal;
					lastClimbNormal = normal;
					connectedBody = collision.rigidbody;
				}
			}
		}
	}

	Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
	{
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}

	float GetMinDot(int layer)
	{
		return (stairsMask & (1 << layer)) == 0 ?
			minGroundDotProduct : minStairsDotProduct;
	}
}