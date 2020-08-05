using UnityEngine;

public class GravityBoxNoPUN : GravityBox
{
	protected override void OnTriggerEnter(Collider col)
	{
		var rotator = col.GetComponent<PlayerRotateBase>();
		if (rotator == null) return;
		SwitchDirection(thisTransform.forward, rotator);
	}
}