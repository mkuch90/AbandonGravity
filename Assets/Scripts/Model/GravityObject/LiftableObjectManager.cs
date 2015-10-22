using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Used for pushing buttons. 
/// 
/// Basically, we want objects to stack when pushing buttons.
/// This class keeps track of what objects are touching so we can have the button
/// affected by multiple objects at the same time.
/// </summary>
public class MassObjectManager : MonoBehaviour {


    private static List<ComplexObject> massObjects;
	

    public MassObjectManager()
    {
        massObjects = new List<ComplexObject>();
	}
    private static void addObject(ComplexObject obj)
    {
        createManagerIfNull();
        if (!massObjects.Contains(obj))
        {
            massObjects.Add(obj);
        }			
	}
    private static bool createManagerIfNull()
    {

        if (massObjects == null)
        {
            massObjects = new List<ComplexObject>();	
            return false;
        }
        return true;
    }
    public static ComplexObject GetComplex(MassObject lobj)
    {
        createManagerIfNull();
        foreach(ComplexObject obj in massObjects){
            if (obj.Contains(lobj))
            {
                return obj;
            }
        }
        ComplexObject newObj = new ComplexObject(lobj);
        addObject(newObj);
        return newObj;
    }
    public static void LinkObjects(MassObject objA, MassObject objB)
    {
        ComplexObject compA = GetComplex(objA);
        ComplexObject compB = GetComplex(objB);
        compA.AddLink(compB);
        compB.AddLink(compA);
    }
    public static float GetMass(MassObject obj)
    {
        ComplexObject cObj = GetComplex(obj);
        return cObj.GetMass();
    }
    public static void UnlinkObjects(MassObject objA, MassObject objB)
    {
        ComplexObject compA = GetComplex(objA);
        ComplexObject compB = GetComplex(objB);
        compA.RemoveLink(compB);
        compB.RemoveLink(compA);
    }
    public static float GetMass(List<MassObject> objs)
    {
        List<ComplexObject> complexes = new List<ComplexObject>();
        float mass = 0;
        foreach (MassObject obj in objs)
        {
            ComplexObject curr = GetComplex(obj);
            if (!complexes.Contains(curr))
            {
                bool addMass = true;
                foreach (ComplexObject complex in complexes)
                {
                    if (complex.IsTouching(curr))
                    {
                        addMass = false;
                    }
                }
                if (addMass)
                {
                    complexes.Add(curr);
                    mass += curr.GetMass();
                }
            }
        }
        //Debug.Log(mass);
        return mass;
    }
}
