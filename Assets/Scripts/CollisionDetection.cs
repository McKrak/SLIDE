using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    bool isCollidingFlag = false;
    public bool isColliding() {
        return isCollidingFlag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Collider")
        {
            isCollidingFlag = true;
            Debug.Log(gameObject.name + " colliding with " + collision.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCollidingFlag = false;
    }
}
