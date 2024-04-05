using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIController : MonoBehaviour
{
    private int acorns = 0;
    private Queue<FoodItem> foods = new Queue<FoodItem>();

    [SerializeField] private TextMeshProUGUI acornValue;

    [SerializeField] private TextMeshProUGUI foodValue;

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PICKUP_ACORN, OnAcornValueChanged);
        Messenger<FoodItem>.AddListener(GameEvent.PICKUP_FOOD, OnFoodChanged);
        //Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        //Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    private void OnDestroy()
    {
        Messenger<FoodItem>.RemoveListener(GameEvent.PICKUP_FOOD, OnFoodChanged);
        //Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        //Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    // Start is called before the first frame update
    void Start()
    {
        foods.Clear();
    }

    void OnAcornValueChanged(int acorn)
    {
        acorns++;
        acornValue.text = acorns.ToString();
    }

    void OnFoodChanged(FoodItem item)
    {
        if (foods.Count < 3)
        {
            foods.Enqueue(item);
        }
        else
        {
            foods.Dequeue();
            foods.Enqueue(item);
        }
        Debug.Log(foods.Count);
        Debug.Log("collected food for: " + item.GetOwner());
        
        string show = "";
        foreach (FoodItem food in foods)
        {
             string text = food.GetOwner().ToString() + " ";
             show += text;
            foodValue.text = show;
        }
    }
}
