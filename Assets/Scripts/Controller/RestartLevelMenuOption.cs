using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Timers;


public class RestartLevelMenuOption : MenuOption
{

	private static System.Timers.Timer aTimer;
	private static string baseName = "";
	private static string SetName = "";
	private static bool loadLevel = false;
	private static float Interval = 1000;
	private static TextMesh mesh;
	private static int ticks = 1;
	private static int startTicks = 3;
	bool started = false;

	protected void Update()
	{
		mesh.text = SetName;
		if (loadLevel)
		{
			Application.LoadLevel(Application.loadedLevel);

		}
	}
	protected new void Start()
	{

		defaultColor = GetComponent<Renderer>().material.color;

	}
	private void StartInternal()
	{
		if (!started)
		{
			aTimer = new System.Timers.Timer(10000);

			// Hook up the Elapsed event for the timer.
			aTimer.Elapsed += new ElapsedEventHandler(RestartLevel);

			// Set the Interval to 5 seconds (2000 milliseconds).
			aTimer.Interval = Interval;
			aTimer.Enabled = false;
			baseName = this.TextToDisplay;
			mesh = (TextMesh)this.GetComponent("TextMesh");
			if (baseName == "")
			{
				baseName = mesh.text;
			}
			SetName = baseName;
			started = true;
			Debug.Log("Started");
		}
	}
	protected void OnEnable()
	{
		StartInternal();
		Debug.Log("TimerStarted");
		aTimer.Enabled = true;
		ticks = startTicks;
		Time.timeScale = 0.01f;
		loadLevel = false;
		SetName = baseName + " (" + ticks + ")";
	}
	protected void OnDisable()
	{


		StartInternal();
		Debug.Log("TimerEnded");
		aTimer.Enabled = false;

	}
	private static void RestartLevel(object source, ElapsedEventArgs e)
	{
		if (ticks > 1)
		{
			ticks--;
			SetName = baseName + " (" + ticks + ")";
			return;
		}
		loadLevel = true;
	}
	protected override void OnMouseUp()
	{

		Application.LoadLevel(Application.loadedLevel);
	}
}
