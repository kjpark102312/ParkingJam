using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] Transform carParents;

    int goalCount = 0;

    // �ڵ����� Goal ���� ���� üũ�ѹ� ���ֱ�
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
