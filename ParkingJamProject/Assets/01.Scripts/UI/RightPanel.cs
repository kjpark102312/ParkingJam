using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanel : MonoBehaviour
{

    [Header("퍼즈창")]
    public UIButton pauseButton;
    public TweenPosition pausePaneltp;

    [Header("챌린지창")]
    public UIButton challengeButton;
    public TweenPosition challengePaneltp;


    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.Add(new EventDelegate(() => {

            pausePaneltp.enabled = true;

            UITweener tween = TweenPosition.Begin(pausePaneltp.gameObject, 0.2f, Vector3.zero);

            UIManager.Instance.GetUI(UIPanels.BackGround).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayForward();

            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.InGameUI).SetActive(false);

            GameManager.Instance.IsPause = false;

        }));


        challengeButton.onClick.Add(new EventDelegate(() =>
        {
            challengePaneltp.enabled = true;

            UIManager.Instance.GetUI(UIPanels.BackGround).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayForward();

            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.InGameUI).SetActive(false);

            challengePaneltp.PlayForward();
        }));
    }
}
