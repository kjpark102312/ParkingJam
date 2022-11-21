using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{

    public UIButton returnButton;


    TweenPosition tp;
    // Start is called before the first frame update
    void Start()
    {

        tp = GetComponent<TweenPosition>();

        returnButton.onClick.Add(new EventDelegate(() =>
        {
            UIManager.Instance.GetUI(UIPanels.PauseUI).SetActive(true);

            tp.enabled = true;
            tp.PlayReverse();


            Debug.Log("Asd");   
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
