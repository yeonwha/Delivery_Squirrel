using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditorInternal;

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

    // Start is called before the first frame update
    void Start()
    {
        foods.Clear();
        acornBar.fillAmount = 0;
    }

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
    public void SetGameActive(bool active)
    {
        if (active)
        {
            Time.timeScale = 1;                         // unpause the game

            //Messenger.Broadcast(GameEvent.GAME_ACTIVE); // broadcast when resumed
        }
        else
        {
            Time.timeScale = 0;                         // pause the game

           // Messenger.Broadcast(GameEvent.GAME_INACTIVE); // broadcast when paused
        }
    }

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

    public void OnFoodChanged(FoodItem item)
    {
        if (foods.Count < 3)
        {
            foods.Add(item);
        }
        else
        {
            foods.RemoveAt(0);
            foods.Add(item);
        }
        Debug.Log(foods.Count);
        Debug.Log("collected food for: " + item.GetOwner());

        UpdateFoodValue();
    }

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
                    //acornValue.text = acorns.ToString();
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
                //acornValue.text = acorns.ToString();
            }
            else
            {
                Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            }
        }
    }

    void UpdateFoodValue()
    {
        string show = "";

        if (foods.Count > 0)
        {
            foreach (FoodItem food in foods)
            {
                string text = food.GetOwner().ToString() + " ";
                
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
