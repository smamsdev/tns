using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : ToTrigger
{
    public List<Combatant> allies;
    public List<Combatant> enemies;
    public Transform battleCenterPosition;
    public GameObject playerFightingPosition;
    public CombatManager combatManager;
    PlayerCombat playerCombat;
    public bool isEnemyFlanked =false;
    public bool isAllyFlanked = false;
    public bool isRandomEnounter;
    public bool isSpawningPartyMembers = false;

    public override IEnumerator TriggerFunction()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        playerCombat = combatManager.playerCombat;

        if (isSpawningPartyMembers)
        {
            AddPartyMembers();
            SetPartyMemberPositions();
        }

        combatManager.battleScheme = this;
        combatManager.StartBattle();
        yield return null;
    }

    void AddPartyMembers()
    {
        for (int i = 0; i < playerCombat.partySO.partyMembers.Count; i++)
        {
            GameObject allyToAddGO = Instantiate(playerCombat.partySO.partyMembers[i].prefab, this.transform);
            Combatant allyCombatant = allyToAddGO.GetComponent<Combatant>();
            allies.Add(allyCombatant);
            allyCombatant.collisionCollider.enabled = false;
        }
    }

    void SetPartyMemberPositions()
    {
        Vector2 playerPos = playerCombat.transform.position;

        float horizontalSpacing = 0.18f;
        float verticalSpacing = 0.16f;

        for (int i = 0; i < allies.Count; i++)
        {
            // each ally goes further back
            float xOffset = horizontalSpacing * (i + 1);

            // alternate up/down
            int verticalDir = (i % 2 == 0) ? 1 : -1;

            // gradually widen vertical spread
            float yOffset = verticalSpacing * ((i / 2) + 1) * verticalDir;

            Vector2 fightingPos = new Vector2(playerPos.x - xOffset,playerPos.y + yOffset);

            allies[i].transform.position = playerCombat.transform.position;

            allies[i].CombatLookDirX = playerCombat.CombatLookDirX;

            allies[i].fightingPosition = new GameObject();
            allies[i].fightingPosition.transform.position = fightingPos;
        }
    }
}

