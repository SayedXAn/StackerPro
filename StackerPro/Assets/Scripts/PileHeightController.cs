using UnityEngine;

public class PileHeightController : MonoBehaviour
{
    [SerializeField] private Transform pile;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform chain;

    [SerializeField] private float triggerDistance = 3f;
    [SerializeField] private float moveSpeed = 3f;

    private float initialCameraY;
    private float initialChainY;

    private void Start()
    {
        initialCameraY = cameraTransform.position.y;
        initialChainY = chain.position.y;
    }

    private void Update()
    {
        float peakY = GetPilePeak();
        Debug.Log("Pile Peak Y: " + peakY);
        //float cameraY = cameraTransform.position.y;

        //// If pile gets too close to the chain/camera area
        //float targetY = peakY + triggerDistance;

        //if (targetY > cameraY)
        //{
        //    Vector3 cameraPosition = cameraTransform.position;

        //    cameraPosition.y = Mathf.Lerp(
        //        cameraPosition.y,
        //        targetY,
        //        moveSpeed * Time.deltaTime
        //    );

        //    cameraTransform.position = cameraPosition;

        //    // Move chain together with camera
        //    Vector3 chainPosition = chain.position;
        //    chainPosition.y = cameraPosition.y;

        //    chain.position = chainPosition;
        //}
    }

    private float GetPilePeak()
    {
        float highestY = float.MinValue;

        foreach (Transform box in pile)
        {
            Collider2D collider = box.GetComponent<Collider2D>();

            if (collider != null && box.GetComponent<Box>().GetDrop())
            {
                highestY = Mathf.Max(
                    highestY,
                    collider.bounds.max.y
                );
            }
        }

        return highestY;
    }
}