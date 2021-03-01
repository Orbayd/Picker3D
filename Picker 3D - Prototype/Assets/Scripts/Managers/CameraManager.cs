using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Basic Camera Follower
*/
public class CameraManager : ManagerBase
{
    public Camera Camera;

    public Vector3 Cameraoffset;

    public Vector3 CameraAngle;

    private Picker _picker;

   
    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = _picker.transform.position + Cameraoffset;
        Camera.transform.rotation = Quaternion.Euler(CameraAngle);
        //Camera.transform.LookAt(Picker.transform);
    }

    public override void Init(GameManager serviceLocator)
    {
        _picker = serviceLocator.Picker;
    }

}
