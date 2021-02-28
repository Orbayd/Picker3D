using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Basic Camera Follower
*/
public class CameraManager : MonoBehaviour
{
    public Camera Camera;

    public Vector3 Cameraoffset;

    public Picker Picker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = Picker.transform.position + Cameraoffset;
        Camera.transform.LookAt(Picker.transform);
    }

    internal void Init(ServiceLocator serviceLocator)
    {
        Picker = serviceLocator.Picker;
    }
}
