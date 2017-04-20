using UnityEngine;
using System.Collections;

public class RagdollParts : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mark")
        {
            other.transform.parent.transform.parent.transform.parent.GetComponent<Obstacles>().markContactScore++;
        }
    }
}
