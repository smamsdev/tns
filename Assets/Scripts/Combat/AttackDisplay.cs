using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackDisplay : MonoBehaviour
{
    public TextMeshProUGUI allyAttackDamageTextMeshProUI;
    public GameObject attackDisplayTextGO;
    [SerializeField] Animator attackDisplayAnimator; 

    public void ShowAttackDisplay(Combatant combatant, bool on)
    {
        if (combatant.attackTotal > 0)
        {
            if (on)
            {
                allyAttackDamageTextMeshProUI.text = combatant.attackTotal.ToString();
                attackDisplayTextGO.SetActive(on);
                attackDisplayAnimator.Play("CombatantAttackDamageFadeUp");
            }

            else
            {
                attackDisplayAnimator.Play("CombatantAttackDamageFadeUpReverse");
            }
        }

        else
        {
            attackDisplayAnimator.Play("CombatantAttackDamageHidden");
        }
    }

    public void SetAttackDisplayDirBasedOnLookDir(Combatant combatant)
    {
        var pos = combatant.combatantUI.attackDisplay.transform.localPosition;

        if (combatant.movementScript.lookDirection == Vector2.left)
        {
            pos.x = -Mathf.Abs(pos.x);
        }
        else
        {
            pos.x = Mathf.Abs(pos.x);
        }

        combatant.combatantUI.attackDisplay.transform.localPosition = pos;
    }
}
