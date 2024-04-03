using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private int value = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if the collider is Player
        if (other.tag == "Player")
        {
            // Broadcast it is Player picking up the heart
            Messenger<int>.Broadcast(GameEvent.PICKUP_ACORN, value);

            // Destory the heart after Player picked it up
            Destroy(this.gameObject);
        }
    }
}
