using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    private float _skipTime = 60f;
    private float _curTime = 0f;

    [SerializeField] GameObject skipUI;

    [SerializeField] RewardAdMob mob;

    void Start()
    {
        skipUI.GetComponentInChildren<UIButton>().onClick.Add(new EventDelegate(() =>
        {
            SoundManager.Instance.PlaySFXSound("ButtonClickSound");
            //mob.ShowAd();
        }));
    }

    void Update()
    {
        _curTime += Time.deltaTime;
        if (_curTime >= _skipTime)
            skipUI.gameObject.SetActive(true);
    }
}
