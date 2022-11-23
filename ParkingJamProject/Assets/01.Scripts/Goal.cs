using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] Transform carParents;

    int goalCount = 0;

    [SerializeField] GameObject[] particles;

    NextStageUI nextStageUI;

    // 자동차가 Goal 지점 오면 체크한번 해주기

    private void Start()
    {
        nextStageUI = FindObjectOfType<NextStageUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            goalCount++;
            other.gameObject.GetComponent<Car>().isPassing = false;
            other.gameObject.SetActive(false);

            if (goalCount == carParents.childCount)
            {
                Invoke("GoalEffect", 1f);
                nextStageUI.OnNextStageUI();
                //LoadSceneManager.Instance.NextScene();
            }
        }
    }

    public void GoalEffect()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].SetActive(true);

        }
    }

}
