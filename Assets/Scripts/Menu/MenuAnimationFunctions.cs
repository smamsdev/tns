using UnityEngine;

public class MenuAnimationFunctions : MonoBehaviour
{
    public GameObject menuGameObject;

    public void DisableGO()
    {
        menuGameObject.SetActive(false);
    }
}
