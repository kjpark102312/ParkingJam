using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class People : MonoBehaviour
{
    public Transform[] points;

    public int index = 0;

    NavMeshAgent agent;

    UIManager uIManager;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        uIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        Patrol(index);
    }

    void Patrol(int idx)
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            if (points.Length == 0)
                return;
            agent.destination = points[index].position;
            index = (index + 1) % points.Length;
        }

        Vector3 dir = points[idx].localPosition - transform.localPosition;

        transform.LookAt(dir);

        agent.destination = points[idx].position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car"))
        {
            Debug.Log("ASD");
            uIManager.GameOverTween();
        }
    }

}
