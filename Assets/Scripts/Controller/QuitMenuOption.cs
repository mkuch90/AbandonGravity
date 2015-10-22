using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

	class QuitMenuOption : MenuOption
	{
		protected override void Start()
		{
			this.TextToDisplay = "Quit";
			base.Start();
		}
		protected override void OnMouseUp()
		{
			Debug.Log("Quit");
			Application.Quit();
		}
	}
