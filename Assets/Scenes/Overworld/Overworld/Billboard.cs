using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
    public Camera cam;
    public Transform planet; // assign your planet center here
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main;

        Vector3 up = (transform.position - planet.position).normalized;

        // 1️⃣ Rotate sprite toward camera horizontally
        Vector3 camForward = cam.transform.forward;
        camForward = Vector3.ProjectOnPlane(camForward, up).normalized;
        transform.forward = camForward;

        // 2️⃣ Sorting based on distance to camera
        float dist = Vector3.Distance(cam.transform.position, transform.position);
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-dist * 1000f);
    }
}
