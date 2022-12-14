using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageUI : MonoBehaviour
{
    [SerializeField] UIButton nextStageButton = null;

    [SerializeField] UILabel goldText = null;

    [SerializeField] TweenAlpha ta = null;

    [SerializeField] TweenAlpha backgroundTa = null;

    [SerializeField] TweenRotation emoji = null;

    [SerializeField] private GoogleAdMob _adMob;

    void Start()
    {
        nextStageButton.onClick.Add(new EventDelegate(() =>
        {
            //if(Random.Range(0,3) != 0)
            //{
            //    _adMob.ShowAd();                
            //    return;
            //}

            SoundManager.Instance.PlaySFXSound("ButtonClickSound");

            StageManager.Instance.curStageIndex++;
            PlayerPrefs.SetInt("Stage", StageManager.Instance.curStageIndex++);

            LoadSceneManager.Instance.NextScene();
        }));

        backgroundTa.onFinished.Add(new EventDelegate(() =>
        {
            ta.enabled = true;
            ta.PlayForward();

            emoji.enabled = true;
            emoji.PlayForward();
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
