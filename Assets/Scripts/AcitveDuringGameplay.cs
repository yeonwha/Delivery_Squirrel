using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDuringGameplay : MonoBehaviour
{
    private void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_ACTIVE, isActive);
        Messenger.AddListener(GameEvent.GAME_INACTIVE, isInactive);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_ACTIVE, isActive);
        Messenger.RemoveListener(GameEvent.GAME_INACTIVE, isInactive);
    }
    void isActive()
    {
        this.enabled = true;
    }

    void isInactive()
    {
        this.enabled = false;
    }
}
