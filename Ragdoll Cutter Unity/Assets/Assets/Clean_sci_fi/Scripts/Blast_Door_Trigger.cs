using UnityEngine;
using System.Collections;

public class Blast_Door_Trigger : MonoBehaviour {
	
	//public Transform B_Door_L;
	public GameObject DoorsParent;
	
	void OnTriggerEnter (Collider other)
	{
		//DoorsParent.BroadcastMessage ("BD_Open", SendMessageOptions.RequireReceiver);
	}
	
	void OnTriggerStay (Collider other)
	{

	}
	
	void OnTriggerExit (Collider other)
	{
		//DoorsParent.BroadcastMessage ("BD_Close", SendMessageOptions.RequireReceiver);
	}
}
