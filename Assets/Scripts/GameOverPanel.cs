using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : BasePopup
{
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI tipText;
  
    private int score = 0;

    public void SetAcornCount(int acornCount)
    {
        score += acornCount;
        
        scoreValue.text = score.ToString();
    
        switch (acornCount)
        {
            case 1:
                tipText.text = "Sorry, no tip for 1 acorn.";
                break;
            case 2:
                tipText.text = "Not bad. here's $1.32 for your tip!";
                break;
            case 3:
                tipText.text = "Lovely. take $100 for your tip!";
                break;
            default:
                tipText.text = "loser...";
                break;
        }
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
