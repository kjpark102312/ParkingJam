using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageUI : MonoBehaviour
{
    [SerializeField] UIButton nextStageButton = null;

    [SerializeField] UILabel goldText = null;

    [SerializeField] TweenAlpha ta = null;

    [SerializeField] TweenAlpha backgroundTa = null;


    void Start()
    {
        nextStageButton.onClick.Add(new EventDelegate(() =>
        {
            StageManager.Instance.curStageIndex++;
            PlayerPrefs.SetInt("Stage", StageManager.Instance.curStageIndex++);

            LoadSceneManager.Instance.NextScene();
        }));

        backgroundTa.onFinished.Add(new EventDelegate(() =>
        {
            ta.enabled = true;
            ta.PlayForward();
        }));
    }

    public void OnNextStageUI()
    {
        backgroundTa.enabled = true;
        backgroundTa.PlayForward();

        UIManager.Instance.DisableInGameUI();

        goldText.text = $"+{StageManager.Instance.getGoldCount}";
    }
}
