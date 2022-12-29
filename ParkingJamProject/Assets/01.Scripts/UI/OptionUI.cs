using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{

    public UIButton returnButton;

    
    TweenPosition tp;



    // Start is called before the first frame update
    void Start()
    {
        tp = GetComponent<TweenPosition>();

        returnButton.onClick.Add(new EventDelegate(() =>
        {
            UITweener tween = TweenPosition.Begin(UIManager.Instance.GetUI(UIPanels.PauseUI), 0.2f, new Vector3(0, 0, 0));

            SoundManager.Instance.PlaySFXSound("ButtonClickSound");

            tp.enabled = true;
            tp.PlayReverse();
        }));

        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
