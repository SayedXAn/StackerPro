using UnityEngine;
using UnityEngine.InputSystem;

public class CraneController : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform boxSpawnPoint;

    private GameObject currentBox;
    private HingeJoint2D currentJoint;

    private bool canDrop = true;
    public float YOffset = 1;
    public float spawnForce = 1;
    private void Start()
    {
        SpawnBox();
    }

    private void Update()
    {
        if (canDrop && Mouse.current.leftButton.wasPressedThisFrame)
        {
            DropBox();
        }
    }

    private void SpawnBox()
    {
        Vector3 spawnPosition = new Vector3(boxSpawnPoint.position.x, boxSpawnPoint.position.y- YOffset, boxSpawnPoint.position.z);
        // Create a new box
        currentBox = Instantiate(
            boxPrefab,
            spawnPosition,
            boxSpawnPoint.rotation
        );

        // Get the joint on the new box
        currentJoint = currentBox.GetComponent<HingeJoint2D>();
        currentJoint.connectedBody = boxSpawnPoint.GetComponent<Rigidbody2D>();

        canDrop = true;
        //currentBox.GetComponent<Rigidbody2D>().AddForceX(spawnForce, ForceMode2D.Impulse);
    }

    private void DropBox()
    {
        canDrop = false;

        // Release the box
        if (currentJoint != null)
        {
            currentJoint.enabled = false;
        }

        // Let physics control it
        Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Wait for this box to finish before spawning another
        StartCoroutine(SpawnNextBox());
    }

    private System.Collections.IEnumerator SpawnNextBox()
    {
        // Temporary delay
        yield return new WaitForSeconds(2f);

        SpawnBox();
    }
}