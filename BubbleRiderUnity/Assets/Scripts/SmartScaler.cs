using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SmartScaler : MonoBehaviour
{

    Camera targetCam;

    CanvasScaler scaler;

    float aspect;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        targetCam = canvas.worldCamera;

        scaler = GetComponent<CanvasScaler>();

        aspect = scaler.referenceResolution.x / scaler.referenceResolution.y;
    }

    // Update is called once per frame
    void Update()
    {
        scaler.matchWidthOrHeight = targetCam.aspect < aspect ? 0 : 1;
    }
}
