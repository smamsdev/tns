using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Display1stMoveScript : MonoBehaviour
{

    TextMeshProUGUI vegetextMeshProUGUI;
    public string firstDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        vegetextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        firstDisplayText = "First Move";    
    }

    // Update is called once per frame
    public void UpdateFirstDisplayText(string firstDisplayText)
    {
        vegetextMeshProUGUI.text = firstDisplayText;
    }
}
