using UnityEngine;

public class GravityBox : GravitySource
{
	[SerializeField] private float gravity = 9.81f;
	protected Transform thisTransform;

	private void Awake()
	{
		thisTransform = transform;
	}

	public override Vector3 GetGravity(Vector3 position)
	{
		Vector3 up = transform.forward;
		return -gravity * up;
	}

	protected virtual void OnTriggerEnter(Collider col)
	{
		var rotator = col.GetComponent<PlayerRotateBase>();
		if (rotator == null) return;
		SwitchDirection(thisTransform.forward, rotator);
	}

	protected void SwitchDirection(Vector3 direction, PlayerRotateBase rotator)
	{
		float _angleThreshold = 0.001f;
		float _angleBetweenUpDirections = Vector3.Angle(direction, rotator.transform.up);
		if (_angleBetweenUpDirections < _angleThreshold)
		{
			return;
		}
		Quaternion _rotationDifference = Quaternion.FromToRotation(rotator.transform.up, direction);
		rotator.Rotate(_rotationDifference);
		CustomGravity.UnregisterAll();
		CustomGravity.Register(this);
	}
}