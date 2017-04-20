using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour {

    public float score;
    public float currentMana;
    public float multiplier;
    public int maxMana;
    public bool hasShield;  //Shield PowerUp


	void Start () {
        InvokeRepeating("GamePlaying", 0.02f, 0.02f);
        currentMana = maxMana;
	}
	
	
	void GamePlaying () {
	    
	}

    public void GameOver()
    {
        print("GameOver");
    }
}
