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
    InGameUI,
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

    private Dictionary<UIPanels, GameObject> _uiPanelDic = new Dictionary<UIPanels, GameObject>();
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
            _uiPanelDic.Clear();
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

    public void DisableInGameUI()
    {
        GetUI(UIPanels.InGameUI).SetActive(false);
        GetUI(UIPanels.LeftUI).SetActive(false);
        GetUI(UIPanels.RightUI).SetActive(false);
    }

    public GameObject GetUI(UIPanels panel)
    {
        if (_uiPanelDic.ContainsKey(panel))
            return _uiPanelDic[panel];
        else
        {
            GameObject obj = _mainUI.transform.Find(panel.ToString()).gameObject;


            if (obj != null)
            {
                _uiPanelDic[panel] = obj;
            }

            return obj;
        }
    }
}
