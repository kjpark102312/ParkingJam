using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadingManager : MonoBehaviour
{
    private float _loadingTime = 0f;

    [SerializeField] private UISprite _loadingBar;

    void Update()
    {
        _loadingTime += Time.deltaTime;

        _loadingBar.fillAmount = _loadingTime / 3f;

        if (_loadingTime >= 3)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
