using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool isMoving;
    private bool allowedMove = true;
    private Vector3 currentPos, targetPos;
    [Header("Speed")]
    public float speed = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving && allowedMove)
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

    public void MovePlayer(InputAction.CallbackContext context)
    {
        //Debug.Log("MovePlayer()");
        //if (!isMoving)
        //{
        //    if(context.ReadValue<Vector2>().x < 0.0)
        //    {
        //        StartCoroutine(MovePlayerLerp(Vector3.left));
        //    }
        //    if (context.ReadValue<Vector2>().x > 0.0)
        //    {
        //        StartCoroutine(MovePlayerLerp(Vector3.right));
        //    }
        //    if (context.ReadValue<Vector2>().y < 0.0)
        //    {
        //        StartCoroutine(MovePlayerLerp(Vector3.down));
        //    }
        //    if (context.ReadValue<Vector2>().y > 0.0)
        //    {
        //        StartCoroutine(MovePlayerLerp(Vector3.up));
        //    }
        //}
    }

    private IEnumerator MovePlayerLerp(Vector3 direction)
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
