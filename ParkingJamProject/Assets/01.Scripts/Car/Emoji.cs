using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoji : MonoBehaviour
{
    TweenScale ts;

    void Start()
    {
        ts = GetComponent<TweenScale>();
        ts.enabled = false;
    }

    public void Play(Vector3 screenPos)
    {
        transform.position = screenPos;

        ts.enabled = true;
        ts.PlayForward();
    }

    public void CallBack()
    {
        ts.delay = 0.2f;
        ts.PlayReverse();
    }
}
