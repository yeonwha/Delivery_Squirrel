using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public enum OwnerType { Vulture, Opossum, Pig };     // food's owner types

    [SerializeField] OwnerType ownerType;               // reference to the enemy


    // when player pick up this item, destroy itself and broadcast to change the ui
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            Debug.Log("OTE2d " + collision.gameObject.tag);

            Messenger<FoodItem>.Broadcast(GameEvent.PICKUP_FOOD, this);
         
        }
    }

    public OwnerType GetOwner()
    {
        return ownerType;
    }
}
