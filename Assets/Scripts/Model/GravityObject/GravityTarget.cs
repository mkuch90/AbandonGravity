using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public abstract class GravityTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
    }
    protected void PhysicsUpdate()
    {
        if (transform.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("All Gravity Targets need to be Rigidbody");
            return;
        }
        limitMaxSpeed(Constants.MaxSpeed);
        Vector3 accel = GravityManager.GetAcceleration(transform.position);
		//Debug.Log (accel);
        transform.GetComponent<Rigidbody>().AddForce(accel*Time.deltaTime, ForceMode.VelocityChange);
    }
    public virtual void FixedUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.z = 0f;
        transform.position = newPos;
        PhysicsUpdate();
    }
    void limitMaxSpeed(float speed)
    {
        if (transform.GetComponent<Rigidbody>().velocity.sqrMagnitude > speed * speed) 
        {
            transform.GetComponent<Rigidbody>().velocity = transform.GetComponent<Rigidbody>().velocity.normalized * speed;
        }
    }
}
