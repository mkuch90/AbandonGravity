using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// A complex object is an object and everything it is touching. 
/// This is so we can stack objects in multiple piles on a box and any "Chain" of objects counts as
/// a single mass to the button rather than discrete objects.
/// 
/// Needs to be tested with 3 or more boxes (I don't think it works)
/// </summary>
public class ComplexObject
{
    public MassObject obj;

    public List<ComplexObject> touching;

    
    public ComplexObject(MassObject a)
    {
        touching = new List<ComplexObject>();
        obj = a;
    }
    public bool Contains(MassObject lobj)
    {
        return lobj == obj;
    }
    public bool IsTouching(MassObject check)
    {
        foreach (ComplexObject touch in touching)
        {
            if (touch.Contains(check))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsTouching(ComplexObject check)
    {
        if(touching.Contains(check))
            {
                return true;
            }
        return false;
    }
    public void AddLink(ComplexObject cobj)
    {
        if (!touching.Contains(cobj))
        {
            touching.Add(cobj);
        }
    }
    public void RemoveLink(ComplexObject cobj)
    {
        touching.Remove(cobj);
    }
    public float GetMass()
    {
        float mass = GetSimpleMass();
        foreach (ComplexObject cobj in touching)
        {
            mass += cobj.GetSimpleMass();
        }
        return mass;
    }
    public float GetSimpleMass()
    {
        return obj.GetMass();
    }

}
