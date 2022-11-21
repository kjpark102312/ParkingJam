using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEmoji : MonoBehaviour
{
    public Emoji[] emoji;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Car"))
        {
            if(!GetComponent<Car>().isMove)
            {
                int index = Random.Range(0, emoji.Length);

                Vector3 screenPos = UICamera.mainCamera.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(transform.GetChild(0).position));

                screenPos = new Vector3(screenPos.x, screenPos.y, 0);

                emoji[index].Play(screenPos);
            }
        }
    }
}
