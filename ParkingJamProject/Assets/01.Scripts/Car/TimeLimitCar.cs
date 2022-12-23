using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitCar : Car
{
    public GameObject timer;

    public GameObject carParents;

    [SerializeField] UISprite timerSprite;

    public List<GameObject> targetCars = new List<GameObject>();
    public float stageTime;
    public float time = 15f;

    int passCarCount = 0;


    protected override void Start()
    {
        base.Start();

        timer = FindObjectOfType<CarTimer>().gameObject;
        timer.GetComponent<UIWidget>().alpha = 1;


        timerSprite = timer.transform.GetChild(0).GetComponent<UISprite>();

        for (int i = 0; i < carParents.transform.childCount; i++)
        {
            targetCars.Add(carParents.transform.GetChild(i).gameObject);
        }
    }

    protected override void Update()
    {
        base.Update();

        if(!isGameOver)
        {
            if(targetCars.Count == 0)
                return;
            if (GameManager.Instance.IsPause == true)
                return;
            time -= Time.deltaTime;

            timerSprite.fillAmount = time / stageTime;

            if (time <= 0)
            {
                isGameOver = true;
                UIManager.Instance.GameOverTween();
            }
        }
    }

    public void CarPass()
    {
        targetCars.RemoveAt(0);

        time += 0.5f;
    }
}
    