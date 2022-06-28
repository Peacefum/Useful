using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TouchEffect : MonoBehaviour
{
    public RectTransform rt;
    Sequence mysequency;
    void Start()
    {
        mysequency = DOTween.Sequence();
        rt.localScale = Vector3.zero;

        mysequency.Append(rt.DOScale(2, 1f)).Insert(0.5f, rt.GetComponent<Image>().DOColor(Color.clear, 0.5f));

        mysequency.SetAutoKill(false);
        mysequency.SetLoops(-1);
    }

}
