using System.Collections;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public Transform transformToFollow;
    public string sceneName;

    [SerializeField] Animator defaultFaderAnimator;
    public bool isCustomSceneStart;

    private void OnEnable()
    {
        FieldEvents.SceneChanging += FadeDown;
        FieldEvents.sceneName = sceneName;

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (transformToFollow == null)
        { 
            transformToFollow = playerGO.transform;
        }
    }

    private void OnDisable()
    {
        FieldEvents.SceneChanging -= FadeDown;
    }

    private void Start()
    {
        CombatEvents.LockPlayerMovement();
        Camera.main.transform.position = new Vector3(transformToFollow.position.x, transformToFollow.transform.position.y, transformToFollow.transform.position.z - 10);

        if (!isCustomSceneStart)
        {
            StartCoroutine(DefaultSceneStart());
        }

        else
        { 
            FieldEvents.StartScene();
        }
    }

    IEnumerator DefaultSceneStart()
    {
        if (defaultFaderAnimator == null)
        {
            defaultFaderAnimator = GameObject.FindGameObjectWithTag("DefaultFader").GetComponent<Animator>();
        }

        defaultFaderAnimator.SetBool("start", true);
        yield return new WaitForSeconds(0.5f);
        CombatEvents.UnlockPlayerMovement();

        StartScene();
    }

    public void FadeUp()
    {
        defaultFaderAnimator.SetBool("start", true);
    }

    public void StartScene()

    {
        FieldEvents.StartScene();
    }

    void FadeDown()
    {
        defaultFaderAnimator.SetTrigger("Trigger2");
    }
}