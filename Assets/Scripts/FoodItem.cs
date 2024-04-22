using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public enum OwnerType { Vulture, Opossum, Pig };

    [SerializeField] OwnerType ownerType;
    //string target = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            //target = this.gameObject.tag;
            Debug.Log("OTE2d " + collision.gameObject.tag);

            Messenger<FoodItem>.Broadcast(GameEvent.PICKUP_FOOD, this);
         
        }
    }

    public OwnerType GetOwner()
    {
        return ownerType;
    }
}
