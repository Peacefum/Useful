using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SturfeeVPS.Core;
using SturfeeVPS.Providers;
using SturfeeVPS.SDK;
using SturfeeVPS;

public class ObjectMove : MonoBehaviour
{
    public JoyStick joystick;
    public float movespeed = 10f;

    private Vector3 _moveVector;
    private Transform _transform;

    public GameObject gpsUI;

    void Start()
    {
        gpsUI = GameObject.Find("GpsUI").gameObject;

        _transform = transform;
        _moveVector = Vector3.zero;

        joystick = FindObjectOfType<JoyStick>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void HandleInput()
    {
        _moveVector = PoolInput();
    }
    public Vector3 PoolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
          Vector3 dir = new Vector3(h, 0, v).normalized;
        
        return dir;
    }
    public void Move()
    {
        gpsUI.transform.position = new Vector3(_transform.position.x, _transform.position.y + 3f, _transform.position.z);
        _transform.Translate(  _moveVector * movespeed * Time.deltaTime,  Camera.main.transform);
    }
}
