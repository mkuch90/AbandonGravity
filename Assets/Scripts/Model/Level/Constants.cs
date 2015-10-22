using UnityEngine;
using System.Collections;

public static class Constants {

    public const float MaxSpeed = 30f;
    public const float effectsVolume = 0.3f;
    public const float musicVolume = 0.1f;

    public static Vector3 Rotate90(Vector3 toBeRot)
    {
        Vector3 ret = Vector3.zero;
        ret.x = -toBeRot.y;
        ret.y = toBeRot.x;
        return ret;
    }
	public static Vector3 Average(Vector3 lhs, Vector3 rhs,float rhsWeight)
	{
		return (lhs+rhs*rhsWeight)*1/(1+rhsWeight);
	}
	public static Vector3 Multiply(Vector3 lhs, Vector3 rhs)
	{
		return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
	}
	public static Vector3 Min(Vector3 lhs,Vector3 rhs){
		return new Vector3(Minimum (lhs.x, rhs.x),Minimum ( lhs.y , rhs.y), Minimum (lhs.z , rhs.z));
	}
	private static float Minimum(float lhs,float rhs)
	{
		if(Mathf.Sign (lhs) != Mathf.Sign (rhs)){
			return 0;
		}
		if(lhs>=0){
			return Mathf.Min (lhs,rhs);
		}
		return Mathf.Max (lhs,rhs);

	}
    
}
