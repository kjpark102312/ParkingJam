using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetsUI : MonoBehaviour
{
    TweenPosition tp;


    public Vector3 pos;
    public float duration;
    public float delay;

    void Start()
    {
        tp = GetComponent<TweenPosition>();


    }
}
