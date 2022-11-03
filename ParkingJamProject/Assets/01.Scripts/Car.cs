using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    float sightAngle = 90f;

    Rigidbody rb;

    [SerializeField] bool isMove = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        float dot = Vector3.Dot(dir.normalized , -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if(theta <= sightAngle)
        {
            Debug.Log("¾Õ");
            if(!isMove)
                StartCoroutine(MoveCo(- transform.right));
        }
        else
        {
            Debug.Log("µÚ");
            if (!isMove)
                StartCoroutine(MoveCo(transform.right));
        }
    }

    IEnumerator MoveCo(Vector3 dir)
    {
        isMove = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation; 
        while (true)
        {
            rb.velocity = dir * 5;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car"))
        {
            StopAllCoroutines();
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            isMove = false;
        }
    }


}
