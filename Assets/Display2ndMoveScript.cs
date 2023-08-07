using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display2ndMoveScript : MonoBehaviour
{

    TextMeshProUGUI textMeshProUGUI;
    public string secondDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        secondDisplayText = "Second Move";
    }

    // Update is called once per frame
    public void UpdateSecondDisplayText(string secondDisplayText)
    {
        textMeshProUGUI.text = secondDisplayText;
    }
}
