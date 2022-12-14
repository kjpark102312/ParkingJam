using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanel : MonoBehaviour
{

    [Header("퍼즈창")]
    public UIButton pauseButton;
    public TweenPosition pausePaneltp;

    [SerializeField] UILabel goldText;


    [Header("챌린지창")]
    public UIButton challengeButton;
    public TweenPosition challengePaneltp;


    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.Add(new EventDelegate(() => {

            SoundManager.Instance.PlaySFXSound("ButtonClickSound");

            pausePaneltp.enabled = true;

            UITweener tween = TweenPosition.Begin(pausePaneltp.gameObject, 0.2f, Vector3.zero);

            UIManager.Instance.GetUI(UIPanels.BackGround).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayForward();

            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.InGameUI).SetActive(false);

            GameManager.Instance.IsPause = true;

            goldText.text = PlayerPrefs.GetInt("Gold").ToString();

        }));


        challengeButton.onClick.Add(new EventDelegate(() =>
        {
            SoundManager.Instance.PlaySFXSound("ButtonClickSound");

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
