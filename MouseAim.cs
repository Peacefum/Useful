using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour
{
    public Transform AimPoint;
    public ParticleSystem AimPointeffect;

    public int bulletLeft = 5;

    public bool isPressed = false;

    void Start()
    {

    }


    void Update()
    {
        if(Input.GetMouseButton(0))
        MousePositionAim();


        if(Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }

        if(isPressed == false)
        {
            ToCenter();
        }

        //if (Input.GetMouseButtonDown(0)){
        //    Shot();
        //}

    }
    public void MousePositionAim() // 이미지가 마우스 따라감
    {
        isPressed = true;

        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        AimPoint.position = Camera.main.ScreenToViewportPoint(screenPoint) ;
        Vector3 d = new Vector3(AimPoint.position.x * Camera.main.pixelWidth,
            AimPoint.position.y * Camera.main.pixelHeight);
        AimPoint.position =   d; 
    }
    public void Shot()
    {
        AimPointeffect.Play();
        if(bulletLeft >0)
        {
            bulletLeft--;
        }
        else
        {
            Debug.Log("빈총");
        }
    }

    public void ToCenter() // 센터로
    {
        var PointerVector = AimPoint.position;

        Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        PointerVector = center;
        AimPoint.position = Vector3.Lerp(AimPoint.position, PointerVector, Time.deltaTime * 5f) ;
    }
}
