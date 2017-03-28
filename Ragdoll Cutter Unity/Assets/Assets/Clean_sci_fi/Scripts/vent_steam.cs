using UnityEngine;
using System.Collections;

public class vent_steam : MonoBehaviour {
	
	public ParticleSystem targetParticles;
	public AudioClip steamHiss;
	//public int randSeed;
	private float playDelay;
	//private float waitToStop;

	void Start()
	{
		playDelay = Random.Range (0.0f,5.0f);
		targetParticles = this.GetComponent<ParticleSystem>();
		StartCoroutine (ventRandomSteam());
	}

	void Update()
	{
		//if the effect is playing wait a random amount then turn it off...
		if(targetParticles.isPlaying)
		{
			//waitToStop = Random.Range (0.0f,5.0f);
		}
	}

	IEnumerator ventRandomSteam()
	{
		yield return new WaitForSeconds (playDelay);
		Debug.Log ("Steam playing: start = " + playDelay);
		if(steamHiss !=null){
		GetComponent<AudioSource>().clip = steamHiss;
		GetComponent<AudioSource>().Play ();
		}
		if (targetParticles != null)
		{
			targetParticles.Play();
		}
		
	}
}
