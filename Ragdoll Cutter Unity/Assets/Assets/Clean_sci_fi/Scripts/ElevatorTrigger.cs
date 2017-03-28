using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour {
	
	public ElevatorManager myManager = null;
	private ElevatorManager[] elevatorManagers;
	private int managerTotal = 0;

	void Awake()
	{
		if (myManager == null)
		{
			FindMyManager();
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if(myManager != null)
		{
			if(myManager.PlayerObject != null)
				myManager.PlayerObject.transform.parent = gameObject.transform.parent;
		}
		else
		{
			Debug.LogWarning("The trigger you have entered (Elevator_trigger) needs to be assigned a manager. Please refer to the manual. If no manager is assigned then the motion in the elevator is likely to be very juddery!");

		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if(myManager.PlayerObject != null)
		{
			myManager.PlayerObject.transform.parent = null;
		}
	}

	void FindMyManager()
	{
		elevatorManagers = Object.FindObjectsOfType(typeof(ElevatorManager)) as ElevatorManager[];
		managerTotal = elevatorManagers.Length;
		
		if (elevatorManagers.Length != 0)
			//Choose the manager which is a parent of the button
			for(int i=0; i<managerTotal; i++)
		{
			ElevatorManager aManager = (ElevatorManager) elevatorManagers[i];
			//Debug.Log ("Testing..." + aManager);
			if(this.transform.IsChildOf(aManager.transform))
				myManager = aManager;
		}
		else
		{
			//Warn the user there are no managers in the scene
			Debug.Log ("There are no ElevatorManager scripts assigned in this scene.");
		}
	}
}
