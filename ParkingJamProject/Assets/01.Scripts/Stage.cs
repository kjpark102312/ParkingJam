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

    public int moveCount;
    public float time;

    void Start()
    {
        limitMoveUI = GameObject.Find("CarMoveCount");
        limitTimeUI = GameObject.Find("CarTimer");

        switch (mode)
        {
            case StageMode.limitMove:
                limitMoveUI.gameObject.SetActive(true);
                break;
            case StageMode.limitTime:
                limitTimeUI.gameObject.SetActive(true);
                break;
        }
    }

    void Update()
    {
        
    }
}
