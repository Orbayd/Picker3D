using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void DelayedAction(this MonoBehaviour monoBehaviour, Action callback, float delay)
    {
        monoBehaviour.StartCoroutine(DeleyadactionInner(callback, delay));
    }
    static IEnumerator DeleyadactionInner(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    static public Vector3 MaxExtends(this Picker picker)
    {
        var meshes = picker.GetComponentsInChildren<MeshRenderer>();
        float max = -100;
        Vector3 position = picker.transform.position;
        foreach (var mesh in meshes)
        {
            if (mesh.bounds.max.z > max)
            {
                max = mesh.bounds.extents.z;
                position = mesh.bounds.max;
            }
        }

        return position;
    }
    static public Vector3 MinExtends(this Picker picker)
    {
        var meshes = picker.GetComponentsInChildren<MeshRenderer>();
        float max = -100;
        Vector3 position = picker.transform.position;
        foreach (var mesh in meshes)
        {
            if (mesh.bounds.extents.y > max)
            {
                max = mesh.bounds.extents.y;
                position = mesh.bounds.extents;
            }
        }

        return position;
    }
}
