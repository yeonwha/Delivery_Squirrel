using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPopup : BasePopup
{
    [SerializeField] private Toggle toggle;

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            Messenger<bool>.Broadcast(GameEvent.ACORN_RESPAWN, toggle.isOn);
        }
    }

    
}
