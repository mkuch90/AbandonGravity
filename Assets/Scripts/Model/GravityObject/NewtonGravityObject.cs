using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class NewtonGravityObject : GravityObject
{

	public float strength = 5f;
	private const float minSquaredDistance = 10f;  //So forces don't go to infinity
	public static float maxSquaredDistance = 10000f;

	public override Vector3 getAcceleration(Vector3 sourcePosition)
	{
		Vector3 positionDiff = getPosition() - sourcePosition;
		float magnitudeSqr = positionDiff.sqrMagnitude;
		if (maxSquaredDistance < magnitudeSqr) //ignore far away objects (>100)
		{
			return Vector3.zero;
		}
		magnitudeSqr = Mathf.Max(magnitudeSqr, minSquaredDistance);
		float force = (strength / magnitudeSqr * strengthFactor);
		return positionDiff.normalized * force;
	}
}
