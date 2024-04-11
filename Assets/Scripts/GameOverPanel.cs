using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : BasePopup
{
    [SerializeField] private TextMeshProUGUI scoreValue;

    private int score = 0;

    public void SetAcornCount(int acornCount)
    {
        score += acornCount;
        
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
