using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    [SerializeField]
    float _speed = 4.0f;

    private bool _isMoving = false;

    private Rigidbody _rigidbody;

    private List<Probs> _capturedProbs = new List<Probs>();
    
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.Log($"[Info] Could not find Rigid Body");
        }

    }
    float _horizontal = 0.0f;

    public void SetInput(float horizontal)
    {
        _horizontal = horizontal * 2.0f;
    }

    public void FixedUpdate()
    {
        if (_isMoving)
        {
            _rigidbody.MovePosition(transform.position
                    + (transform.forward * Time.fixedDeltaTime * _speed)
                    + transform.right * Time.fixedDeltaTime * _horizontal);
            
        }
    }

    public void Init(Vector3 position)
    {
        Debug.Log($"[INFO Init Position] {position}");
        var newPosition = position;
        newPosition.y +=  this.MinExtends().y;
        this.transform.position = newPosition;
    }

    public void Move()
    {
        _isMoving = true;
        
    }

    public void Stop()
    {
        _isMoving = false;
        _rigidbody.velocity = Vector3.zero;

        foreach (var prob in _capturedProbs)
        {
            prob.GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 1).normalized * 4.0f;
        }
       
    }

    public void OnTriggerEnter(Collider other)
    {
        var prob = other.gameObject.GetComponent<Probs>();
        if (prob != null)
        {
            //Debug.Log($"Object Captured {prob.name}");
            _capturedProbs.Add(prob);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        var probs = other.gameObject.GetComponent<Probs>();
        if (probs != null)
        {
            Debug.Log($"Object Lost {probs.name}");
            _capturedProbs.Remove(probs);
        }
    }

    //public Vector3 MaxExtends()
    //{
    //    var meshes = GetComponentsInChildren<MeshRenderer>();
    //    float max = -100;
    //    Vector3 position = this.transform.position;
    //    foreach (var mesh in meshes)
    //    {
    //        if (mesh.bounds.max.z > max)
    //        {
    //            max = mesh.bounds.extents.z;
    //            position = mesh.bounds.max;
    //        }
    //    }

    //    return position;
    //}
    //public Vector3 MinExtends()
    //{
    //    var meshes = GetComponentsInChildren<MeshRenderer>();
    //    float max = -100;
    //    Vector3 position = this.transform.position;
    //    foreach (var mesh in meshes)
    //    {    
    //        if (mesh.bounds.extents.y > max)
    //        {
    //            max = mesh.bounds.extents.y;
    //            position = mesh.bounds.extents;
    //        }
    //    }

    //    return position;
    //}

}
