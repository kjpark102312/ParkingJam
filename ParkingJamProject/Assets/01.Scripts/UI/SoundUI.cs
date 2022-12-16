using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUI : MonoBehaviour
{
    public UIButton soundButton;
    [SerializeField] GameObject toggleSwitch;
    [SerializeField] GameObject toggleSwitchBG;


    [SerializeField] TweenPosition posTween;
    [SerializeField] TweenColor colorTween;

    public Color color;

    bool isOn = true;

    // Start is called before the first frame update
    void Start()
    {
        soundButton.onClick.Add(new EventDelegate(() =>
        {
            if(isOn)
            {
                posTween.enabled = true;
                colorTween.enabled = true;

                posTween.PlayForward();
                colorTween.PlayForward();

                isOn = !isOn;

                SoundManager.Instance.enabled = false;
            }
            else
            {
                posTween.PlayReverse();
                colorTween.PlayReverse();

                isOn = !isOn;

                SoundManager.Instance.enabled = true;
            }
            
        }));
    }

    void Update()
    {
        
    }
}
