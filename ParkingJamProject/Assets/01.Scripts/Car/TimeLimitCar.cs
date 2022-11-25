using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitCar : Car
{
    public GameObject timer;

    public GameObject carParents;

    [SerializeField] UISprite timerSprite;

    public List<GameObject> targetCars = new List<GameObject>();

    float time = 15f;

    int passCarCount = 0;

    private void Start()
    {
        timer = FindObjectOfType<CarTimer>().gameObject;
        timerSprite = timer.transform.GetChild(0).GetComponent<UISprite>();

        for (int i = 0; i < carParents.transform.childCount; i++)
        {
            targetCars.Add(carParents.transform.GetChild(i).gameObject);
        }
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
                    if(carParents.transform.GetChild(i).gameObject == targetCars[i])
                    {
                        Debug.Log("PassCount");

                        targetCars.RemoveAt(i); 

                        time += 0.5f;   
                        passCarCount++;
                    }
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
    