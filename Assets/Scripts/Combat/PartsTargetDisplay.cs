using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PartsTargetDisplay : MonoBehaviour
{
    public Enemy enemy;

    [SerializeField] GameObject bodyTargetDisplay;
    [SerializeField] GameObject armsTargetDisplay;
    [SerializeField] GameObject headTargetDisplay;

    [SerializeField] TextMeshPro bodyDescriptionTextMeshPro, armsDescriptionTextMeshPro, headDescriptionTextMeshPro;

    [SerializeField] Animator bodyAnimator, armsAnimator, headAnimator;

    public int bodyMaxHP, armsMaxHP, headMaxHP;

    public string  bodyHeader, armsHeader, headHeader, defaultBodyDescription, defaultArmsDescription, defaultHeadDescription, injuredBodyDescription, injuredArmsDescription, injuredHeadDescription;

    private void OnEnable()
    {
        bodyTargetDisplay.SetActive(false);
        armsTargetDisplay.SetActive(false);
        headTargetDisplay.SetActive(false);
    }

    public void UpdateTargetDisplay(bool showBody, bool showArms, bool showHead) 
    {

        string bodyHP = enemy.enemyBodyHP.ToString() + " / " + bodyMaxHP.ToString();
        string armsHP = enemy.enemyArmsHP.ToString() + " / " + armsMaxHP.ToString();
        string headHP = enemy.enemyHeadHP.ToString() + " / " + headMaxHP.ToString();

        bodyTargetDisplay.SetActive(showBody);
        armsTargetDisplay.SetActive(showArms);
        headTargetDisplay.SetActive(showHead);

        bodyDescriptionTextMeshPro.text = bodyHeader + "<br>" + bodyHP + "<br>" + defaultBodyDescription;
        armsDescriptionTextMeshPro.text = armsHeader + "<br>" + armsHP + "<br>" + defaultArmsDescription;
        headDescriptionTextMeshPro.text = headHeader + "<br>" + headHP + "<br>" + defaultHeadDescription;

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

    public void FlipTargetDisplay()
    {
        var flippedPos = bodyDescriptionTextMeshPro.rectTransform.localPosition;
        bodyDescriptionTextMeshPro.rectTransform.pivot = new Vector2(1f, 0.5f);
        flippedPos.x = -flippedPos.x;
        bodyDescriptionTextMeshPro.rectTransform.localPosition = flippedPos;

        flippedPos = armsDescriptionTextMeshPro.rectTransform.localPosition;
        armsDescriptionTextMeshPro.rectTransform.pivot = new Vector2(1f, 0.5f);
        flippedPos.x = -flippedPos.x;
        armsDescriptionTextMeshPro.rectTransform.localPosition = flippedPos;

        flippedPos = headDescriptionTextMeshPro.rectTransform.localPosition;
        headDescriptionTextMeshPro.rectTransform.pivot = new Vector2(1f, 0.5f);
        flippedPos.x = -flippedPos.x;
        headDescriptionTextMeshPro.rectTransform.localPosition = flippedPos;
    }

}
