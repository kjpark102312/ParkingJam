using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeUI : MonoBehaviour
{
    public UIButton returnButton;

    private TweenPosition challengeTp;
    void Start()
    {
        challengeTp = GetComponent<TweenPosition>();
        returnButton.onClick.Add(new EventDelegate(() =>
        {
            challengeTp.PlayReverse();

            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.LvUI).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayReverse();
        }));
    }

    void Update()
    {
        
    }
}
