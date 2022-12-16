using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isPause = true;
    public bool IsPause
    {
        get
        {
            return _isPause;
        }

        set
        {
            _isPause = value;
            _onPauseChanged(_isPause);
        }
    }

    private bool _isVibrate = true;
    public bool IsVibrate
    {
        get
        {
            return _isVibrate;
        }

        set
        {
            _isVibrate = value;
        }
    }

    private bool _isOnSound = true;
    public bool IsOnSound
    {
        get
        {
            return _isOnSound;
        }

        set
        {
            _isOnSound = value;
        }
    }

    public Action<bool> _onPauseChanged = (_isPaused) => { };

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = Instantiate(new GameObject());
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        //DontDestroyOnLoad(_instance.gameObject);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { _onPauseChanged = (x) => { }; };
    }
}
