using UnityEngine;
using System.Collections;

public class moveUpDown : MonoBehaviour {

	public float height = 3.2f;
	public float speed = 2.0f;
	public float originalTexVPos;
	public float totalTime = 5.0f;

	public AnimationCurve scrollVCurve;

	float ElapsedTime = 0.0f;

	void Start()
	{
		originalTexVPos = transform.position.y;

		StartCoroutine("upDown");

	}

	IEnumerator upDown()
	{
		while(ElapsedTime < totalTime)
		{
			float texVPos = scrollVCurve.Evaluate(ElapsedTime/totalTime) * 1.0f;
			//print (texVPos);
			transform.position = new Vector3(transform.position.x, (originalTexVPos + texVPos), transform.position.z);
			yield return null;
			ElapsedTime += Time.deltaTime;
		}

	}
}
