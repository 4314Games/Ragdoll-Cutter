using UnityEngine;
using System.Collections;

public class AnimationPath : MonoBehaviour 
{
	public AnimationCurve XCurve;
	
	public float TotalTravelTime = 5.0f;
	
	public float TravelSpeed = 50.0f;
	
	public float XRange = 10.0f;


	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine("Travel");
	}
	
	IEnumerator Travel()
	{
		float ElapsedTime = 0.0f;
		float originalXPos = transform.position.x;

		
		while(ElapsedTime < TotalTravelTime)
		{
			float XPos = XCurve.Evaluate(ElapsedTime/TotalTravelTime) * XRange;
			
			//transform.position = new Vector3(XPos, transform.position.y, transform.position.z + TravelSpeed * -Time.deltaTime) + originalPos;
			transform.position = new Vector3((originalXPos + XPos), transform.position.y, transform.position.z);
			
			yield return null;
			
			ElapsedTime += Time.deltaTime;
		}
	}
}
