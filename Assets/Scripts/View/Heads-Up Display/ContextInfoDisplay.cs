using UnityEngine;
using System.Collections.Generic;

public class ContextInfoDisplay : MonoBehaviour {

    ContextInfo CurrentMessage;
	// Use this for initialization
	void Start () {
        CurrentMessage = null;
	}

    void OnGUI()
    {
        if (CurrentMessage != null)
        {
            if (!CurrentMessage.displayMessage)
            {
                return;
            }
            GUI.TextArea(CurrentMessage.GetPosition(), CurrentMessage.GetMessage());
        }
    }

    public void DisplayMessage(ContextInfo message)
    {
        CurrentMessage = message;
    }
}
