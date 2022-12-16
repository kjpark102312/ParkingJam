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
    public GameObject limitTimePanel;

    public InGameUI gameUI;

    public TimeLimitCar limitCar;

    public int moveCount;
    public float time;

    void Start()
    {
        limitMoveUI = UIManager.Instance.GetUI(UIPanels.InGameUI).transform.Find("CarMoveCount").gameObject;
        limitTimePanel = GameObject.Find("CarLimitTime");
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
                limitCar.stageTime = time;
                limitCar.time = time;
                break;
        }
    }

    private void Update()
    {
        switch (mode)
        {
            case StageMode.limitMove:
                {
                    if (moveCount <= 0)
                    {
                        UIManager.Instance.GameOverTween();
                    }
                    break;
                }

            case StageMode.limitTime:
                
                break;
        }
        
    }

    public void UpdateMovecount()
    {
        moveCount--;
        gameUI.moveCount.text = $"{moveCount} moves";
    }
}
