using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartUI : MonoBehaviour
{
    public UIButton restartButton;
    void Start()
    {
        restartButton.onClick.Add(new EventDelegate(() =>
        {
            SoundManager.Instance.PlaySFXSound("ButtonClickSound");
            LoadSceneManager.Instance.ReloadScene();
            Debug.Log("?? ??????");
        }));
    }
}
