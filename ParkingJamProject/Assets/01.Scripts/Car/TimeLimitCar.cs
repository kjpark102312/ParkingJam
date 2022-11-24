using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitCar : Car
{
    public GameObject timer;

    public GameObject carParents;

    [SerializeField] UISprite timerSprite;


    float time = 15f;

    private void Start()
    {
        timer = FindObjectOfType<CarTimer>().gameObject;
        timerSprite = timer.transform.GetChild(0).GetComponent<UISprite>();
    }

    void Update()
    {

        Vector3 screenPos = UICamera.mainCamera.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(transform.position));

        timer.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

        if(!isGameOver)
        {
            for (int i = 0; i < carParents.transform.childCount; i++)
            {
                if(carParents.transform.GetChild(i).GetComponent<Car>().isPassing)
                {
                    Debug.Log(carParents.transform.GetChild(i));
                    return;
                }
            }

            time -= Time.deltaTime;

            timerSprite.fillAmount = time / 15;

            if (time <= 0)
            {
                isGameOver = true;
                UIManager.Instance.GameOverTween();
            }
        }
    }
}
