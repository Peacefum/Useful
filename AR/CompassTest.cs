using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARS;
using UnityEngine.UI;
using System;

public class CompassTest : MonoBehaviour
{
     public GameObject target;
    public Text headingText;

    private bool startTracking = false;

    public static float heights = 0;

    public bool isNoCompass = false;
    public GameObject placeObjects;

    void OnEnable()
    {
        Input.compass.enabled = true;
        Input.location.Start();


        if (isNoCompass == true) return;

        var b = target.transform.position;
        b.y =  heights;
        target.transform.position = b;
        StartCoroutine(InitializeCompass());
    }
    private void OnDisable()
    {
        Input.compass.enabled = false;
        Input.location.Stop();
    }
    public void RePositinoning()
    {
      //  target.transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        //var pu = testProbs.transform.localPosition;
        //pu.y = Camera.main.transform.position.y + 2;
        //testProbs.transform.localPosition = pu;
    }
    public void ButtonPosition()
    {
        //target.transform.rotation = Quaternion.Euler(0,tt,0);
        // StartCoroutine(Reposition());
        var r = target.transform.rotation;
        r.x = 0; r.z = 0;
        placeObjects.SetActive(true);
        placeObjects.transform.rotation = r;
    }
    private float compassSmooth = 0.5f;
    private float m_lastMagneticHeading = 0f;
    private float m_lastTrueHeading = 0f;
    public GameObject testProbs;
    IEnumerator Reposition()
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            //   var r  =  Compass3D.ObjectRotation.eulerAngles;
            //r.x = 0; r.z = 0;
            //var f = Compass3D.CameraRotation.eulerAngles;
            //f.x = 0; f.z = 0;


            //  target.transform.rotation = Quaternion.Euler(r); 
          //  Debug.Log(Input.compass.headingAccuracy);
            float currentMagneticHeading = (float)Math.Round(Input.compass.magneticHeading, 2);
            float currentTrueHeading = (float)Math.Round(Input.compass.trueHeading, 2);
            if (m_lastMagneticHeading < currentMagneticHeading - compassSmooth || m_lastMagneticHeading > currentMagneticHeading + compassSmooth)
            {
                m_lastMagneticHeading = currentMagneticHeading;
                target.transform.rotation = Quaternion.Euler(90, m_lastMagneticHeading,0); // 스프라이트가 90도 됨
                //    COmpasObjets.localRotation = Quaternion.Lerp(COmpasObjets.localRotation, Quaternion.Euler(0, 0, m_lastMagneticHeading), Time.deltaTime);
                //if (m_lastTrueHeading < currentTrueHeading - compassSmooth || m_lastTrueHeading > currentTrueHeading + compassSmooth)
                //{
                //    m_lastTrueHeading = currentTrueHeading;
                //    target.transform.rotation = Quaternion.Euler(90, m_lastTrueHeading, 0); 
                //}
            }
            yield return null;
        }
    }
    void Update()
    {


        if (isNoCompass == true) return;
        if(startTracking)
        {
            headingText.text = ((int)Input.compass.trueHeading).ToString() + "° " + DegreesToCardinalDetailed(Input.compass.trueHeading);


            //float currentMagneticHeading = (float)Math.Round(Input.compass.magneticHeading, 2);

            //if (m_lastMagneticHeading < currentMagneticHeading - compassSmooth || m_lastMagneticHeading > currentMagneticHeading + compassSmooth)
            //{
            //    m_lastMagneticHeading = currentMagneticHeading;
            //    target.transform.rotation = Quaternion.Lerp(target.transform.rotation, Quaternion.Euler(0, m_lastMagneticHeading, 0), Time.deltaTime);
            //    //    COmpasObjets.localRotation = Quaternion.Lerp(COmpasObjets.localRotation, Quaternion.Euler(0, 0, m_lastMagneticHeading), Time.deltaTime);
            //}


            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            Quaternion compass = Quaternion.Euler(0, -Input.compass.magneticHeading, 0);
            Quaternion north =  Quaternion.Euler(90, cameraRotation.eulerAngles.y + compass.eulerAngles.y, 0);
            target.transform.rotation = Quaternion.Lerp(target.transform.rotation, north,Time.smoothDeltaTime);

        }
    }

    IEnumerator InitializeCompass()
    {

        yield return new WaitForSeconds(1f); // 나침반을 1초 만 활성화후 끔
        startTracking |= Input.compass.enabled;
    }
    private static string DegreesToCardinalDetailed(double degrees)
    {
        string[] caridnals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
        return caridnals[(int)Math.Round(((double)degrees * 10 % 3600) / 225)];
    }
}


