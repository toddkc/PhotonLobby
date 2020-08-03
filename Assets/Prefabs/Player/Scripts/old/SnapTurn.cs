/// <summary>
/// this should go on a gameobject that is the parent of the CameraRig
/// </summary>
/// 
using System.Collections;
using UnityEngine;

public class SnapTurn : MonoBehaviour
{
	[SerializeField] private float delay = 0.2f;
	[SerializeField] private float maxTurn = 45f;
	[SerializeField] private OVRInput.Axis2D turnAxis = default;
	private bool canTurn = true;
	private WaitForSeconds turnDelay;
	private Transform thisTransform;

	private void Awake()
	{
		thisTransform = transform;
		turnDelay = new WaitForSeconds(delay);
	}

	private void Update()
	{
		TryTurn();
	}

	private void TryTurn()
	{
		float _turnInput = OVRInput.Get(turnAxis).x;
		if (!canTurn || _turnInput == 0) return;
		canTurn = false;
		StartCoroutine(ResetTurn());
		ProcessTurn(_turnInput);
	}

	private IEnumerator ResetTurn()
	{
		yield return turnDelay;
		canTurn = true;
	}

	private void ProcessTurn(float amount)
	{
		thisTransform.rotation *= Quaternion.AngleAxis(amount * maxTurn, thisTransform.up);
	}
}
