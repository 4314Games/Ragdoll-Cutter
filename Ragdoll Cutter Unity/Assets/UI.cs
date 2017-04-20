using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

    //Other scripts
    public Gameplay gamplayScript;
    public PowerUps powerUpsScript;
    public GameObject gameplayParent;
    //GamePlay
    public TextMesh multiplierText;
    public TextMesh scoreText;
    public TextMesh manaText;
    
    
    //Slowmotion
    bool slowMoStatus;
   

	void Start () {
        InvokeRepeating("GamePlaying", 0.02f, 0.005f);
        slowMoStatus = false;        
	}
	
	void GamePlaying()
    {
        scoreText.text = gamplayScript.score.ToString("F0");
        manaText.text = gamplayScript.currentMana.ToString("F0") + " / " + gamplayScript.maxMana.ToString();
        multiplierText.text = gamplayScript.multiplier + 1.ToString();
        if (Input.GetMouseButtonDown(0))
        {
            FindButtonHit();
        }
    }

    void FindButtonHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.name == "SlowButton")  //Slomotion Button
            {
                print("HitSlow");
                if (slowMoStatus == false)  //Slow motion on
                {
                    powerUpsScript.InvokeRepeating("SlowMotion", 0.02f, 0.02f);
                    slowMoStatus = true;
                }
                else if(slowMoStatus == true)   //slow motion off
                {
                    powerUpsScript.SlowMotionOff();
                    slowMoStatus = false;
                }
            }
            
        }
    }

}
