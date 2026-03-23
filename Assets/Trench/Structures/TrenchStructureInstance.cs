using TMPro;
using UnityEngine;

public class TrenchStructureInstance : MonoBehaviour
{
    public TrenchStructureSO structureSO;
    public SpriteRenderer spriteRenderer;
    public MenuButtonHighlighted menuButtonHighlighted;
    public Animator animator;
    public TextMeshProUGUI textMeshProUGUI;
    public TrenchManager.Team team;
    public string targetTag;
    public int currentHP;

    public void SpawnUnit()
    {
        Vector3 spawnPos = transform.position + new Vector3(0f, -0.01f, 0f);
        GameObject newPawnGO = Instantiate(structureSO.SpawnPrefab, spawnPos, Quaternion.identity, transform);

        newPawnGO.name = structureSO.SpawnPrefab.name;
        Pawn pawn = newPawnGO.GetComponent<Pawn>();
        pawn.team = team;

        if (team == TrenchManager.Team.Left)
            pawn.defaultAdvanceVector = Vector2.right;

        else
        {
            pawn.defaultAdvanceVector = Vector2.left;
            newPawnGO.transform.Rotate(0f, 180f, 0f);
        }

        pawn.EnterState(pawn.advancing);
    }
}
