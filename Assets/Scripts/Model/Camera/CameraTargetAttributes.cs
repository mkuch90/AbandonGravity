using UnityEngine;
using System.Collections;

public class CameraTargetAttributes : MonoBehaviour
{

	public float heightOffset = 0.0f;
	public float distanceModifier = 1.0f;
	public float velocityLookAhead = 0.15f;
	public Vector2 maxLookAhead = new Vector2(3.0f, 3.0f);
}
