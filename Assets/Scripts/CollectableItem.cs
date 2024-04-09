using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private int value = 0;
    private int maxValue = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if the collider is Player
        if (collision.gameObject.tag == "Player")
        {

            value += 1;

            if (value > maxValue)
            {
                value = maxValue;
            }

            float valuePercent = ((float)value) / maxValue;
            // Broadcast it is Player picking up the heart
            Messenger<float>.Broadcast(GameEvent.PICKUP_ACORN, valuePercent);

            // Destory the heart after Player picked it up
            Destroy(this.gameObject);
        }
    }
}
