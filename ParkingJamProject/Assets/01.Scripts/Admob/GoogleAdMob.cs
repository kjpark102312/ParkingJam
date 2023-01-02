using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleAdMob : MonoBehaviour
{
    [SerializeField] private TweenAlpha backgroundTa;

    private InterstitialAd interstitial;

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif
        this.interstitial = new InterstitialAd(adUnitId);


        // 광고가 로드되면 실행
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // 광고 로드가 실패하면 실행
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // 광고가 표시되면 실행
        this.interstitial.OnAdOpening += HandleOnAdOpening;
        // 광고를 닫으면 실행.
        this.interstitial.OnAdClosed += HandleOnAdClosed;


        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError);
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");

        
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");

        StageManager.Instance.curStageIndex++;
        PlayerPrefs.SetInt("Stage", StageManager.Instance.curStageIndex++);

        LoadSceneManager.Instance.NextScene();
    }

    private void Start()
    {
        //RequestInterstitial();
    }

    public void ShowAd()
    {
        backgroundTa.enabled = true;
        backgroundTa.PlayForward();

        backgroundTa.onFinished.Add(new EventDelegate(() =>
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
        }));
    }
}
