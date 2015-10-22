using UnityEngine;
using System.Collections;

public class UnpauseButtonMenuOption : MenuOption {

    private ResponseManager manager;
    protected override void Start()
    {
        manager = (ResponseManager)FindObjectOfType(typeof(ResponseManager));
		this.TextToDisplay = "Continue";
		base.Start();
    }

	protected override void OnMouseUp()
    {
        manager.PauseGame();
    }
}
