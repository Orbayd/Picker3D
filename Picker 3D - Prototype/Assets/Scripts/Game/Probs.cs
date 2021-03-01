using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProbType
{
    SPHERE,CUBE, CYLINDER ,SPHERE_EXPLOSIVE , CUBE_EXPLOSIVE
}

public class Probs : MonoBehaviour
{
    public ProbType ProbType;

    public Vector3 InitalAngularVelocity;
    public Vector3 InitalVelocity;

    public void Init()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Init(Vector3.zero, Vector3.zero);
    }
    public void Init(Vector3 velocity,Vector3 angularVelocity)
    {
        InitalAngularVelocity = angularVelocity;
        InitalVelocity = velocity;
    }

    public void Begin()
    {
        this.GetComponent<Rigidbody>().velocity = InitalAngularVelocity;
        this.GetComponent<Rigidbody>().angularVelocity = InitalVelocity;
    }
   
}
