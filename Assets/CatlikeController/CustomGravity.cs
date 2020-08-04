using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this was taken from the wonderful tutorial found here:
/// https://catlikecoding.com/unity/tutorials/movement/
/// </summary>

public class CustomGravity : MonoBehaviour
{
	static List<GravitySource> sources = new List<GravitySource>();

	public static void Register(GravitySource source)
	{
		if (sources.Contains(source)) return;
		sources.Add(source);
	}

	public static void Unregister(GravitySource source)
	{
		if (!sources.Contains(source)) return;
		sources.Remove(source);
	}

	public static void UnregisterAll()
	{
		sources.Clear();
	}

	public static Vector3 GetGravity(Vector3 position)
	{
		Vector3 g = Vector3.zero;
		if (sources.Count == 0)
		{
			g += Physics.gravity;
		}
		for (int i = 0; i < sources.Count; i++)
		{
			g += sources[i].GetGravity(position);
		}
		return g;
	}

	public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
	{
		Vector3 g = Vector3.zero;
		if(sources.Count == 0)
		{
			g += Physics.gravity;
		}
		for (int i = 0; i < sources.Count; i++)
		{
			g += sources[i].GetGravity(position);
		}
		upAxis = -g.normalized;
		return g;
	}

	public static Vector3 GetUpAxis(Vector3 position)
	{
		Vector3 g = Vector3.zero;
		if (sources.Count == 0)
		{
			g += Physics.gravity;
		}
		for (int i = 0; i < sources.Count; i++)
		{
			g += sources[i].GetGravity(position);
		}
		return -g.normalized;
	}
}