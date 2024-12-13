using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public RectTransform compassBarTransform;

    public RectTransform northMarkerTransform;
    public RectTransform eastMarkerTransform;
    public RectTransform westMarkerTransform;
    public RectTransform southMarkerTransform;

    public Camera playerCamera;
    public Transform cameraObjectTransform;
    public Transform northObjectTransform;
    public Transform eastObjectTransform;
    public Transform westObjectTransform;
    public Transform southObjectTransform;

    // Update is called once per frame
    void Update()
    {
        SetMarkerPosition(northMarkerTransform, northObjectTransform.position);   
        SetMarkerPosition(westMarkerTransform, westObjectTransform.position);   
        SetMarkerPosition(eastMarkerTransform, eastObjectTransform.position);   
        SetMarkerPosition(southMarkerTransform, southObjectTransform.position);   
    }

    void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(2 * angle / playerCamera.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width / 2 * compassPositionX, 0);


        // Hide Markers too far
        float distance = Mathf.Clamp(2 * angle / playerCamera.fieldOfView, -8, 8);

        if (distance < -5 || distance > 5)
        {
            markerTransform.gameObject.SetActive(false);
        }
        else
        {
            markerTransform.gameObject.SetActive(true);
        }
    }
}
