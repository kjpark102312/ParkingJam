using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public UIButton optionButton;
    public TweenPosition optionPaneltp;

    public UIButton returnButton;
    private TweenPosition thisTp;

    public TweenPosition[] widgetTps;
    void Start()
    {
        thisTp = GetComponent<TweenPosition>();

        for (int i = 0; i < widgetTps.Length-1; i++)
        {
            Debug.Log(i);   
            widgetTps[i].onFinished.Add(new EventDelegate(() =>
            {
                for (int j = 0; j < widgetTps.Length; j++)
                    widgetTps[j].PlayReverse();
            }));
        }


        optionButton.onClick.Add(new EventDelegate(() =>
        {
            UITweener tween = TweenPosition.Begin(this.gameObject, 0.2f, new Vector3(-406f, 0, 0));

            for (int i = 0; i < widgetTps.Length; i++)
            {
                widgetTps[i].enabled = true;
                widgetTps[i].PlayForward();
                Debug.Log(i);
            }
            
            optionPaneltp.enabled = true;
            optionPaneltp.PlayForward();

        }));

        returnButton.onClick.Add(new EventDelegate(() =>
        {
            UITweener tween = TweenPosition.Begin(this.gameObject, 0.2f, new Vector3(406f, 0, 0));
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayReverse();
            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.LvUI).SetActive(true);

        }));
    }

    public void Reverse()
    {
    }

    void Update()
    {
        
    }
}
