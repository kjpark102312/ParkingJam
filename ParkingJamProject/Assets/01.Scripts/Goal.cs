using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] Transform carParents;

    int goalCount = 0;

    // 자동차가 Goal 지점 오면 체크한번 해주기
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            goalCount++;

            if (goalCount == carParents.childCount)
            {
                LoadSceneManager.Instance.NextScene();
            }
        }
    }

}
