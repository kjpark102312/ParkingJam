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
        Patrol();
    }

    void Patrol()
    {
        agent.destination = points[index].position;

        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            if (points.Length == 0)
                return;
            agent.destination = points[index].position;

            index = index + 1;
            if(index >= points.Length)
            {
                index = 0;
            }
        }

        Vector3 dir = points[index].localPosition - transform.localPosition;

        transform.LookAt(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car"))
        {
            uIManager.GameOverTween();
        }
    }

}
