
using UnityEngine;
using System.Collections;
/// <summary>
/// This bastard class monitors human input. It's not well defined.
/// </summary>
public class ResponseManager : MonoBehaviour {


    private static ArrayList activateTargets;
    private static ArrayList grabbableTargets;
    private PlatformerController controller;
    private PauseMenuContainer menuScreen;
	private DeathMenuContainer deathMenu;
    public static float maxDist = 2f;
    public static KeyCode GrabOrDrop = KeyCode.R;
    public static KeyCode Walk = KeyCode.LeftShift;
    public static KeyCode Activate = KeyCode.E;
    public static KeyCode Menu = KeyCode.Escape;
    private bool menuActive = false;
	public static bool CanPause = true;

    void Start()
    {
        Time.timeScale = 1;
        menuActive = false;
		menuScreen = (PauseMenuContainer)FindObjectOfType(typeof(PauseMenuContainer));
		deathMenu = (DeathMenuContainer)FindObjectOfType(typeof(DeathMenuContainer));
		controller = (PlatformerController)FindObjectOfType (typeof(PlatformerController));
		deathMenu.Active(false);
        menuScreen.Active(false);
    }
	public void Death()
	{
		Debug.LogWarning("Death");
		deathMenu.Active(true);
	}
    /// <summary>
    /// Pauses the game by setting the speed of time to 0.
	/// Resumes the game by setting the speed of time to 1.
    /// </summary>
    public void PauseGame()
    {
		if(!CanPause){
			return;
		}
		if (deathMenu.gameObject.activeSelf)
		{
			return;
		}
        menuActive = !menuActive;
        menuScreen.Active(menuActive);
        if (!menuActive)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
	/// <summary>
	/// This basically wiats for the player to try and activate buttons and boxes
	/// </summary>
    void Update()
    {
		this.transform.position=Vector3.zero;
        if (controller == null)
        {
            return;
        }

        if (Input.GetKeyDown(Menu))
        {
            PauseGame();
        }
        if (Input.GetKeyDown(Activate))
        {
            ResponseTarget target = ClosestOperand(controller.transform.position, controller.ForwardDirection(), activateTargets);
            if (target != null)
            {
                target.Activate();
                controller.PushButton();
                return;
            }


        }
        if (Input.GetKeyDown(GrabOrDrop))
        {
            if (controller.IsHoldingObject())
            {
                controller.DropObject(); 

                return;
            }
            ResponseTarget target = ClosestOperand(controller.transform.position, controller.ForwardDirection(), grabbableTargets);
            if (target != null)
            {
                target.Activate();
                return;
            }


        }

        if (Input.GetKey(Walk)) {
            controller.IsWalking(true);
        }
        else
        {
            controller.IsWalking(false);
        }
        
    }
    public static void AddActivateObject(ResponseTarget obj)
    {
        createManagerIfNull();
        if (!activateTargets.Contains(obj))
        {
            activateTargets.Add(obj);
        }
    }
    public static bool RemoveActivateObject(ResponseTarget obj)
    {
        if (!createManagerIfNull())
        {
            return false;
        }
        if (activateTargets.Contains(obj))
        {
            activateTargets.Remove(obj);
            return true;
        }
        return false;
    }

    public static void AddGrabbableObject(ResponseTarget obj)
    {
        createManagerIfNull();
        if (!grabbableTargets.Contains(obj))
        {
            grabbableTargets.Add(obj);
        }
    }
    public static bool RemoveGrabbableObject(ResponseTarget obj)
    {
        if (!createManagerIfNull())
        {
            return false;
        }
        if (grabbableTargets.Contains(obj))
        {
            grabbableTargets.Remove(obj);
            return true;
        }
        return false;
    }
    private static bool createManagerIfNull()
    {

        if (activateTargets == null)
        {
            activateTargets = new ArrayList();
        }
        if (grabbableTargets == null)
        {
            grabbableTargets = new ArrayList();
        }
        
        return true;
    }
	/// <summary>
	/// Gets the closest object in a certain direction which can be acted upon.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="direction"></param>
	/// <param name="responseTargets"></param>
	/// <returns></returns>
    private ResponseTarget ClosestOperand(Vector3 position,Vector3 direction,ArrayList responseTargets)
    {
        if (responseTargets == null)
        {
            return null;
        }
        if (!createManagerIfNull())
        {
            return null;
        }
        ResponseTarget retvalue = null;
        float retValDistSqrd = -1;
        foreach (object obj in responseTargets)
        {
            if (obj != null)
            {
                ResponseTarget current = (ResponseTarget)obj;
                float curDistSqrd = (current.GetPosition() - position).sqrMagnitude;
                if (curDistSqrd < maxDist*maxDist && (retValDistSqrd == -1 || (curDistSqrd < retValDistSqrd)))
                {
                    Vector3 distance = (current.GetPosition() - position - direction.normalized);
                    if (distance.sqrMagnitude < curDistSqrd)
                    {
                        retValDistSqrd = curDistSqrd;
                        retvalue = current;
                    }
                }  
            }
            
        }

        return retvalue;
    }
	/// <summary>
	/// Gets the options to display in front of the user.
	/// Currently, it only displays "pick up" and "activate"
	/// </summary>
	/// <returns></returns>
    public ArrayList GetDisplayOptions()
    {

        ArrayList options = new ArrayList();
        //Populate options if there is no button press.
        ResponseTarget target = ClosestOperand(controller.transform.position, controller.ForwardDirection(), activateTargets);
		
        if (target != null)
        {
			target.VisualCue();
            ButtonOption option = new ButtonOption();
            option.target = target;
            option.message = "to Activate.";
            option.button = Activate;
            options.Add(option);
            return options;
        }
        if (!controller.IsHoldingObject())
        {
            target = ClosestOperand(controller.transform.position, controller.ForwardDirection(), grabbableTargets);
            if (target != null)
            {
                ButtonOption option = new ButtonOption();
                option.message = "to pick up.";
                option.button = GrabOrDrop;
                option.target = target;
                options.Add(option);                
            }
        }
        return options;
    }
}