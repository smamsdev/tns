using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TargetDisplay : MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] CombatManager combatManager; 

    [SerializeField] GameObject bodyTargetDisplay;
    [SerializeField] GameObject armsTargetDisplay;
    [SerializeField] GameObject headTargetDisplay;

    [SerializeField] TextMeshPro bodyDescriptionTextMeshPro, armsDescriptionTextMeshPro, headDescriptionTextMeshPro;

    [SerializeField] Animator bodyAnimator, armsAnimator, headAnimator;

    public int bodyMaxHP, armsMaxHP, headMaxHP;

    public string  bodyHeader, armsHeader, headHeader, defaultBodyDescription, defaultArmsDescription, defaultHeadDescription, injuredBodyDescription, injuredArmsDescription, injuredHeadDescription;

    private void OnEnable()
    {
        combatManager = GetComponentInParent<CombatManager>();
    }

    public void UpdateTargetDisplay(bool showBody, bool showArms, bool showHead) 
    {

        string bodyHP = enemy.enemyBodyHP.ToString() + " / " + bodyMaxHP.ToString();
        string armsHP = enemy.enemyArmsHP.ToString() + " / " + armsMaxHP.ToString();
        string headHP = enemy.enemyHeadHP.ToString() + " / " + headMaxHP.ToString();

        bodyTargetDisplay.SetActive(showBody);
        armsTargetDisplay.SetActive(showArms);
        headTargetDisplay.SetActive(showHead);

        bodyDescriptionTextMeshPro.text = bodyHeader+ "<br>" + defaultBodyDescription + "<br>" + bodyHP;
        armsDescriptionTextMeshPro.text = armsHeader + "<br>" + defaultArmsDescription + "<br>" + armsHP;
        headDescriptionTextMeshPro.text = headHeader + "<br>" + defaultHeadDescription + "<br>" + headHP;

        if (enemy.enemyBodyHP == 0)
        {
            UpdateBodyDescription(injuredBodyDescription);
            bodyAnimator.SetBool("isDestroyed", true);
        }

        if (enemy.enemyArmsHP == 0)
        {
            UpdateArmsDescription(injuredArmsDescription);
            armsAnimator.SetBool("isDestroyed", true);
        }

        if (enemy.enemyHeadHP == 0)
        {
            UpdateHeadDescription(injuredHeadDescription);
            headAnimator.SetBool("isDestroyed", true);
        }
    }

    public void InitializeEnemyPartsHP()

    {
        bodyMaxHP = enemy.enemyBodyHP;
        armsMaxHP = enemy.enemyArmsHP;
        headMaxHP = enemy.enemyHeadHP;
    }

    void UpdateBodyDescription(string description)
    { bodyDescriptionTextMeshPro.text = description; }

    void UpdateArmsDescription(string description)
    { armsDescriptionTextMeshPro.text = description; }

    void UpdateHeadDescription(string description)
    { headDescriptionTextMeshPro.text = description; }

}
