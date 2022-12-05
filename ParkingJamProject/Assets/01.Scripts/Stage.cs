using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public enum StageMode
    {
        limitMove,
        limitTime,
    }

    public StageMode mode = StageMode.limitMove;

    public GameObject limitMoveUI;
    public GameObject limitTimeUI;

    public InGameUI gameUI;

    public TimeLimitCar limitCar;

    public int moveCount;
    public float time;

    void Start()
    {
        limitMoveUI = GameObject.Find("CarMoveCount");
        limitTimeUI = GameObject.Find("CarTimer");
        gameUI = FindObjectOfType<InGameUI>();
        limitCar = FindObjectOfType<TimeLimitCar>();

        switch (mode)
        {
            case StageMode.limitMove:
                {
                    limitMoveUI.gameObject.SetActive(true);
                    gameUI.moveCount.gameObject.SetActive(true);
                    gameUI.moveCount.text = $"{moveCount} moves";
                    break;
                }
                
            case StageMode.limitTime:
                limitTimeUI.gameObject.SetActive(true);
                limitCar.stageTime = time;
                limitCar.time = time;
                break;
        }
    }

    public void UpdateMovecount()
    {
        moveCount--;
        gameUI.moveCount.text = $"{moveCount} moves";
    }
}
