using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public abstract class GravityMod : MonoBehaviour {
	/*instead of tihs, it should be mapped in the other direction. Each gravityobject should have a list of direction mods
	//Each mod should be logically in the same group as a few other objects
	 * Proposal: have the direction mod a dumb trigger
	 * A grouper links to the direction mod. It listens to all of them and activates if it detects a change. The grouper
	 * holds the proplery of the direciton mod and the response target is just a trigger.
	 * The manager watches the grouper which holds the direction mod objects.
	 * */
	public bool activated = false;
	bool justActivated = false;
	public bool timer = false;
	public float timeout = 10f;

	private static System.Timers.Timer aTimer;
	private float activateTime = 0f;
	public GameObject tickAudioSource;
	public GameObject activateAudioSource;
	private bool DisableOnUpdate = false;  //exists for thread purposes

	public List<GravityResponseTarget> buttons;
	void Start()
	{
		if (tickAudioSource == null || activateAudioSource==null)
		{
			Debug.LogError("Light time audio missing");
		}
		if(activated){
			foreach (GravityResponseTarget target in buttons)
			{
				target.SetActive(true);
			}
		}
	}
	void Update()
	{
		if (DisableOnUpdate)
		{
			if (Time.time - activateTime >= timeout - 0.5f)
			{
				SetAllActive(false);
				aTimer.Dispose();
			}

			DisableOnUpdate = false;
		}
		foreach (GravityResponseTarget target in buttons)
		{
			if (target.WasButtonPressed())
			{
				target.ResetButtonPressed();
				SetAllActive(true);
				break;
			}
		}
		if (activated && timer && !tickAudioSource.GetComponent<AudioSource>().isPlaying)
		{
			tickAudioSource.GetComponent<AudioSource>().Play();
		}
		

	}
	public void SetAllActive(bool active)
	{
		if (active && timer)
		{
			activateTime = Time.time;
			if (aTimer != null)
			{
				aTimer.Dispose();
			}
			aTimer = new System.Timers.Timer(10000);

			// Hook up the Elapsed event for the timer.
			aTimer.Elapsed += new ElapsedEventHandler(TimerDeactivate);

			// Set the Interval to 5 seconds (2000 milliseconds).
			aTimer.Interval = timeout*1000;
			aTimer.Enabled = true;

		}
		activateAudioSource.GetComponent<AudioSource>().Play();
		if (!active)
		{
			tickAudioSource.GetComponent<AudioSource>().Stop();
		}
		if (active == activated)
		{
			return;
		}
		activated = active;
		justActivated = active;
		foreach (GravityResponseTarget target in buttons)
		{
			target.SetActive(active);
			target.ResetButtonPressed();
		}
	}

	private void TimerDeactivate(object sender, ElapsedEventArgs e)
	{
		DisableOnUpdate = true;
	}
	public bool IsActivated()
	{
		return activated;
	}
	public bool JustActivated()
	{
		return justActivated;
	}
	public void ResetJustActivated()
	{
		justActivated = false;
	}
	

}
