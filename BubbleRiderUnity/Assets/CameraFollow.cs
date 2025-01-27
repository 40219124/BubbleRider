using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform Target;
    Vector3 CamOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamOffset = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Target.position + CamOffset;
    }
}
