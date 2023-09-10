using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TargetDisplay : MonoBehaviour
{

    [SerializeField] GameObject bodyTargetDisplay;
    [SerializeField] GameObject armsTargetDisplay;
    [SerializeField] GameObject headTargetDisplay;

    [SerializeField] Enemy enemy;

    [SerializeField] TextMeshPro bodyHPDisplayTextMeshPro;
    [SerializeField] TextMeshPro armsHPDisplayTextMeshPro;
    [SerializeField] TextMeshPro headHPDisplayTextMeshPro;

    [SerializeField] TextMeshPro bodyDescriptionTextMeshPro;
    [SerializeField] TextMeshPro armsDescriptionTextMeshPro;
    [SerializeField] TextMeshPro headDescriptionTextMeshPro;


    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator armsAnimator;
    [SerializeField] Animator headAnimator;

    public int bodyMaxHP;
    public int armsMaxHP;
    public int headMaxHP;

    int bodyCurrentHP;
    int armsCurrentHP;
    int headCurrentHP;

    private void OnEnable()
    {
        CombatEvents.HighlightBodypartTarget += UpdateTargetDisplay;
        CombatEvents.InitializePartsHP += InitializePartsHP;
        CombatEvents.UpdateTargetDisplayBodyDescription += UpdateBodyDescription;
        CombatEvents.UpdateTargetDisplayArmsDescription += UpdateArmsDescription;
        CombatEvents.UpdateTargetDisplayHeadDescription += UpdateHeadDescription;
    }

    private void OnDisable()
    {
        CombatEvents.HighlightBodypartTarget -= UpdateTargetDisplay;
        CombatEvents.InitializePartsHP -= InitializePartsHP;
        CombatEvents.UpdateTargetDisplayBodyDescription -= UpdateBodyDescription;
        CombatEvents.UpdateTargetDisplayArmsDescription -= UpdateArmsDescription;
        CombatEvents.UpdateTargetDisplayHeadDescription -= UpdateHeadDescription;
    }

    private void Start()
    {

    }


    void UpdateTargetDisplay(bool showBody, bool showArms, bool showHead) 
    {
        bodyTargetDisplay.SetActive(showBody);
        armsTargetDisplay.SetActive(showArms);
        headTargetDisplay.SetActive(showHead);

        bodyCurrentHP = enemy.enemyBodyHP;
        armsCurrentHP = enemy.enemyArmsHP;
        headCurrentHP = enemy.enemyHeadHP;


       bodyHPDisplayTextMeshPro.text = bodyCurrentHP.ToString() + " / " + bodyMaxHP.ToString();
       armsHPDisplayTextMeshPro.text = armsCurrentHP.ToString() + " / " + armsMaxHP.ToString();
       headHPDisplayTextMeshPro.text = headCurrentHP.ToString() + " / " + headMaxHP.ToString();

        if (bodyCurrentHP == 0)
        {
            CombatEvents.UpdateTargetDisplayBodyDescription.Invoke("Body Destroyed!<br>Enemy takes double damage");
            bodyAnimator.SetBool("isDestroyed", true);
        }

        if (armsCurrentHP == 0)
        {
            CombatEvents.UpdateTargetDisplayArmsDescription.Invoke("Arms Destroyed!<br>Enemy attacks are halved");
            armsAnimator.SetBool("isDestroyed", true);
        }

        if (headCurrentHP == 0)
        {
            CombatEvents.UpdateTargetDisplayHeadDescription.Invoke("Head Destroyed!<br>Enemy fumbles half their attacks");
            headAnimator.SetBool("isDestroyed", true);
        }

    }

    public void InitializePartsHP()

    {
        bodyMaxHP = enemy.enemyBodyHP;
        armsMaxHP = enemy.enemyArmsHP;
        headMaxHP = enemy.enemyHeadHP;

        bodyCurrentHP = bodyMaxHP;
        armsCurrentHP = armsMaxHP;
        headCurrentHP = headMaxHP;
    }

    void UpdateBodyDescription(string description)
    { bodyDescriptionTextMeshPro.text = description; }

    void UpdateArmsDescription(string description)
    { armsDescriptionTextMeshPro.text = description; }

    void UpdateHeadDescription(string description)
    { headDescriptionTextMeshPro.text = description; }



}
