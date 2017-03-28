using UnityEngine;
using System.Collections;

public class AnimatedUVs : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 0.0f, 1.0f );
	public float rateMutilplier = 1.0f;
	public string textureName = "_MainTex";
	public bool boolUV = false;
	
	Vector2 uvOffset = Vector2.zero;

	void scrollUV(bool scrollOn)
	{
		if(scrollOn == true)
		{
			uvOffset += ( (uvAnimationRate * rateMutilplier) * Time.deltaTime );
			if( GetComponent<Renderer>().enabled )
			{
				GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
			}
		}
	}

	void LateUpdate() 
	{
		scrollUV(boolUV);
	}
}