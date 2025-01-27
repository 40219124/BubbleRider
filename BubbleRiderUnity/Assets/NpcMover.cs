using UnityEngine;

public class NpcMover : MonoBehaviour
{
    [SerializeField]
    Transform StartPoint;
    [SerializeField]
    Transform EndPoint;

    [SerializeField]
    float CycleTime = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, (Mathf.Sin(CycleTime * Time.time / (Mathf.PI * 2f)) + 1f) / 2f);
    }
}
