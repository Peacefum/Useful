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
		//������������ ����Ͽ��� ������ �ȵȴ�




		//GetComponent<Renderer>().material.SetTexture("_MainTex", whiteMap);
	}

	RenderTexture getWhiteRT()
	{

		RenderTexture rt = new RenderTexture( 512 ,512, 32);
		Graphics.Blit(whiteMap, rt);

		return rt;
	}

	void CreateClearTexture() // �����ؽ��� ���� �����
	{
		whiteMap = new Texture2D(1, 1);
		whiteMap.SetPixel(0, 0, Color.clear); // ���� �����ؽ��� ����  ������ WHITE
		whiteMap.Apply();
	}
}
