using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    public GameObject[] starterObstacles;

    public float spawnRate;
    public float startingRate;

    public List<GameObject> activeObstacles = new List<GameObject>();
    

	void Start () {
        PickObstacle();                
        spawnRate = startingRate;
	}
	
    void PickObstacle()
    {
        //TODO: obstalce is based on score.
        List<GameObject> availableObstacles = new List<GameObject>();   //List to store available obstacles
        for(int x = 0; x < starterObstacles.Length; x++)                                                                                ///Finds avaiblable obstales from current obstacle
        {                                                                                                                               // array and adds to list
            if (starterObstacles[x].GetComponent<Obstacles>().activeStatus == false) availableObstacles.Add(starterObstacles[x]);            
        }
        GameObject chosenObstacle = availableObstacles[Random.Range(0, availableObstacles.Count)];      //picks obstacle at random from list
        chosenObstacle.GetComponent<Obstacles>().Starter();                                             //actviate obstacle
        chosenObstacle.transform.position = transform.position;                                         //sets obstacle position
        chosenObstacle.transform.rotation = transform.rotation;
        activeObstacles.Add(chosenObstacle);                                                            //Adds obstacled to active list
        StartCoroutine(SpawnDelayer());                                                                 //End spawn, wait for new call
        //TODO: make spawndelayer decrease/spawn faster.
    }   

    IEnumerator SpawnDelayer()  //SpawnRate Control
    {
        yield return new WaitForSeconds(spawnRate);
        PickObstacle();
    }
    
	
	
}
