using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawMatClick : MonoBehaviour
{
    DrawLine d;
    Button b;
    private void Start()
    {
        b = GetComponent<Button>();

        d = FindObjectOfType<DrawLine>();
        b.onClick.AddListener(() => { MatClick(); });
    }
    public void MatClick()
    {
      d._currentLineMat = 
            transform.GetComponent<Image>().material;
    }
}
