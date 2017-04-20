using UnityEngine;
using System.Collections;

public class Shapes : MonoBehaviour {
    
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            GameObject.Find("GamePlay").GetComponent<Gameplay>().GameOver();
            print(other.name + " Hit!");           
        }
    }

}
