using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : MonoBehaviour
{
    [SerializeField] private Image acornbg;
    [SerializeField] AudioClip musicTrack;
    // Start is called before the first frame update
    void Start()
    {
        acornbg.fillAmount = 1;
        SoundManager.Instance.StopMusic();
        Messenger.Broadcast(GameEvent.POPUP_OPENED);
    }

    public void Close()
    {
        if (gameObject.activeSelf)
        {
            SoundManager.Instance.PlayMusic(musicTrack);
            this.gameObject.SetActive(false);

            Messenger.Broadcast(GameEvent.POPUP_CLOSED);
        }
        else
        {
            Debug.LogError(this + ".Closed() - trying to close a popup that is inactive!");
        }
    }

    public void OnStartAgainButton()
    {
        Close();
    }
}
