using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class ConstantGravityObject : GravityObject
{
	public float strength = 10f;
	public Vector3 constantDirection = new Vector3(0, -1, 0);
	/// <summary>
	/// Gets the acceleration this object imparts due to gravity. Constant Gravity.
	/// </summary>
	/// <returns>
	/// The acceleration in the form of a 3-Dimentional vector.
	/// </returns>
	/// <param name='sourcePosition'>
	/// The position of the object being affected by this object.
	/// </param>
	public override Vector3 getAcceleration(Vector3 sourcePosition)
	{
		Vector3 toBeUsed = directionModFactor;
		if (toBeUsed == Vector3.zero)
		{
			toBeUsed = constantDirection;
		}
		return toBeUsed * strength * strengthFactor;
	}
}
