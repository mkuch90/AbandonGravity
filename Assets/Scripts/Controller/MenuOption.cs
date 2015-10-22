using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class MenuOption : MonoBehaviour {

	public string LevelToLoad = "";
	public string TextToDisplay = "";
	protected Color defaultColor;
	protected virtual void Start()
	{
		defaultColor = GetComponent<Renderer>().material.color;
		if(TextToDisplay !=""){
			TextMesh mesh = (TextMesh)this.GetComponent("TextMesh");
			mesh.text = TextToDisplay;
		}
	}
	protected void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.yellow;
	}
	protected void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = defaultColor;
	}
	protected virtual void OnMouseUp()
	{
		if (LevelToLoad == "")
		{
			return;
		}
		Application.LoadLevel(LevelToLoad);
	}
}
