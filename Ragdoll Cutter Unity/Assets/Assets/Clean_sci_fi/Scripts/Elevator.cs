using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
	public ElevatorManager myManager = null;
	//public AnimatedUVs UVscroller = null;
	//public AnimationCurve scrollVCurve;
	public elevatorButton ActiveButton = null;

	public AudioClip elevatorStart;
	public AudioClip elevatorStop;
	public AudioClip elevatorRunning;
	public GameObject SFX_Player_1 = null;
	//public GameObject SFX_Player_2 = null;

	public bool buttonsActive = true;

	float floorCurrentY;
	float floorDestinationY;
	float floorTravelTime;

	private ElevatorManager[] elevatorManagers;
	private int managerTotal = 0;

	void Awake()
	{
		if (myManager == null)
		{
			FindMyManager();
		}
	}

	void Start()
	{
		if(myManager != null && myManager.FloorsArray.Length > 0)
		{
			//Check to see if user has entered a valid starting floor integer. 
			if (myManager.StartAtFloorNumber <= myManager.FloorsArray.Length)
			{
				//Debug.Log ("StartAtFloorNumber =" + myManager.StartAtFloorNumber);
				//Yes! Move the elevator compartment to the starting floor height
				float initialYPos = myManager.FloorsArray[myManager.StartAtFloorNumber].transform.position.y;
				transform.position = new Vector3 (transform.position.x, initialYPos, transform.position.z);
				//if(ActiveButton != null)ActiveButton.lightItUp(false);
			}
			else
			{
				//Debug.Log ("StartAtFloorNumber =" + myManager.StartAtFloorNumber);
				//No! Move the elevator compartment to the floor height of the first floor array entry
				float initialYPos = myManager.FloorsArray[0].transform.position.y;
				transform.position = new Vector3 (transform.position.x, initialYPos, transform.position.z);
			}
			if(myManager.ElevatorLockObject != null)
			{
				myManager.ElevatorLockObject.SetActive(false);
			}
			else
			{
				Debug.LogWarning("You need to assign a lock object to the elevator system to prevent the user falling out. See the manual for more information!");
			}
		}
	}

	public IEnumerator startElevator()
	{
		if(myManager != null)
		{
			if (myManager.GotoFloorNum != myManager.CurrentFloorNum)
			{
				//Debug.Log ("Number has changed. Elevator should move");
				if(ActiveButton != null)ActiveButton.lightItUp(true);
				StartCoroutine("Travel");

				if(myManager.ElevatorDoorsObject != null)
				{
					myManager.ElevatorDoorsObject.BroadcastMessage("Door_Close", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					Debug.LogWarning("You need to assign an Elevator Doors parent object to the elevator system to prevent the user falling out. See the manual for more information!");
				}

				myManager.FloorsArray[myManager.CurrentFloorNum].BroadcastMessage("Door_Close", SendMessageOptions.DontRequireReceiver);
				yield return null;
			}
			else
			{
				//Debug.Log ("Elevator not going anywhere");
				yield return null;
			}
		}
		else
		{
			Debug.Log ("Please set up links to the Elevator Manager first!");
		}
	}

	IEnumerator Travel()
	{
		float ElapsedTime = 0.0f;
		float originalYPos = transform.position.y;

		buttonsActive = false;

		if(myManager.ElevatorLockObject != null)
		{
			myManager.ElevatorLockObject.SetActive(true);
		}
		else
		{
			Debug.LogWarning("You need to assign a lock object to the elevator system to prevent the user falling out. See the manual for more information!");
		}


		yield return new WaitForSeconds(myManager.ElevatorDelay);

		floorCurrentY = myManager.FloorsArray[myManager.CurrentFloorNum].transform.position.y;
		floorDestinationY = myManager.FloorsArray[myManager.GotoFloorNum].transform.position.y;
		float floorDifference = floorDestinationY - floorCurrentY;
		float floorTravelTime = (Mathf.Abs(myManager.CurrentFloorNum - myManager.GotoFloorNum)) * myManager.TotalTravelTime;

		GetComponent<AudioSource>().clip = elevatorRunning;
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
		if(SFX_Player_1 != null)
		{
			SFX_Player_1.GetComponent<AudioSource>().clip = elevatorStart;
			//audio.loop = false;
			GetComponent<AudioSource>().PlayOneShot(elevatorStart, 1.0F);
			//SFX_Player_1.audio.Play ();
		}

		/*if(UVscroller != null)
		{
			if(floorDestinationY > floorCurrentY)//Going up!
			{
				UVscroller.uvAnimationRate = new Vector2( 0.0f, 1.0f );
			}
			else //Going up!
			{
				UVscroller.uvAnimationRate = new Vector2( 0.0f, -1.0f );
			}
			UVscroller.boolUV = true;
		}
		*/

		//Debug.Log ("ActiveButton = "+ ActiveButton);

		while(ElapsedTime < floorTravelTime)
		{
			float YPos = myManager.YCurve.Evaluate(ElapsedTime/floorTravelTime) * floorDifference;
			//float texVPos = scrollVCurve.Evaluate(ElapsedTime/floorTravelTime) * 1.0f;
			//if(UVscroller != null)UVscroller.rateMutilplier = texVPos;
			transform.position = new Vector3(transform.position.x, (originalYPos + YPos), transform.position.z);
			yield return null;
			ElapsedTime += Time.deltaTime;
		}

		//if(UVscroller != null)UVscroller.boolUV = false;
		if(ActiveButton != null)ActiveButton.lightItUp(false);

		GetComponent<AudioSource>().clip = elevatorStop;
		GetComponent<AudioSource>().PlayOneShot(elevatorStop, 1.0F);
		myManager.FloorsArray[myManager.GotoFloorNum].BroadcastMessage("Door_Open", SendMessageOptions.DontRequireReceiver);

		if(myManager.ElevatorDoorsObject != null)
		{
			myManager.ElevatorDoorsObject.BroadcastMessage ("Door_Open", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Debug.LogWarning("You need to assign an Elevator Doors parent object to the elevator system to prevent the user falling out. See the manual for more information!");
		}

		if(myManager.ElevatorLockObject != null)
		{
			myManager.ElevatorLockObject.SetActive(false);
		}
		else
		{
			Debug.LogWarning("You need to assign a lock object to the elevator system to prevent the user falling out. See the manual for more information!");
		}

		//Update the current floor integer
		myManager.CurrentFloorNum = myManager.GotoFloorNum;
		buttonsActive = true;
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
			Debug.Log ("There are no ElevatorManager scripts assigned in this scene. Elevator buttopns will not work without these scripts. Please see the manual for assistance");
		}
	}
}
