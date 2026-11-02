using UnityEngine;

public class NeighborDetector : MonoBehaviour
{
    [Header("Raycast L")]
    public Transform raycastL;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool raycast(Vector2 direction, Vector2 origin)
    {
        float rayLength = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength);

        if (hit.collider != null && hit.collider.tag != gameObject.tag)
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
