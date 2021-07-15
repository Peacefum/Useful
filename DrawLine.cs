using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public RectTransform _aimPoint;
    public Transform _particlePoint;

    //void Update() // 에임포인트를 기점으로 앞으로 레이가 나가야함
    //{

    //    Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

    //    Vector3 vv = new Vector3( _aimPoint.anchoredPosition.x / center.x,
    //       _aimPoint.anchoredPosition.y / center.y);


    //    Ray ray = Camera.main.ViewportPointToRay(vv);
    //    ray.direction = Camera.main.transform.forward - (vv * -1); // 방향때문에 -1 곱

    //    Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f);

    //    //_particlePoint.position =
    //    //    new Vector3(ray.direction.x * 0.3f, ray.direction.y * 0.575f, ray.direction.z)  +ray.origin;  // 곡선으로됨 퍼스펙티브


    //    //오긴 오는데 이상하다 0714
    //    //Vector3 ss = Camera.main.WorldToScreenPoint(_aimPoint.position);
    //    //Debug.Log(ss);
    //    //Debug.DrawLine(ss, ss + Vector3.forward, Color.green, 5f);

    //}





    private void Update()
    {
        Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main,_aimPoint.position); // 와 위에거 한줄로 끝

        _particlePoint.position = ray.direction + ray.origin;
    }

}
