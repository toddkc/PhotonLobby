using UnityEngine;

/// <summary>
/// this was taken from the wonderful tutorial found here:
/// https://catlikecoding.com/unity/tutorials/movement/
/// </summary>

public class GravitySource : MonoBehaviour
{
	[SerializeField] private bool registerOnEnable = false;
	public virtual Vector3 GetGravity(Vector3 position)
	{
		return Physics.gravity;
	}

	void OnEnable()
	{
		if(registerOnEnable)
			CustomGravity.Register(this);
	}

	void OnDisable()
	{
		CustomGravity.Unregister(this);
	}
}