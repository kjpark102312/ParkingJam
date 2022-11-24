using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageUI : MonoBehaviour
{
    [SerializeField] UIButton nextStageButton = null;

    [SerializeField] TweenAlpha ta = null;
    [SerializeField] TweenPosition tp = null;
    [SerializeField] TweenScale tc = null;

    [SerializeField] TweenAlpha backgroundTa = null;


    void Start()
    {
        nextStageButton.onClick.Add(new EventDelegate(() =>
        {
            StageManager.Instance.curStageIndex++;
            PlayerPrefs.SetInt("Stage", StageManager.Instance.curStageIndex++);

            LoadSceneManager.Instance.NextScene();
        }));

        ta.onFinished.Add(new EventDelegate(() =>
        {
            tp.enabled = true;
            tc.enabled = true;
            

            tp.PlayForward();
            tc.PlayForward();
        }));
    }

    public void OnNextStageUI()
    {
        backgroundTa.enabled = true;
        backgroundTa.PlayForward();

        ta.enabled = true;
        ta.PlayForward();
    }
}
