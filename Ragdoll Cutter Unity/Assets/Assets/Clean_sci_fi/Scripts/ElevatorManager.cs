using UnityEngine;
using System.Collections;

public class ElevatorManager : MonoBehaviour {
	
	public GameObject PlayerObject;

	public GameObject ElevatorObject;

	public GameObject ElevatorLockObject;

	public GameObject ElevatorDoorsObject;

	public AnimationCurve YCurve;

	public float TotalTravelTime = 5.0f;

	public float TravelSpeed = 50.0f;

	public float ElevatorDelay = 2.0f;

	public int StartAtFloorNumber = 0;
	public int CurrentFloorNum = 0;
	public int GotoFloorNum = 0;

	public GameObject[] FloorsArray = new GameObject[2];

	private Elevator_doors doorToOpen;
	private GameObject PlayerRoot;

	void Awake()
	{
		CurrentFloorNum = StartAtFloorNumber;
		if (PlayerObject == null)
		{
			//Go find a candidate for the player...
			//Debug.Log("Searching for player!");
			FindThePlayer();
		}
	}

	void Start()
	{
		if (FloorsArray[StartAtFloorNumber] != null)
		{
			Traverse(FloorsArray[StartAtFloorNumber]);
		}
	}

	//Function to look at children of the floor prefab and test for a
	//Elevator_doors() script. If found then it calls the public function
	//to open the door.
	void Traverse(GameObject obj)
	{
		//Debug.Log (obj.name);
		if(obj.GetComponent<Elevator_doors>())
		{
			doorToOpen = obj.GetComponent<Elevator_doors>();
			doorToOpen.Door_Open();
		}
		foreach (Transform child in obj.transform)
		{
			Traverse (child.gameObject);
		}
	}

	void FindThePlayer()
	{
		GameObject taggedObject = GameObject.FindWithTag("Player");

		if (taggedObject != null)
		{
			//Debug.Log("taggedObject = " + taggedObject.name);
			PlayerRoot = taggedObject.transform.root.gameObject;
			//Debug.Log("PlayerRoot = " + PlayerRoot.name);
			PlayerObject = PlayerRoot;
		}
		else
		{
			Debug.Log("Player root object not found! Assign a player gameObject to the Elevator manager script.");
		}

	}
}
