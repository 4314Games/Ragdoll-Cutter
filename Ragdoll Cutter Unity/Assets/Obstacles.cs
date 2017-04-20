using UnityEngine;
using System.Collections;

public class Obstacles : MonoBehaviour {

    public float speed;
    public bool activeStatus;
    bool addedScore;
    public int markContactScore;   //Has contact with all markers been made

	public void Starter ()      //activates obstacle. 
    {
        activeStatus = true;
        InvokeRepeating("Using", 0.02f, 0.02f);
        GetComponent<Animator>().SetBool("playing", true);
        addedScore = false;
        markContactScore = 0;
	}

    void Using()
    {
        transform.Translate(0, 0, speed);
        if (transform.localPosition.z < -0.65 && addedScore == false)
        {
            GameObject.Find("GamePlay").GetComponent<Gameplay>().multiplier++;    //Scores a point
            addedScore = true;
            print(markContactScore);
        }
        else if(transform.localPosition.z < 5.5f && transform.localPosition.z > 14)   //Cancels animations and sets for cutout position
        {
            GetComponent<Animator>().SetBool("playing", false);
        }
        else if (transform.localPosition.z < -14) Deactivate();          //When reaches certain point, deactivate
    }

    public void Deactivate()   //deactivate obstacle
    {
        GetComponent<Animator>().SetBool("playing", false);
        CancelInvoke("Using");
        GameObject.Find("ObstacleSpawnPoint").GetComponent<Spawner>().activeObstacles.Remove(gameObject);   //removes from actives list
        activeStatus = false;
    }
}
