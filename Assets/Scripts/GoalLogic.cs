using UnityEngine;
using UnityEngine.Tilemaps;

public class GoalLogic : MonoBehaviour
{
    private TilemapCollider2D collider;

    private void ScriptFetch(){
        collider = GetComponentInChildren<TilemapCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other){
       
    }

}
