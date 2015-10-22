using UnityEngine;
using System.Collections;

public class ContextInfo : MonoBehaviour {

    public string message = "";
    private float startTime = 0f;
    public float timeToPlay = 0f;
    public ContextInfoDisplay display;
    public float timeDisplayed = 10f;
    public bool displayMessage = true;
    public Vector2 boxDimensions;

    public void Start(){
        startTime = Time.time;
    }
    public string GetMessage()
    {
        return message;
    }

    public void Update()
    {
        if (Time.time > timeToPlay + startTime && Time.time < timeToPlay + timeDisplayed + startTime)
        {
            display.DisplayMessage(this);
        }
    }
    public Rect GetPosition()
    {
        return new Rect(10, 10, boxDimensions.x, boxDimensions.y);
    }
	public void OnGUI(){
		GUI.Label(new Rect(Screen.width/4,Screen.height/4,Screen.width,Screen.height),"Here is a block of text\nlalalala\nanother line\nI could do this all day!");
	}
	
	
}
