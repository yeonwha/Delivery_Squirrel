using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if the collider is Player
        if (collision.gameObject.tag == "Player")
        {
            // Destory the acorn after Player picked it up
            Destroy(this.gameObject);

            // Broadcast it is Player picking up the acorn
            Messenger.Broadcast(GameEvent.PICKUP_ACORN);


        }
    }
}
