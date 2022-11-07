using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 lastPos;

    GameObject hitObj;

    bool isCanTouchCar = true;

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
                if (!isCanTouchCar)
                    return;

                if (hit.collider.CompareTag("Car"))
                {
                    firstPos = hit.point    ;
                    hitObj = hit.collider.gameObject;

                    //Debug.DrawRay(firstPos, -hitObj.transform.right, Color.red, 20f);
                }
            }
        }
        if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane ))
            {
                if (!isCanTouchCar)
                    return;

                lastPos = hit.point;

                if ((lastPos - firstPos).sqrMagnitude > 0.5f && !hitObj.GetComponent<Car>().isMove)
                {
                    isCanTouchCar = false;
                    hitObj.GetComponent<Car>().Move(lastPos - firstPos);
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isCanTouchCar = true;
        }

    }
}
