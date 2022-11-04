using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 lastPos;

    GameObject hitObj;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, 1 << 6))
            {
                if (hit.collider == null)
                    return;
                Debug.Log("클릭");

                if (hit.collider.CompareTag("Car"))
                {
                    firstPos = hit.point    ;
                    hitObj = hit.collider.gameObject;

                    Debug.DrawRay(firstPos, -hitObj.transform.right, Color.red, 20f);

                    Debug.Log("맞음");
                }
            }
        }
        if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane ))
            {
                lastPos = hit.point;

                Debug.DrawLine(hitObj.transform.position, lastPos, Color.red);

                if ((lastPos - firstPos).sqrMagnitude > 0.5f)
                {
                    hitObj.GetComponent<Car>().Move(lastPos - firstPos);
                    return;
                }
            }
        }

    }
}
