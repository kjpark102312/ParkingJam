using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public UIButton optionButton;
    public TweenPosition optionPaneltp;


    public UIButton returnButton;
    private TweenPosition thisTp;

    void Start()
    {
        thisTp = GetComponent<TweenPosition>();
        //UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().onFinished.Add(new EventDelegate(() =>
        //{
        //    UIManager.Instance.GetUI(UIPanels.BackGround).gameObject.SetActive(false);
        //}));

        optionButton.onClick.Add(new EventDelegate(() =>
        {
            this.gameObject.SetActive(false);
            optionPaneltp.enabled = true;
            optionPaneltp.PlayForward();

        }));

        returnButton.onClick.Add(new EventDelegate(() =>
        {
            thisTp.PlayReverse();
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayReverse();
            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.LvUI).SetActive(true);

        }));
    }

    void Update()
    {
        
    }
}
