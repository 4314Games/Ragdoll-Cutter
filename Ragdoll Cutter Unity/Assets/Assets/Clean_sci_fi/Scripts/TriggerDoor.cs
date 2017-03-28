using UnityEngine;

public class TriggerDoor : MonoBehaviour {

	public Object target;
	
	void DoDoorTrigger (string myAnim, bool ventFX)
	{
		Object currentTarget = target != null ? target : gameObject;
		Behaviour targetBehaviour = currentTarget as Behaviour;
		GameObject targetGameObject = currentTarget as GameObject;

		
		if (targetBehaviour != null)
			targetGameObject = targetBehaviour.gameObject;
			targetGameObject.GetComponent<Animation>().Play (myAnim);
			targetGameObject.GetComponent<AudioSource>().Play ();
		if (ventFX == true)
			targetGameObject.GetComponentInChildren<ParticleSystem>().Play();
		else 
			targetGameObject.GetComponentInChildren<ParticleSystem>().Stop();
	}

	void OnTriggerEnter (Collider other) {
		DoDoorTrigger ("Room_door_open", true);

	}
	
	void OnTriggerExit (Collider other) {
		DoDoorTrigger ("Room_door_close", false);

	}

}