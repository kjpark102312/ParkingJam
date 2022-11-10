using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    TweenAlpha tween;
    void Start()
    {
        tween = GetComponentInChildren<TweenAlpha>();
    }

    void Update()
    {
        
    }

    public void GameOverTween()
    {
        Debug.Log("GameOver");
        tween.enabled = true;
    }
}
