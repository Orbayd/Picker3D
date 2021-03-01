using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Show(bool IsVisible)
    {
        this.gameObject.SetActive(IsVisible);
    }
}
