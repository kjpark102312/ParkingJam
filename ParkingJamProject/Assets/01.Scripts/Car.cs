using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    float sightAngle = 90f;

    Rigidbody rb;

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
            StartCoroutine(MoveCo(- transform.right));
        }
        else
        {
            Debug.Log("µÚ");
            StartCoroutine(MoveCo(transform.right));
        }
    }

    IEnumerator MoveCo(Vector3 dir)
    {
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
        }
    }


}
