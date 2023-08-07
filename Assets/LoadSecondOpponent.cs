using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSecondOpponent : MonoBehaviour
{
  

    [SerializeField] EnemyManagerSO enemyManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("change level");

        if (other.tag == "Player")

        {
            //SceneManager.LoadScene(0, LoadSceneMode.Single);
            enemyManager.SetMediumEnemy();

        }

    }
}  
