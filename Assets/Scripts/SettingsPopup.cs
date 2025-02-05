using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPopup : BasePopup
{
    [SerializeField] private Toggle acornToggle;
    [SerializeField] private Toggle effectSoundToggle;

    [SerializeField] private Slider musicSlider;
   // [SerializeField] AudioClip musicTrack;

    private void Start()
    {
        //musicSlider.value = SoundManager.Instance.MusicVolume;
    }

    public override void Open()
    {
        base.Open();
        acornToggle.isOn = (PlayerPrefs.GetInt(PlayerPrefConstants.ACORN_RESPAWN) != 0);
        effectSoundToggle.isOn = (PlayerPrefs.GetInt(PlayerPrefConstants.EFFECT_SOUND) != 0);
        musicSlider.value = PlayerPrefs.GetFloat("volume", 1);
    }
    // Update is called once per frame
    void Update()
    {

    }

    // check all the setting values and set as they are saved
    public void OnOkButton()
    {
        PlayerPrefs.SetInt(PlayerPrefConstants.ACORN_RESPAWN, (acornToggle.isOn ? 1 : 0));
        PlayerPrefs.SetInt(PlayerPrefConstants.EFFECT_SOUND, (effectSoundToggle.isOn ? 1 : 0));
        PlayerPrefs.SetFloat("volume", musicSlider.value);
        Close();
    }

    public void OnCancelButton()
    {
        Close();
    }

    // when the volume slide changed, background music volume changes
    public void OnMusicVolumeChanged(float value)
    {
        Debug.Log("Music vol changed to:" + value);
        SoundManager.Instance.MusicVolume = value;
    }
}
