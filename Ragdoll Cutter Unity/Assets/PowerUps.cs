using UnityEngine;
using System.Collections;

public class PowerUps : MonoBehaviour {

    public Gameplay gameplayScript;

    //Slowmotion
    public float timeScaleSlowMo;
    public float slowMoManaDrain;
    //Shield
    public int shieldCost;
    public int shieldDuration;
    //Nuke
    public int nukeCost;


    void Start() {

    }

    public void SlowMotion()
    {
        print("SlowStarted");
        Time.timeScale = timeScaleSlowMo;
        gameplayScript.currentMana -= Time.deltaTime * slowMoManaDrain;
        if (gameplayScript.currentMana < 0)
        {
            SlowMotionOff();
        }
    }

    public void SlowMotionOff()
    {
        CancelInvoke("SlowMotion");
        Time.timeScale = 1;
        print("SlowFinished");
    }

    public void ShieldOn()
    {
        if(gameplayScript.currentMana >= shieldCost)
        {
            gameplayScript.currentMana -= shieldCost;
            gameplayScript.hasShield = true;
            StartCoroutine(ShieldOff());
        }
    }

    IEnumerator ShieldOff()
    {
        yield return new WaitForSeconds(shieldDuration);
        if (gameplayScript.hasShield == true) gameplayScript.hasShield = false;
    }

    public void NukeMode()
    {
        //TODO: deactivate all active obstacles and move to 
        //out of view location. Play particles or something at each
        //of their positions
    }
}
