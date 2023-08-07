using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadButtons : MonoBehaviour
{

    [SerializeField] EnemyUploadList enemyUploadList;
    [SerializeField] UnityEvent destroyEnemy;

    public void LoadBattle()

    {
      //  SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

  public void LoadEnemyEasy()

    {
        enemyUploadList.EnemyEasy();
    }

    public void LoadEnemyMid()

    {
        enemyUploadList.EnemyMid();
    }

    public void LoadEnemyHard()

    {
        enemyUploadList.EnemyHard();
    }

    public void FireDestroyEnemyEvent()
    {
        destroyEnemy.Invoke();
    }

}

            