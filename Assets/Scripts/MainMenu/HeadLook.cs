using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HeadLook : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private float minYValue, maxYValue, minXValue, maxXValue;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = mainCamera.nearClipPlane + 1;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        worldPosition.x = Mathf.Clamp(worldPosition.x, minXValue, maxXValue);
        worldPosition.y = Mathf.Clamp(worldPosition.y, minYValue, maxYValue);
        transform.position = worldPosition;
    }
}
