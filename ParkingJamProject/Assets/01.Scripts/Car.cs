using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    float sightAngle = 90f;

    Rigidbody rb;

    public bool isMove = false;


    float speed = 15f;

    public Transform passPos;

    NavMeshAgent agent;

    Vector3 curMoveDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {


        if (!isMove)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void Move(Vector3 dir)
    {
        float dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if (theta <= sightAngle)
        {
            Debug.Log("앞");
            if (!isMove)
            {
                StartCoroutine(MoveCo(-transform.right));

                curMoveDir = -transform.right;
            }

        }
        else
        {
            Debug.Log("뒤");
            if (!isMove)
            {
                StartCoroutine(MoveCo(transform.right));

                curMoveDir = transform.right;
            }


        }
    }

    public void PassCheck(Vector3 dir)
    {
        RaycastHit hit;


        Debug.DrawRay(transform.position, dir, Color.red, 20f);
        if (Physics.Raycast(transform.position, dir, out hit, 100f, 1 << 6))
        {
            Debug.Log(hit);

            return;
        }

        Pass();
    }


    IEnumerator MoveCo(Vector3 dir)
    {
        isMove = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        PassCheck(curMoveDir);

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
    
            
            
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            
        }
    }

    void ColKnockBack()
    {
        if(isMove)
        {
            Debug.Log("넉백");

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(-curMoveDir * 3f, ForceMode.Impulse);

            Debug.Log(-curMoveDir * 3f);
            isMove = false;
        }
    }

    public void Pass()
    {
        //  패스
        StopAllCoroutines();
        rb.velocity = Vector3.zero;
        agent.destination = passPos.position;

       
        Debug.Log("패스");
    }
}

