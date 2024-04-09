using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    // when ESC key pressed
    virtual public void Open()
    {
        if (!IsActive())
        {
            this.gameObject.SetActive(true);     // Popup object
            Messenger.Broadcast(GameEvent.POPUP_OPENED);
        }
        else
        {
            Debug.LogError(this + ".Open() - trying to open a popup that is active!");
        }
    }

    public void Close()
    {
        if (IsActive())
        {
            this.gameObject.SetActive(false);
            Messenger.Broadcast(GameEvent.POPUP_CLOSED);
        }
        else
        {
            Debug.LogError(this + ".Closed() - trying to close a popup that is inactive!");
        }
    }


    // check whether OptionsPopup is open or not
    virtual public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
