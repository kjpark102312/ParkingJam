using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI Panel들을 열거형으로 정리
/// </summary>
public enum UIPanels
{
    Title,
    PauseUI,          
    OptionUI,
    LeftUI,
    RightUI,
    LvUI,
    BackGround,
    Exit,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _mainUI = null; 
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }
    static UIManager _instance = null;

    private Dictionary<UIPanels, GameObject> uiPanelDic = new Dictionary<UIPanels, GameObject>();
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this; _instance._mainUI = GameObject.Find("MainUI");
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += (scene, loadSceneMode) =>
        {
            uiPanelDic.Clear();
        };
    }

    TweenAlpha tween;
    void Start()
    {
        tween = GetComponentInChildren<TweenAlpha>();
    }

    public void GameOverTween()
    {
        Debug.Log("GameOver");
        tween.enabled = true;
    }

    public GameObject GetUI(UIPanels panel)
    {
        if (uiPanelDic.ContainsKey(panel))
            return uiPanelDic[panel];
        else
        {
            GameObject obj = _mainUI.transform.Find(panel.ToString()).gameObject;


            if (obj != null)
            {
                uiPanelDic[panel] = obj;
            }

            return obj;
        }
    }
}
