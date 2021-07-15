using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public RectTransform _aimPoint;
    public Transform _particlePoint;

    public ParticleSystem ps;

    public GameObject _linePrefab;
    public LineRenderer _currentLinespec;

    public Transform _linePool;

    public Color _currentColor;

    public enum CurrentMode
    {
        NONE,DrawLine,ParticleLine
    }
    public CurrentMode _currentmode;

    public void InstallLine(Vector3 pos)
    {
        if(_currentLinespec == null)
        {
            GameObject g = Instantiate(_linePrefab, _linePool);
            g.transform.position = pos;

            _currentLinespec = g.GetComponent<LineRenderer>();
            _currentLinespec.startColor = _currentColor;
        }
        else
        {
            AddLine(pos);
        }

    }

    public void AddLine(Vector3 pos)
    {
        _currentLinespec.positionCount++;
        _currentLinespec.SetPosition(_currentLinespec.positionCount-1, pos);
    }
    public void DisableLine()
    {
        _currentLinespec = null;
    }



    /// <summary>
    /// ////////////////////////////////////////////
    /// </summary>
    public void PsStop()
    {
        var pss = ps.emission;
        pss.rateOverDistance = 0;

        //var psss = ps.noise;
        //psss.enabled = true;

    }

    public void PsStart()
    {
        var pss = ps.emission;
        pss.rateOverDistance = 100;

        //var psss = ps.noise;
        //psss.enabled = false;
    }

    public void ChangeMode()
    {
        ++_currentmode;

        if (_currentmode > CurrentMode.ParticleLine)
        {
            _currentmode = CurrentMode.NONE;
        }

    }

    public void LineStarter()
    {
        Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, _aimPoint.position); // 와 위에거 한줄로 끝

        _particlePoint.position = ray.direction + ray.origin;



        if (GetComponent<MouseAim>().isPressed == true)
        {
            switch (_currentmode)
            {
                case CurrentMode.NONE:
                    break;
                case CurrentMode.DrawLine:
                    InstallLine(_particlePoint.position);
                    break;
                case CurrentMode.ParticleLine:
                    PsStart();
                    break;
            }
        }
        else
        {
            DisableLine();
            PsStop();
        }

    }

}


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
