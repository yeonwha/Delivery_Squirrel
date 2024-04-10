using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : BasePopup
{
    [SerializeField] private TextMeshProUGUI scoreValue;

    private int score = 0;

    private bool pickedGoldenAcorn = false;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PICKUP_GOLD, OnPickupGoldenAcorn);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PICKUP_GOLD, OnPickupGoldenAcorn);
    }


    private void OnPickupGoldenAcorn()
    {
        pickedGoldenAcorn = true;
    }

    public void SetAcornCount(int acornCount)
    {
        score += acornCount;
        
        if (pickedGoldenAcorn)
        {
            score += 99;
        }

        scoreValue.text = score.ToString();
    }

    public void OnExitGameButton()
    {
        Debug.Log("Exiting Gmae");
        Application.Quit();
    }

    public void OnStartAgainButton()
    {
        Close();
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }
}
