using System.Collections;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private bool isMoving;
    private bool allowedMove = true;
    private bool inTeleporter = false;
    private GameObject teleporterDest = null;

    private Vector3 currentPos, targetPos;
    [Header("Speed")]
    public float speed = 0.2f;

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

    private Vector3 lastDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // get current grid coordinates
        Vector3Int currentGridPos = groundTilemap.WorldToCell(transform.position);

        if (!isMoving)
        {
            if (slideTilemap.HasTile(currentGridPos) && canMove(lastDirection)) // if current tile is a slide tile,
            {
                // force player to continue moving in same direction
                StartCoroutine(MovePlayerLerp(lastDirection));
                //Debug.Log("Sliding!");
            } 
            else if (conveyerU.HasTile(currentGridPos) && canMove(Vector3.up)) // if current tile is a conveyerbelt up tile
            {
                // force player to move up
                StartCoroutine(MovePlayerLerp(Vector3.up));
            } 
            else if (conveyerL.HasTile(currentGridPos) && canMove(Vector3.left)) // same logic for other directions
            {
                StartCoroutine(MovePlayerLerp(Vector3.left));
            }
            else if (conveyerR.HasTile(currentGridPos) && canMove(Vector3.right))
            {
                StartCoroutine(MovePlayerLerp(Vector3.right));
            }
            else if (conveyerD.HasTile(currentGridPos) && canMove(Vector3.down))
            {
                StartCoroutine(MovePlayerLerp(Vector3.down));
            }
            else
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
        // if ((!groundTilemap.HasTile(gridPos) && !slideTilemap.HasTile(gridPos)) || wallTilemap.HasTile(gridPos))
        if (wallTilemap.HasTile(gridPos) )
        {
            return false; // player can't move.
        }
        return true;
    }



}
