using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    float sightAngle = 90f;

    Rigidbody rb;

    [SerializeField] bool isMove = false;

    float speed = 15f;

    public Transform passPos;

    NavMeshAgent agent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 dir)
    {
        float dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if (theta <= sightAngle)
        {
            Debug.Log("앞");
            if (!isMove)
                StartCoroutine(MoveCo(-transform.right));
        }
        else
        {
            Debug.Log("뒤");
            if (!isMove)
                StartCoroutine(MoveCo(transform.right));
        }
    }

    public void PassCheck(Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, 100f, 1 << 7))
        {
            return;
        }

        Pass();
    }


    IEnumerator MoveCo(Vector3 dir)
    {
        isMove = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;

        PassCheck(dir);
        while (true)
        {
            rb.velocity = dir * speed;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Wall"))
        {
            StopAllCoroutines();
            ColKnockBack();

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            isMove = false;
        }
    }

    void ColKnockBack()
    {
        rb.AddForce(-rb.velocity, ForceMode.Force);
    }

    public void Pass()
    {
        //  패스
        agent.SetDestination(passPos.position);
        Debug.Log("패스");
    }
}

