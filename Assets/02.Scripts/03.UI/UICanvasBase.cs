using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    protected Canvas canvas;
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public virtual void ShowPanel()
    {
        canvas.enabled = true;
    }
    public virtual void HidePanel()
    {
        canvas.enabled = false;
    }
}
