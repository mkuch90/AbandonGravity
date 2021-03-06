using UnityEngine;
using System.Collections;


/// <summary>
/// Interface for setting up a response target. 
/// Any object which has an effect based on another object should use this interface.
/// </summary>
public interface ResponseTarget  {

    void Activate();
    void Deactivate();
    Vector3 GetPosition();
	void VisualCue();
}
