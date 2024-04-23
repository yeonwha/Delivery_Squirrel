using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationArrived : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if the collider is Player
        if (collision.gameObject.tag == "Player")
        {
            // Broadcast it is Player reached the goal
            Messenger.Broadcast(GameEvent.BACK_HOME);
        }
    }
}
