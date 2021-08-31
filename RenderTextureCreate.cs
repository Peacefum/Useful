using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

	Texture2D whiteMap;

    public void Awake()
    {
		CreateClearTexture();

		GetComponent<Renderer>().material.SetTexture("_MainTex", getWhiteRT());
		GetComponent<Renderer>().material.SetTexture("_BumpMap", getWhiteRT());
		GetComponent<Renderer>().material.SetTexture("_ParallaxMap", getWhiteRT());
		GetComponent<Renderer>().material.SetFloat("_BumpScale", -0.5f);


		
		//GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", getWhiteRT());
		//GetComponent<Renderer>().material.SetFloat("_GlossMapScale", 1f);
		//범프스케일은 모바일에서 적용이 안된다




		//GetComponent<Renderer>().material.SetTexture("_MainTex", whiteMap);
	}

	RenderTexture getWhiteRT()
	{

		RenderTexture rt = new RenderTexture( 512 ,512, 32);
		Graphics.Blit(whiteMap, rt);

		return rt;
	}

	void CreateClearTexture() // 렌더텍스쳐 원판 만들기
	{
		whiteMap = new Texture2D(1, 1);
		whiteMap.SetPixel(0, 0, Color.clear); // 원판 렌더텍스쳐 색깔  원래는 WHITE
		whiteMap.Apply();
	}
}
