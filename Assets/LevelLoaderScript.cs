using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{

    [SerializeField] Collider2D firstEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("change level");

        if(other.tag == "Player")

        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
                                
        }


    }


}
