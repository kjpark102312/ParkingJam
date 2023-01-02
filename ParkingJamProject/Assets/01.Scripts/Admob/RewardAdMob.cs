using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAdMob : MonoBehaviour
{

    [SerializeField] private TweenAlpha backgroundTa;

    private RewardedAd rewardedAd;

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");

        StageManager.Instance.curStageIndex++;
        PlayerPrefs.SetInt("Stage", StageManager.Instance.curStageIndex++);

        LoadSceneManager.Instance.NextScene();
    }

    private void Start()
    {
        //CreateAndLoadRewardedAd();
    }

    public void ShowAd()
    {
        backgroundTa.enabled = true;
        backgroundTa.PlayForward();

        backgroundTa.onFinished.Add(new EventDelegate(() =>
        {
            if (this.rewardedAd.IsLoaded())
            {
                this.rewardedAd.Show();
            }
        }));
    }


}
