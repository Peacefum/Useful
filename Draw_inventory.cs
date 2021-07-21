using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw_inventory : MonoBehaviour
{
    public GameObject _slotItem;
    public RectTransform _parentPanel;

    public List<Material> _mats = new List<Material>();
    public List<Image> _matsList = new List<Image>();
    //머테리얼 리스트하고 이미지 리스트 
    //모드에 따라서 가지고 있는 칸이 달라진다
    //칸이 있는것  = 라인, 파티클라인, 데칼, 바닥에그리기, 드래그인벤

    private float padding = 45;
    int i = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        INITSlot(); // 메뉴생성
    }
    private void OnDisable()
    {
         // 메뉴끄기? 
    }
    private void INITSlot()
    {
        float x = _parentPanel.rect.width / (_slotItem.GetComponent<RectTransform>().rect.width+ padding);
        float y = _parentPanel.rect.height / (_slotItem.GetComponent<RectTransform>().rect.height + padding);
        int matxnum = _mats.Count;

        //Debug.Log(Mathf.RoundToInt(x));
        // Debug.Log(Mathf.RoundToInt(y));

        for( int i=0; i< Mathf.RoundToInt(y); i++)
        {

            for ( int j = 0; j< Mathf.RoundToInt(x) ; j++)
            {

                if (matxnum <= 0) break;
                matxnum--;
                Debug.Log(matxnum);

                GameObject g = Instantiate(_slotItem);
                g.transform.parent = _parentPanel;

                

                g.transform.position = _slotItem.transform.position;
                g.transform.position =
                new Vector3(g.transform.position.x + (g.GetComponent<RectTransform>().rect.width + padding) * j,
                    g.transform.position.y + (-g.GetComponent<RectTransform>().rect.height + -padding) * i);

                //  g.GetComponentInChildren<Image>().sprite = _mats[i + j].;
                g.transform.GetChild(0).GetComponent<Image>().material =
                    _mats[matxnum];


                _matsList.Add( g.GetComponent<Image>());
                //이미지리스트에 저장

            }

        }

        _slotItem.SetActive(false);

    }

}
