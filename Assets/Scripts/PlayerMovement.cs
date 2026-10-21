using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private bool isMoving;
    private bool allowedMove = true;
    private Vector3 currentPos, targetPos;
    [Header("Speed")]
    public float speed = 0.2f;
    
    [Header("Ground Tilemap")]
    public Tilemap groundTilemap;
    
    [Header("Wall Tilemap")]
    public Tilemap wallTilemap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
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
    private IEnumerator MovePlayerLerp(Vector3 direction)
    {
        if (canMove(direction))
        {
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
        }
    }

    // checks if the tile the player is about to move to is not a wall tile and is a ground tile
    private bool canMove(Vector3 direction)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + direction);
        // if tile player is trying to move to is either not ground or a wall,
        if(!groundTilemap.HasTile(gridPos) || wallTilemap.HasTile(gridPos))
        {
            return false; // player can't move.
        }
        return true;
    }

}
