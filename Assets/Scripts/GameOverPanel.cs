using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : BasePopup
{

    [SerializeField] private TextMeshProUGUI acornNumber;
    //// Start is called before the first frame update
    //public void Open(int acornCount)
    //{
    //    this.Open();
    //    acornNumber.text = acornCount.ToString();
    //}

    public void SetAcornCount(int acornCount)
    {
        acornNumber.text = acornCount.ToString();
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
