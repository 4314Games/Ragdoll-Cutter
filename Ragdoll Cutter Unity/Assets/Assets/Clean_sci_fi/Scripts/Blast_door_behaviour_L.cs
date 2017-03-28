using UnityEngine;
using System.Collections;

public class Blast_door_behaviour_L : MonoBehaviour {

	public Object targetDoor;
	public ParticleSystem targetParticles;
	public AudioClip doorOpenClip;
	public AudioClip doorCloseClip;
	
	void DoDoorTrigger (bool openOrClose)
	{
		Object currentTarget = targetDoor != null ? targetDoor : gameObject;
		Behaviour targetBehaviour = currentTarget as Behaviour;
		GameObject targetGameObject = currentTarget as GameObject;

		
		if (targetBehaviour != null)
		
			targetGameObject = targetBehaviour.gameObject;
			
		
		//door opening = true, closing = false
		if (openOrClose == true)
			{
				Debug.Log ("Door opening!");
				//targetGameObject.animation.Play ("Room_door_open");
				targetGameObject.GetComponent<Animation>().CrossFade ("B_Door_L_open");
				GetComponent<AudioSource>().clip = doorOpenClip;
				GetComponent<AudioSource>().Play ();
				if (targetParticles != null) targetParticles.Play();
			}
		else
			{
				//targetGameObject.animation.Play ("Room_door_close");
				targetGameObject.GetComponent<Animation>().CrossFade ("B_Door_L_close");
				GetComponent<AudioSource>().clip = doorCloseClip;
				GetComponent<AudioSource>().Play ();
				if (targetParticles != null) targetParticles.Play();
			}
		
	}

	void OnTriggerEnter (Collider other) {
		DoDoorTrigger (true);
	}
	
	void OnTriggerExit (Collider other) {
		DoDoorTrigger (false);
	}
}
