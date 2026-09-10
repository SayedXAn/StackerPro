using UnityEngine;
using System.Collections.Generic;

public class CameraStackFollow : MonoBehaviour
{
    public GameObject chain;
    public GameObject LRPos;
    public float chainThresh = 1f;
    [Header("Settings")]
    [Tooltip("How much empty space to keep above the highest box")]
    public float lookAheadOffset = 3f;

    [Tooltip("How quickly the camera follows the stack")]
    public float smoothTime = 0.25f;

    [Tooltip("Minimum vertical change before the camera starts following")]
    public float movementThreshold = 0.1f;

    [Tooltip("The default height if there are no boxes")]
    public float baseHeight = 0f;

    [Tooltip("The lowest Y position the camera is allowed to reach")]
    public float minimumCameraY = 1f;

    private List<GameObject> stackedBoxes = new List<GameObject>();

    private float currentVelocity = 0f;
    private int counter = 0;

    public void RegisterBox(GameObject box)
    {
        if (box != null && !stackedBoxes.Contains(box))
        {
            stackedBoxes.Add(box);
            counter++;
            if(counter == 10)
            {
                CleanupList();
                counter = 0;
            }
        }
    }

    private void LateUpdate()
    {
        // Remove destroyed boxes
        stackedBoxes.RemoveAll(box => box == null);

        float highestPoint = float.MinValue;

        // Find the highest point of the entire pile
        foreach (GameObject box in stackedBoxes)
        {
            if (box == null)
                continue;

            Collider2D col = box.GetComponent<Collider2D>();

            if (col != null)
            {
                // Actual world-space top of the collider
                float boxTop = col.bounds.max.y;

                if (boxTop > highestPoint)
                {
                    highestPoint = boxTop;
                }
            }
        }

        // No boxes found
        if (highestPoint == float.MinValue)
        {
            MoveCamera(baseHeight);
            return;
        }

        // Camera target based on current pile peak
        float targetY = highestPoint + lookAheadOffset;

        // Never allow the camera target below the minimum height
        targetY = Mathf.Max(targetY, minimumCameraY);

        // Don't react to tiny physics fluctuations
        if (Mathf.Abs(targetY - transform.position.y) < movementThreshold)
        {
            return;
        }

        MoveCamera(targetY);
    }

    private void MoveCamera(float targetY)
    {
        float newY = Mathf.SmoothDamp(
            transform.position.y,
            targetY,
            ref currentVelocity,
            smoothTime
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
        chain.transform.position = new Vector3(
            chain.transform.position.x,
            newY + chainThresh,
            chain.transform.position.z
        );
        LRPos.transform.position = new Vector3(
            LRPos.transform.position.x,
            newY + chainThresh,
            LRPos.transform.position.z
        );
    }

    public void CleanupList()
    {
        stackedBoxes.RemoveAll(box => box == null);
    }
}