using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadButtons : MonoBehaviour
{

    [SerializeField] EnemyUploadList enemyUploadList;
    [SerializeField] UnityEvent destroyEnemy;

    [SerializeField] GameObject playerObject;

    public void LoadBattle()

    {
        //  SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void LoadEnemyEasy()

    {
        enemyUploadList.EnemyEasy(); 
        SetupBattle();
    }

    public void LoadEnemyMid()

    {
        enemyUploadList.EnemyMid();
        SetupBattle();
    }

    public void LoadEnemyHard()

    {
        enemyUploadList.EnemyHard();
        SetupBattle();
    }

    public void FireDestroyEnemyEvent()
    {
        destroyEnemy.Invoke();
    }

    public void SetupBattle()

    {       
        CombatEvents.BeginBattle.Invoke();
        CombatEvents.UpdatePlayerPosition.Invoke(new Vector2(-1.6f, -0.5f), 1);
    
    }

}

            