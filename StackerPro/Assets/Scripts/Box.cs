using UnityEngine;

public class Box : MonoBehaviour
{
    bool isDropped = false;
    public CameraStackFollow cameraController;
    private void Start()
    {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStackFollow>();
    }
    public void SetDrop(bool x)
    {
        isDropped = x;
    }
    public bool GetDrop()
    {
        return isDropped;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDropped && collision.gameObject.CompareTag("box"))
        {
            if (cameraController != null)
            {
                cameraController.RegisterBox(gameObject);
            }
        }
    }
}
