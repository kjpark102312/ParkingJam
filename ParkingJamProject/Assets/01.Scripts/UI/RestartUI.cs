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
            LoadSceneManager.Instance.ReloadScene();
            Debug.Log("æ¿ ¿ÁΩ√¿€");
        }));
    }
}
