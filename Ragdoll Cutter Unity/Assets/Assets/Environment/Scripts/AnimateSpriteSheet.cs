using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class AnimateSpriteSheet : MonoBehaviour
{
	public int Columns = 5;
	public int Rows = 5;
	public float FramesPerSecond = 10f;
	public bool RunOnce = true;
	public int matIndex = 0;
	private int matTotal = 0;
	
	public float RunTimeInSeconds
	{
		get
		{
			return ( (1f / FramesPerSecond) * (Columns * Rows) );
		}
	}
	
	private Material materialCopy = null;
	
	void Start()
	{
		matTotal = gameObject.GetComponent<Renderer>().materials.Length;
		if(matIndex > matTotal)
		{
			matIndex = matTotal;
			Debug.Log("index is higher than total materials: setting at last material index");
		}
		// Copy its material to itself in order to create an instance not connected to any other
		//materialCopy = new Material(renderer.sharedMaterial);
		materialCopy = new Material(GetComponent<Renderer>().materials[matIndex]);
		//myMaterial = renderer.materials[0];
		//renderer.sharedMaterial = materialCopy;
		GetComponent<Renderer>().materials[matIndex] = materialCopy;
		
		Vector2 size = new Vector2(1f / Columns, 1f / Rows);
		//renderer.sharedMaterial.SetTextureScale("_MainTex", size);
		GetComponent<Renderer>().materials[matIndex].SetTextureScale("_MainTex", size);
	}
	
	void OnEnable()
	{
		StartCoroutine(UpdateTiling());
	}
	
	private IEnumerator UpdateTiling()
	{
		float x = 0f;
		float y = 0f;
		Vector2 offset = Vector2.zero;
		
		while (true)
		{
			for (int i = Rows-1; i >= 0; i--) // y
			{
				y = (float) i / Rows;
				
				for (int j = 0; j <= Columns-1; j++) // x
				{
					x = (float) j / Columns;
					
					offset.Set(x, y);
					
					//renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
					GetComponent<Renderer>().materials[matIndex].SetTextureOffset("_MainTex", offset);
					yield return new WaitForSeconds(1f / FramesPerSecond);
				}
			}
			
			if (RunOnce)
			{
				yield break;
			}
		}
	}
}