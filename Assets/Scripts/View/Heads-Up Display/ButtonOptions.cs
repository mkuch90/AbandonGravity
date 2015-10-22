using UnityEngine;
using System.Collections;


[RequireComponent(typeof(GUIText))]
public class ButtonOptions : MonoBehaviour {


    public ResponseManager manager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<GUIText>().text = "";
        if (manager == null)
        {
            return;
        }
        ArrayList options = manager.GetDisplayOptions();
        foreach (object obj in options)
        {
            if (obj == null)
            {
                return;
            }
            ButtonOption option = (ButtonOption)obj;
            transform.GetComponent<GUIText>().text = option.GetDisplay();
        }
	}
}
