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
}
