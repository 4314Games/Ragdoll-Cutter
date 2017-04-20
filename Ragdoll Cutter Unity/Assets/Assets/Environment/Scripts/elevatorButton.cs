using UnityEngine;
using System.Collections;

public class elevatorButton : MonoBehaviour
{
	public ElevatorManager myManager = null;
	public Elevator myElevator = null;

	private Vector3 screenCentre = new Vector3(0.5f, 0.5f, 0.0f);
	//Camera so we can cast a ray and choose a button
	public Camera GameCamera = null;
	//What floor are we going to?
	//Following int dictates what kind of button:
	//		0 through positive ints dictate specific floors in the floor array
	//		-1 = incremetally UP only (simplly move up one floor at a time)
	//		-2 = incremetally DOWN only (simplly move down one floor at a time)
	public int CallFloor = 0;
	private ElevatorManager[] elevatorManagers;
	private int managerTotal = 0;
	private Elevator[] elevators;
	private int elevatorTotal = 0;

	void Awake()
	{
		if (!gameObject.GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();
		if (GameCamera == null)
		{
			FindGameCamera();
		}
		if (myManager == null)
		{
			FindMyManager();
		}
		if (myElevator == null && myManager != null)
		{
			FindMyElevator();
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if(!Input.GetMouseButtonDown(0))
			return;
		if(GameCamera != null)
		{
			Ray R = GameCamera.ViewportPointToRay(screenCentre);
			RaycastHit Hit;
			if(GetComponent<BoxCollider>().Raycast(R, out Hit, 1000.0f))
			{
				if((myManager != null)&&(myElevator != null))
				{
					//Debug.Log ("Pressed button!");
					if(myElevator.buttonsActive == true)
					{
						if (CallFloor >= 0)
						{
							eleButtonFloorSwitch();
						}else{
							eleButtonUpDownSwitch();
						}
					}
				}
				else
				{
					Debug.LogWarning("Link this button to game objects with ElevatorManager script and Elevator scripts present");
					Debug.Log("Button will not work until user sets up the links between scripted objects");
				}
			}
		}
	}

	void eleButtonUpDownSwitch()
	{
		if(CallFloor == -1)
			//We're going UP
		{
			if(myManager.CurrentFloorNum != myManager.FloorsArray.Length -1)
				//We are not at the top - so move up!
			{
				myManager.GotoFloorNum = myManager.CurrentFloorNum + 1;
				myElevator.ActiveButton = this;
				myElevator.StartCoroutine("startElevator");
			}
			else
			{
				//test to see if button attached to the elevator
				if((buttonOffSFX != null))
				{
					GetComponent<AudioSource>().clip = buttonOffSFX;
					GetComponent<AudioSource>().Play();
				}
			}
		}
		if(CallFloor == -2)
			//We're going DOWN
		{
			if(myManager.CurrentFloorNum > 0)
				//We are not at the bottom - so move down!
			{
				myManager.GotoFloorNum = myManager.CurrentFloorNum - 1;
				myElevator.ActiveButton = this;
				myElevator.StartCoroutine("startElevator");
			}
			else
			{
				//test to see if button attached to the elevator
				if((buttonOffSFX != null))
				{
					GetComponent<AudioSource>().clip = buttonOffSFX;
					GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	void eleButtonFloorSwitch()
	{
		if ((CallFloor != myManager.CurrentFloorNum)&&(myManager.FloorsArray.Length >= CallFloor))
		{
			//Debug.Log ("Called Floor = "+ CallFloor);
			myManager.GotoFloorNum = CallFloor;
			//lightItUp(true);
			myElevator.ActiveButton = this;
			myElevator.StartCoroutine("startElevator");
		}
		else
		{
			//Debug.Log ("Pressed same button as current floor!");
			//test to see if button attached to the elevator
			if((buttonOffSFX != null))
			{
				GetComponent<AudioSource>().clip = buttonOffSFX;
				GetComponent<AudioSource>().Play();
			}
		}

	}

	public int materialIndex = 0;
	public string textureName = "_EmitTex";
	public bool illumOn = false;
	public float howBright = 1.0f;
	public float offValue = 0.0f;
	public AudioClip buttonOnSFX;
	public AudioClip buttonOffSFX;
	
	public void lightItUp(bool illumOn)
	{
		//offValue = renderer.materials[ materialIndex ].GetFloat("_EmitPow");
		
		if(illumOn == true)
		{
			if( GetComponent<Renderer>().enabled )
			{
				//Debug.Log ("True called");
				GetComponent<Renderer>().materials[ materialIndex ].SetFloat("_EmitPow", offValue + howBright);
				if((buttonOnSFX != null))
				{
					GetComponent<AudioSource>().clip = buttonOnSFX;
					GetComponent<AudioSource>().Play();
				}
			}
		}
		else
		{
			if( GetComponent<Renderer>().enabled )
			{
				//Debug.Log ("False called");
				GetComponent<Renderer>().materials[ materialIndex ].SetFloat("_EmitPow", offValue);
				if((buttonOffSFX != null))
				{
					GetComponent<AudioSource>().clip = buttonOffSFX;
					GetComponent<AudioSource>().Play();
				}
			}
		}
	}
	void FindGameCamera()
	{
		GameObject taggedObject = GameObject.FindWithTag("MainCamera");
		
		if (taggedObject != null)
		{
			//Debug.Log("taggedObject = " + taggedObject.name);
			Camera foundCamera = taggedObject.GetComponent<Camera>();
			if (foundCamera != null)
				GameCamera = foundCamera;
		}
		else
		{
			Debug.Log("Main Camera not found! Assign a Camera Object to the elevatorButton scripts.");
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
			Debug.Log ("There are no ElevatorManager scripts assigned in this scene. Elevator buttons will not work without these scripts. Please see the manual for assistance");
		}
	}

	void FindMyElevator()
	{
		elevators = Object.FindObjectsOfType(typeof(Elevator)) as Elevator[];
		elevatorTotal = elevators.Length;

		if (elevators.Length != 0)
			//Choose the manager which is a parent of the button
			for(int i=0; i<elevatorTotal; i++)
		{
			Elevator aElevator = (Elevator) elevators[i];
			//Debug.Log ("Testing..." + aElevator);
			if(aElevator.transform.IsChildOf(myManager.transform))
				myElevator = aElevator;
		}
		else
		{
			//Warn the user there are no managers in the scene
			Debug.Log ("There are no Elevator scripts assigned in this scene. Elevator buttons will not work without these scripts. Please see the manual for assistance");
		}
	}

}
