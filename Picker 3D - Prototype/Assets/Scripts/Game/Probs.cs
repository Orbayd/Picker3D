using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProbType
{
    SPHERE,CUBE, CYLINDER
}

public class Probs : MonoBehaviour
{
    public ProbType ProbType;
    public void Init()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
   
}
