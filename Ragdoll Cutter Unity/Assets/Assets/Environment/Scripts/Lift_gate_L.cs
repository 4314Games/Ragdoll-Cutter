using UnityEngine;
using System.Collections;

public class Lift_gate_L: MonoBehaviour {

	public Object targetGate;
	public ParticleSystem targetParticles;
	//public AudioClip gateOpenClip;
	//public AudioClip gateCloseClip;
	
	void DoDoorTrigger (bool openOrClose)
	{
		Object currentTarget = targetGate != null ? targetGate : gameObject;
		Behaviour targetBehaviour = currentTarget as Behaviour;
		GameObject targetGameObject = currentTarget as GameObject;

		
		if (targetBehaviour != null)
		
			targetGameObject = targetBehaviour.gameObject;
			
		
		//door opening = true, closing = false
		if (openOrClose == true)
			{
				Debug.Log ("Gates opening!");
				//targetGameObject.animation.Play ("Room_door_open");
				targetGameObject.GetComponent<Animation>().CrossFade ("Lift_gate_L_Close");
				//audio.clip = doorOpenClip;
				//audio.Play ();
				if (targetParticles != null) targetParticles.Play();
			}
		else
			{
				//targetGameObject.animation.Play ("Room_door_close");
				targetGameObject.GetComponent<Animation>().CrossFade ("Lift_gate_L_Open");
				//audio.clip = doorCloseClip;
				//audio.Play ();
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
