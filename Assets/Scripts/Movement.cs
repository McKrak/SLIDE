using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    bool playerAlive = true;
    bool isWin = false;

    [Header("Level UI Object")]
    public GameObject levelUI;


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
    [Header("Breakable Tilemap")]
    public Tilemap breakableTilemap;
    [Header("Goal Tilemap")]
    public Tilemap goalTilemap;

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
        // get current grid coordinates
        Vector3Int currentGridPos = groundTilemap.WorldToCell(transform.position);

        if (!isMoving && playerAlive)
        {
            if ((breakableTilemap != null) && breakableTilemap.HasTile(currentGridPos))
            {
                playerAlive = false;
                PlayerDie();
            }
            else if (goalTilemap != null && goalTilemap.HasTile(currentGridPos))
            {
                playerAlive = false;
                isWin = true;
                levelUI.SetActive(true);
            }
            
            else if (slideTilemap.HasTile(currentGridPos) && canMove(lastDirection) && slidesOnIce) // if current tile is a slide tile,
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
        } else if (isWin)
        {
            if (Keyboard.current.anyKey.isPressed)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }

    }


    // Detects if enter teleporter, set inTeleporter to true so we teleport when we're stopped
    void OnTriggerEnter2D(Collider2D collision)
    {
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
        }
        return true;
    }

    void PlayerDie()
    {
        StartCoroutine(restartScene());
    }

    void PlayerGoal()
    {
        
    }

    IEnumerator restartScene()
    {
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
