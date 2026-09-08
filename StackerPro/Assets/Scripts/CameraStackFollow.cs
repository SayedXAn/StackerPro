using UnityEngine;
using System.Collections.Generic;

public class CameraStackFollow : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How much empty space to keep above the highest box")]
    public float lookAheadOffset = 3f;

    [Tooltip("How fast the camera moves (0.1 is slow/smooth, 1 is instant)")]
    public float smoothSpeed = 0.1f;

    [Tooltip("The default height if there are no boxes")]
    public float baseHeight = 0f;

    // A list to keep track of all boxes currently in the scene
    private List<GameObject> stackedBoxes = new List<GameObject>();

    // Call this method when you spawn a new box
    public void RegisterBox(GameObject box)
    {
        if (!stackedBoxes.Contains(box))
        {
            stackedBoxes.Add(box);
        }
    }

    void LateUpdate()
    {
        if(stackedBoxes.Count == 0) return;
        float targetY = baseHeight;

        // 1. Find the highest point in the stack
        if (stackedBoxes.Count > 0)
        {
            float highestPoint = float.MinValue;

            foreach (GameObject box in stackedBoxes)
            {
                if (box == null) continue; // Skip if box was destroyed

                // Get the collider to calculate the exact top edge
                Collider2D col = box.GetComponent<Collider2D>();
                if (col != null)
                {
                    // Calculate the top edge of the box in world space
                    float boxHeight = col.bounds.size.y;
                    float boxTop = box.transform.position.y + (boxHeight / 2f);

                    if (boxTop > highestPoint)
                    {
                        highestPoint = boxTop;
                    }
                }
            }

            // Set the target to the highest point found
            targetY = highestPoint;
        }

        // 2. Apply the offset (so we look slightly above the stack)
        targetY += lookAheadOffset;

        // 3. Smoothly move the camera
        // We only modify the Y position, keeping X and Z as they are
        if(targetY > transform.position.y)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }

        if ((targetY - lookAheadOffset) < transform.position.y)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, Mathf.Clamp(targetY - lookAheadOffset, 1f, targetY - lookAheadOffset), transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
        Debug.Log("Camera Position: " + transform.position.y);
        Debug.Log("Target Y: " + targetY);
    }

    // Optional: Clean up list if boxes are destroyed
    // (You can call this if you have a mechanic that removes boxes)
    public void CleanupList()
    {
        stackedBoxes.RemoveAll(box => box == null);
    }
}