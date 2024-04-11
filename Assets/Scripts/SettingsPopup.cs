using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPopup : BasePopup
{
    [SerializeField] private Toggle toggle;


    public override void Open()
    {
        base.Open();
        toggle.isOn = (PlayerPrefs.GetInt(PlayerPrefConstants.ACORN_RESPAWN) != 0);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnOkButton()
    {
        PlayerPrefs.SetInt(PlayerPrefConstants.ACORN_RESPAWN, (toggle.isOn ? 1 : 0));
        Close();
        //PlayerPrefs.set
    }

    public void OnCancelButton()
    {
        Close();
    }

}
