using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RendPointer : MonoBehaviour
{
    public int resolution = 512;
     Texture2D whiteMap;
    public string currentwhiteMap; 
    public float brushSize;
    public Texture2D brushTexture;
    public Color brushColor;
    Vector2 stored;
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();

    public Transform brushPoint;
    public bool isDrawing = false;
    //텍스쳐 모아서 클리어 렌더텍스쳐 생성
    //[SerializeField]
    //Material mat;
    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
        MeshcolliderTransparenter();

    }

    public void MeshcolliderTransparenter() // 투명 머테리얼로 초기화  
    //텍스쳐 모아서 클리어 렌더텍스쳐 생성
    {
        foreach (MeshCollider ms in FindObjectsOfType<MeshCollider>())
        {
            Renderer rend = ms.GetComponent<Renderer>();
            paintTextures.Add(ms, getWhiteRT());
            rend.material.SetTexture("_MainTex", paintTextures[ms]);

       //     Material[] mat = ms.GetComponent<MeshRenderer>().materials;  // 지금은 메쉬콜라이더 가진애만 이지만 //메쉬콜라이더에 지정한  머테리얼 가진애로 바꿔야함
          //  mat[1].SetTexture("_MainTex", rend.material.GetTexture("_PaintMap"));
        }
    }

    void Update() // 업데이트가 빠르지 않으니 포인트를 두고 따라오는식으로 
    {

        Debug.DrawRay(transform.position, transform.forward * 20f, Color.magenta);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) // delete previous and uncomment for mouse painting
        {
            MeshCollider coll = hit.collider as MeshCollider;
            if (coll != null || coll.sharedMesh == null)
            {

                if (!paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                {

                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    paintTextures.Add(coll, getWhiteRT());
                    rend.material.SetTexture("_MainTex", paintTextures[coll]);

                    //  imag.texture = rend.material.GetTexture("_PaintMap"); 투명 이미지에 적용가능
                   // Material[] mat = hit.transform.GetComponent<MeshRenderer>().materials; // 머테리얼 두개로 만들어서 하나에 뒤집어 쓰기
               //     mat[1].SetTexture("_MainTex", rend.material.GetTexture("_PaintMap"));
                }

                    //    currentwhiteMap = hit.transform.name;
                    //}

                    if (isDrawing == true)
                    if (stored != hit.lightmapCoord) // stop drawing on the same point
                {

                    currentwhiteMap = hit.transform.name;
                    ////////////////////////////////////////////////////////------------------------------------------------------------------ 0628
                    // Vector3 uvWorldPosition = Vector3.zero;
                    ////////////////////////////////////////////////////////
                    stored = hit.lightmapCoord;
                    Vector2 pixelUV = hit.lightmapCoord;
                    pixelUV.y *= resolution;
                    pixelUV.x *= resolution;
                    //uvworldPosion.x와 y는 렌더텍스쳐의 가운데 위치에 있어야한듯하다
                    // Debug.Log(pixelUV); // meshcollider로 하니까 문제가 해결되었다 위치가 다른것인가?


                        DrawTexture(paintTextures[coll], pixelUV.x, pixelUV.y);

                }


                if(hit.collider)
                {
                    brushPoint.gameObject.SetActive(true);
                    brushPoint.rotation = Quaternion.Lerp(brushPoint.rotation, Quaternion.LookRotation( hit.normal),Time.deltaTime * 10); // 닿는 방향으로 회전
                    
                    brushPoint.position = hit.point - Vector3.forward *0.05f; // 
                }
                else
                {
                    brushPoint.gameObject.SetActive(false);
                }
            }
        }

        if(Input.GetMouseButton(0)) // bool 로 따로 만들어주어야함 /------------------------------------------------------------------------------ 0628
        {
            isDrawing = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }
    

    void DrawTexture(RenderTexture rt, float posX, float posY) // 콜라이더로 받은 텍스쳐 포지션에 정확히 매칭해 그림 // 위치를 렌더텍스쳐 위치로
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture; // 현재 드로우 하는 텍스쳐
        ///////---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 21 06 28 수정-------------------------
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        ///////-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size



        // draw brushtexture
        //Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture,null);//  brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture );
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture, 
            new Rect(0,0, 1f,1f),0,0,0,0,brushColor, null);//  brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture ); // 색도 같이  -- 0629

        Debug.Log(brushTexture.width + " , " + brushTexture.height);
        Debug.Log(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)));

        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture
    }

    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }

    void CreateClearTexture() // 렌더텍스쳐 원판 만들기
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.clear); // 원판 렌더텍스쳐 색깔  원래는 WHITE
        whiteMap.Apply();
    }

    //Texture2D toTexture2D(RenderTexture rTex)
    //{
    //    sourceTex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
    //    RenderTexture.active = rTex;
    //    sourceTex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
    //    sourceTex.Apply();
    //    RenderTexture.active = null;
    //    return sourceTex;
    //}
}
