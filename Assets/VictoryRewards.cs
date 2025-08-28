using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryRewards : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public GridLayoutGroup gridLayout;

    void Start()
    {
        float preferredWidth = tmp.preferredWidth;
        Vector2 newCellSize = gridLayout.cellSize;
        newCellSize.x = preferredWidth;
        gridLayout.cellSize = newCellSize;
    }
}
