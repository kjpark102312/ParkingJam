using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{

    [SerializeField] UILabel lvText;
    public UILabel moveCount;

    Stage stage; 
    // Start is called before the first frame update
    void Start()
    {
        StageManager.Instance.curStageIndex = PlayerPrefs.GetInt("Stage");
        lvText.text = $"Level {StageManager.Instance.curStageIndex+1}";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
