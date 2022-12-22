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
        if (GameManager.Instance.IsPause)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, 1 << 6))
            {
                if (hit.collider == null)
                {
                    Debug.Log("QQQQ");
                    return;
                }
                if (!isCanTouchCar)
                    return;

                Debug.Log(hit.transform.gameObject);

                if (hit.collider.CompareTag("Car"))
                {
                    Debug.Log("QQQQ");
                    firstPos = hit.point    ;
                    hitObj = hit.collider.gameObject;
                }
            }
        }
        if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, 1<< 6))
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
