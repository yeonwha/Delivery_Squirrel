using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
//using UnityEditorInternal;

public class UIController : MonoBehaviour
{
    private int acorns = 0;
    private int maxAcorns = 3;

    private List<FoodItem> foods = new List<FoodItem>();

    [SerializeField] private TextMeshProUGUI acornValue;

    [SerializeField] private TextMeshProUGUI foodValue;

    [SerializeField] private Image acornBar;

    [SerializeField] private GameOverPanel gameOverPanel;

    [SerializeField] private SettingsPopup settingsPopup;

    private int popupsActive = 0;
    private void Awake()
    {
        //Messenger.AddListener(GameEvent.PICKUP_ACORN, OnAcornValueChanged);
        Messenger<FoodItem>.AddListener(GameEvent.PICKUP_FOOD, OnFoodChanged);
        Messenger<string>.AddListener(GameEvent.ENEMY_CONTACT, OnEnemyContact);
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    private void OnDestroy()
    {
       // Messenger.RemoveListener(GameEvent.PICKUP_ACORN, OnAcornValueChanged);
        Messenger<FoodItem>.RemoveListener(GameEvent.PICKUP_FOOD, OnFoodChanged);
        Messenger<string>.RemoveListener(GameEvent.ENEMY_CONTACT, OnEnemyContact);
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
    }

    // empty the food ui value, and acorn bar
    void Start()
    {
        foods.Clear();
        acornBar.fillAmount = 0;
    }

    // check if setting popups is open or not to open it when ESC key's pressed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsPopup.IsActive())
        {
            settingsPopup.Open();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPopup.IsActive())
            {
                settingsPopup.Close();
            }
        }
    }

    // when a popup is open, deactivate the game scene
    public void SetGameActive(bool active)
    {
        if (active)
        {
            Time.timeScale = 1;                         // unpause the game
        }
        else
        {
            Time.timeScale = 0;                         // pause the game
        }
    }

    // change the acorn bar based on the acorn amount that player picked
    public void CollectAcorn()
    {
        acorns++;
        if (acorns > maxAcorns) {
            acorns = maxAcorns;
        }
        float acornPercentage = (acorns * 1.0f) / maxAcorns;
        UpdateAcorn(acornPercentage);
    }

    void UpdateAcorn(float acornRate)
    {
        acornBar.fillAmount = acornRate;
    }

    // show food's owner types that player collected. player can carry only 3 foods. 
    public void OnFoodChanged(FoodItem item)
    {
        if (foods.Count < 3)
        {
            foods.Add(item);
        }
        else
        {
            foods.RemoveAt(0);    // when player already has 3 foods, remove the first food of the list
            foods.Add(item);      // add new food
        }
        Debug.Log(foods.Count);
        Debug.Log("collected food for: " + item.GetOwner());

        UpdateFoodValue();
    }

    // when collide with enemy, they take their food otherwise take the acorn
    void OnEnemyContact(string enemy)
    {
        bool foodTaken = false;
        if (foods.Count > 0)
        {   
            foreach (FoodItem food in foods)
            {
                if (food.GetOwner().ToString() == enemy)
                {
                    foods.Remove(food);
                    foodTaken = true;
                    break;
                }
            }

            UpdateFoodValue();

            if (!foodTaken)
            {
                if (acorns > 0)
                {
                    acorns--;
                    UpdateAcorn((float)acorns / 3f);
                }
                else
                {
                    Messenger.Broadcast(GameEvent.PLAYER_DEAD);
                }
            }

        }
        else
        {
            if (acorns > 0)
            {
                acorns--;
                UpdateAcorn((float)acorns / 3f);
            }
            else
            {
                Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            }
        }
    }

    // update ui of food information
    void UpdateFoodValue()
    {
        string show = "";

        if (foods.Count > 0)
        {
            foreach (FoodItem food in foods)
            {
                string text = food.GetOwner().ToString().Substring(0,1) + " ";
                
                show += text;
                
                foodValue.text = show;
            }
        }
        else
        {
            foodValue.text = "";
        }
    }

    void OnPopupOpened()
    {
        if (popupsActive == 0)
        {
            SetGameActive(false);
        }
        popupsActive++;
    }

    void OnPopupClosed()
    {
        popupsActive--;
        if (popupsActive == 0)
        {
            SetGameActive(true);
        }
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.Open();
        gameOverPanel.SetAcornCount(acorns);
    }

    public int getAcornsNum()
    {
        return acorns;
    }
}
