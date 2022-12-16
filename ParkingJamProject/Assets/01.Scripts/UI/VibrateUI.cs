using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateUI : MonoBehaviour
{
    public UIButton vibrateButton;
    [SerializeField] GameObject toggleSwitch;
    [SerializeField] GameObject toggleSwitchBG;


    [SerializeField] TweenPosition posTween;
    [SerializeField] TweenColor colorTween;
    public Color color;

    bool isOn = true;

    void Start()
    {
        vibrateButton.onClick.Add(new EventDelegate(() =>
        {
            if (isOn)
            {
                posTween.enabled = true;
                colorTween.enabled = true;

                posTween.PlayForward();
                colorTween.PlayForward();

                isOn = !isOn;
                GameManager.Instance.IsVibrate = false;
            }
            else
            {
                posTween.PlayReverse();
                colorTween.PlayReverse();

                isOn = !isOn;

                GameManager.Instance.IsVibrate = true;
            }
        }));
    }

    void Update()
    {
        
    }
}
