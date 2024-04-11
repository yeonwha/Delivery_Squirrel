using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Scene1Controller : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField] AudioClip sfxLaser;
    [SerializeField] AudioClip musicTrack;

        // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = SoundManager.Instance.MusicVolume;
        //sfxSlider.value = SoundManager.Instance.SfxVolume;
    }


    public void OnChangeScene(string sceneName)
    {
        Debug.Log("OnChangeScene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // -----------------------------
    // SFX UI METHODS
    // -----------------------------

    public void OnPlaySfx() {       
        Debug.Log("OnPlaySfx()");     
        //SoundManager.Instance.PlaySfx(sfxLaser);
    }

    public void OnSfxVolumeChanged(float value)
    {
        Debug.Log("OnSfxVolumeChanged() value:" + value);
       //SoundManager.Instance.SfxVolume = value;           
    }


    // -----------------------------
    // MUSIC UI METHODS
    // -----------------------------

    public void OnStartMusic()
    {
        Debug.Log("OnStartMusic()");
        SoundManager.Instance.PlayMusic(musicTrack);
    }

    public void OnStopMusic()
    {
        Debug.Log("OnStopMusic()");
        SoundManager.Instance.StopMusic();
    }

    public void OnMusicVolumeChanged(float value)
    {
        Debug.Log("Music vol changed to:" + value);
        SoundManager.Instance.MusicVolume = value;    
    }       
}
