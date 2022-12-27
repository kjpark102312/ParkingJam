using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _firstPos;
    private Vector3 _lastPos;

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
                    return;
                }
                if (!isCanTouchCar)
                    return;


                if (hit.collider.CompareTag("Car"))
                {
                    _firstPos = hit.point;
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

                _lastPos = hit.point;

                if ((_lastPos - _firstPos).sqrMagnitude > 0.5f && !hitObj.GetComponent<Car>().isMove)
                {
                    isCanTouchCar = false;
                    hitObj.GetComponent<Car>().Move(_lastPos - _firstPos);
                }

                
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isCanTouchCar = true;
        }



    }

}
