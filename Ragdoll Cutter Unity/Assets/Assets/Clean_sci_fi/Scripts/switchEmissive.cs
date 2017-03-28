using UnityEngine;
using System.Collections;

public class switchEmissive : MonoBehaviour {

	public int materialIndex = 0;
	public string textureName = "_Illum";
	public bool illumOn = false;
	public float howBright = 1.0f;
	private float offValue;
	
	public void lightItUp(bool illumOn)
	{
		offValue = GetComponent<Renderer>().materials[ materialIndex ].GetFloat("_EmissionLM");

		if(illumOn == true)
		{
			if( GetComponent<Renderer>().enabled )
			{
				GetComponent<Renderer>().materials[ materialIndex ].SetFloat("_EmissionLM", offValue + howBright);
			}
			else
			{
				GetComponent<Renderer>().materials[ materialIndex ].SetFloat("_EmissionLM", offValue);
			}
		}
	}
	
	void Update() 
	{
		//lightItUp(illumOn);
	}
}
