using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class CraneController : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform boxSpawnPoint;

    private GameObject currentBox;
    private HingeJoint2D currentJoint;

    private bool canDrop = true;
    public float YOffset = 1;
    //public float spawnForce = 1;
    public GameObject spawnParent;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
        SpawnBox();
    }

    private void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if(canDrop && touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                DropBox();
                ScoreManager.Instance.AddScore();
                UIManager.Instance.UpdateScoreText();
            }
        }

        if (canDrop && (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
        {
            DropBox();
            ScoreManager.Instance.AddScore();
            UIManager.Instance.UpdateScoreText();
        }
    }

    private void SpawnBox()
    {
        Vector3 spawnPosition = new Vector3(boxSpawnPoint.position.x, boxSpawnPoint.position.y- YOffset, boxSpawnPoint.position.z);
        // Create a new box
        currentBox = Instantiate(
            boxPrefab,
            spawnPosition,
            boxSpawnPoint.rotation,
            spawnParent.transform
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

        currentBox.GetComponent<Box>().SetDrop(true);        

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