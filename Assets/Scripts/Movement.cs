using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    [Header("Speed")]
    public float speed = 0.2f;

    [Header("Controlled By Player")]
    public bool isControlledByPlayer = false;

    [Header("Slides On Ice")]
    public bool slidesOnIce = true;

    [Header("Moves On Conveyer")]
    public bool movesOnConveyer = true;


    [Header("Ground Tilemap")]
    public Tilemap groundTilemap;
    [Header("Wall Tilemap")]
    public Tilemap wallTilemap;
    [Header("Slide Tilemap")]
    public Tilemap slideTilemap;
    [Header("Conveyer Up Tilemap")]
    public Tilemap conveyerU;
    [Header("Conveyer Left Tilemap")]
    public Tilemap conveyerL;
    [Header("Conveyer Right Tilemap")]
    public Tilemap conveyerR;
    [Header("Conveyer Down Tilemap")]
    public Tilemap conveyerD;
    //[Header("Collider Left")]
    //public GameObject colliderL;
    //[Header("Collider Right")]
    //public GameObject colliderR;
    //[Header("Collider Up")]
    //public GameObject colliderU;
    //[Header("Collider Down")]
    //public GameObject colliderD;

    private bool neighborL = false;
    private bool neighborR = false;
    private bool neighborU = false;
    private bool neighborD = false;

    private Vector3 currentPos, targetPos;
    private bool isMoving;
    private Vector3 lastDirection;

    private bool inTeleporter = false;
    private GameObject teleporterDest = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //updateColliders();  
        // get current grid coordinates
        // check to see if objects are on each side of the player using NeighborDetector
        if (isControlledByPlayer)
        {
            neighborL = GetComponent<NeighborDetector>().raycast(Vector2.left);
            neighborR = GetComponent<NeighborDetector>().raycast(Vector2.right);
            neighborU = GetComponent<NeighborDetector>().raycast(Vector2.up);
            neighborD = GetComponent<NeighborDetector>().raycast(Vector2.down);
        }
        Vector3Int currentGridPos = groundTilemap.WorldToCell(transform.position);

        if (!isMoving)
        {
            if (slideTilemap.HasTile(currentGridPos) && canMove(lastDirection) && slidesOnIce) // if current tile is a slide tile,
            {
                // force player to continue moving in same direction
                StartCoroutine(MovePlayerLerp(lastDirection));
                //Debug.Log("Sliding!");
            }
            else if (conveyerU.HasTile(currentGridPos) && canMove(Vector3.up) && movesOnConveyer)
            {
                StartCoroutine(MovePlayerLerp(Vector3.up));
            }
            else if (conveyerL.HasTile(currentGridPos) && canMove(Vector3.left) && movesOnConveyer) // same logic for other directions
            {
                StartCoroutine(MovePlayerLerp(Vector3.left));
            }
            else if (conveyerR.HasTile(currentGridPos) && canMove(Vector3.right) && movesOnConveyer)
            {
                StartCoroutine(MovePlayerLerp(Vector3.right));
            }
            else if (conveyerD.HasTile(currentGridPos) && canMove(Vector3.down) && movesOnConveyer)
            {
                StartCoroutine(MovePlayerLerp(Vector3.down));
            }

            else if (isControlledByPlayer)
            {
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                {
                    StartCoroutine(MovePlayerLerp(Vector3.up));
                }
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                {
                    StartCoroutine(MovePlayerLerp(Vector3.left));
                }
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                {
                    StartCoroutine(MovePlayerLerp(Vector3.down));
                }
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    StartCoroutine(MovePlayerLerp(Vector3.right));
                }
            }
        }

    }

    // used to update the colliders for the player with other objects.
    //void updateColliders()
    //{
    //    isCollidingL = colliderL.GetComponent<CollisionDetection>().isColliding();
    //    if(isCollidingL)
    //    {
    //        Debug.Log("Colliding Left!");
    //    }
    //    //isCollidingR = colliderR.GetComponent<CollisionDetection>().isColliding();
    //    //if(isCollidingR)
    //    //{
    //    //    print("Colliding Right");
    //    //}
    //    //isCollidingU = colliderU.GetComponent<CollisionDetection>().isColliding();
    //    //if(isCollidingU)
    //    //{
    //    //    print("Colliding Up");
    //    //}
    //    //isCollidingD = colliderD.GetComponent<CollisionDetection>().isColliding();
    //    //if(isCollidingD)
    //    //{
    //    //    print("Colliding Down");
    //    //}
    //}

    // Detects if enter teleporter, set inTeleporter to true so we teleport when we're stopped
    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("GURT"); // Antonio, who is gurt?
        if ((collision.tag == "Teleporter") && (!inTeleporter))
        {
            inTeleporter = true;
            Teleporter script = collision.gameObject.GetComponent<Teleporter>();
            teleporterDest = script.destinationTeleporter;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        inTeleporter = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        inTeleporter = false;
    }

    private IEnumerator MovePlayerLerp(Vector3 direction)
    {
        if (canMove(direction))
        {
            lastDirection = direction;
            isMoving = true;

            float elapsedTime = 0;

            currentPos = transform.position;
            targetPos = currentPos + direction;

            while (elapsedTime < speed)
            {
                transform.position = Vector3.Lerp(currentPos, targetPos, (elapsedTime / speed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // forces player to end in the right position
            transform.position = targetPos;

            isMoving = false;

            // Teleporter logic
            if (inTeleporter && (teleporterDest != null))
            {
                print("WARP");
                transform.position = new Vector3(
                    teleporterDest.transform.position.x,
                    teleporterDest.transform.position.y,
                    transform.position.z
                );
                teleporterDest = null;
            }
        }
    }

    // checks if the tile the player is about to move to is not a wall tile and is a ground tile
    private bool canMove(Vector3 direction)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + direction);
        // if tile player is trying to move to is either not ground or a wall,
        if (wallTilemap.HasTile(gridPos))
        {
            return false; // player can't move.
        } else
        {
            switch (direction.ToString())
            {
                case "(0.0, 1.0, 0.0)":
                    if (neighborU)
                    {
                        return false;
                    }
                    break;
                case "(0.0, -1.0, 0.0)":
                    if (neighborD)
                    {
                        return false;
                    }
                    break;
                case "(-1.0, 0.0, 0.0)":
                    if (neighborL)
                    {
                        return false;
                    }
                    break;
                case "(1.0, 0.0, 0.0)":
                    if (neighborR)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

}
