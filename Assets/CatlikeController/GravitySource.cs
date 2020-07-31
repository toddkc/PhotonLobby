using UnityEngine;

/// <summary>
/// this was taken from the wonderful tutorial found here:
/// https://catlikecoding.com/unity/tutorials/movement/
/// </summary>

public class GravitySource : MonoBehaviour
{
	public virtual Vector3 GetGravity(Vector3 position)
	{
		return Physics.gravity;
	}

	void OnEnable()
	{
		CustomGravity.Register(this);
	}

	void OnDisable()
	{
		CustomGravity.Unregister(this);
	}
}