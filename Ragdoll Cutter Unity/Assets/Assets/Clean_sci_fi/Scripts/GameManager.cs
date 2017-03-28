using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public Elevator_doors[] Elevator_doorsArray;
	public int myInt = 15;
	

	// Use this for initialization
	void Awake ()
	{
		Elevator_doorsArray = Object.FindObjectsOfType(typeof(Elevator_doors)) as Elevator_doors[];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{

		}
	}
}
