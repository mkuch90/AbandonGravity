using UnityEngine;
using System.Collections;

public class MenuContainer : MonoBehaviour
{

	/// <summary>
	/// This basically activates this object and all it's children recursively. 
	/// Allows us to hide the pause menu.
	/// </summary>
	/// <param name="active"> Whether this object is active</param>
	public void Active(bool active)
	{
		ActivateMenu(this.transform, active);
	}
	void ActivateMenu(Transform obj, bool active)
	{
		obj.gameObject.SetActive(active);
		foreach (Transform child in obj)
		{
			ActivateMenu(child, active);
		}
	}
}
