using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{

    [SerializeField] UILabel lvText;
    // Start is called before the first frame update
    void Start()
    {
        lvText.text = $"Level {StageManager.Instance.curStageIndex+1}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
