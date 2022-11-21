using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanel : MonoBehaviour
{
    public UIButton pauseButton;
    public TweenPosition rightPaneltp;



    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.Add(new EventDelegate(() => {

            rightPaneltp.enabled = true;
            rightPaneltp.PlayForward();
            UIManager.Instance.GetUI(UIPanels.BackGround).SetActive(true);
            UIManager.Instance.GetUI(UIPanels.BackGround).GetComponent<TweenPosition>().PlayForward();

            UIManager.Instance.GetUI(UIPanels.LeftUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.RightUI).SetActive(false);
            UIManager.Instance.GetUI(UIPanels.LvUI).SetActive(false);
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
