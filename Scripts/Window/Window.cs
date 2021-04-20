using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window : MonoBehaviour
{
    public virtual void Open()
    {
        this.gameObject.SetActive(true);
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
