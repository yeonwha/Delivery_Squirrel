using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] UIController ui;
    [SerializeField] GameObject golden_acorn;

    private const float minY = -60.0f;     // minimum Y where player can survive
    private const float scaredY = -40.0f;  // Y where player's hurt animation triggered

    [SerializeField] Transform startPt;   // Player's start point when game starts or restarts

    private GameObject[] acorns;          // count and store the acorn collected
    private const int acornTotal = 3;     // maximum total number of acorn

    private GameObject acorn;             // reference to acorn item
    [SerializeField] private Transform acorn1Pt;     // a acorn's position in the scene
    [SerializeField] private Transform acorn2Pt;     // a acorn's position in the scene
    [SerializeField] private Transform acorn3Pt;     // a acorn's position in the scene
    private Vector3[] acornSpawnPts;      // acorns' positions

    [SerializeField] private GameObject acornPrefab;   // acorn's prefab to place in the scene

    private bool isGameOver = false;      // check if player's dead or not

    [SerializeField] AudioClip musicTrack;            // background music
    [SerializeField] AudioClip clapsSound;            // claps sound when player reaches the goal point

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.BACK_HOME, OnBackHome);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.AddListener(GameEvent.PICKUP_ACORN, OnPickupAcorn);

    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.BACK_HOME, OnBackHome);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.RemoveListener(GameEvent.PICKUP_ACORN, OnPickupAcorn);

    }

    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.Instance.PlayMusic(musicTrack);

        // place acrons in the scene
        acornSpawnPts = new Vector3[acornTotal];

        acornSpawnPts[0] = acorn1Pt.transform.position;
        acornSpawnPts[1] = acorn2Pt.transform.position;
        acornSpawnPts[2] = acorn3Pt.transform.position;
        
        acorns = new GameObject[acornTotal];
        PlaceAcorns();
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is falling and make it die when fallen down too deep
        if (player.transform.position.y < scaredY)
        {
            player.Falling();

            if(player.transform.position.y < minY)
            {
                // trigger the player death if the game is not over
                if (!isGameOver)
                {
                    isGameOver = true;
                    PlayerDeath();
                }
            }
        }
    }

    // when player picked up acorn, update Ui and respawn if easy mode is on
    void OnPickupAcorn()
    {
        ui.CollectAcorn();
        if (isAcornRespawnActive() && ui.getAcornsNum() < acornTotal)
        {
            StartCoroutine(RespawnAcorns());
        }
    }

    void PlayerDeath()
    {
        StartGameLostSequence();
    }

    // if game is restarted, back to the first loadscene and place acorns again
    void OnRestartGame()
    {
        SceneManager.LoadScene(0);
        PlaceAcorns();
    }

    // place acorn items at the position
    void PlaceAcorns()
    {
        for(int i = 0; i < acornTotal; i++)
        {
            if (acorns[i] == null)
            {
                acorn = Instantiate(acornPrefab) as GameObject;
                acorn.transform.position = acornSpawnPts[i];
                acorns[i] = acorn;  
            }
        }
    }

    // respawn acorns after 5 sec when easy mode is on
    IEnumerator RespawnAcorns()
    {
         yield return 0;   // wait for the frame finishes
        
        for (int i = 0; i < acornTotal; i++)
        {
            if (acorns[i] == null)
            {

                yield return new WaitForSeconds(5.0f);

                acorn = Instantiate(acornPrefab) as GameObject;
                acorn.transform.position = acornSpawnPts[i];
                acorns[i] = acorn;
            }
        }
    }

    // when player dies, call the gameover panel
    void OnPlayerDead()
    {
        StartGameLostSequence();
    }

    // when player reached the goal point, call the gameover panel
    void OnBackHome()
    {
        StartGameWonSequence();
    }

    // when player dies, gameover panel's up with no clap sound
    void StartGameLostSequence()
    {
        SoundManager.Instance.StopMusic();
        ui.ShowGameOverPanel();
    }

    // when player reached the goal, game over panel's up with clap sound
    void StartGameWonSequence()
    {
        // play happy sound effect
        SoundManager.Instance.PlaySfx(clapsSound);
        SoundManager.Instance.StopMusic();
        ui.ShowGameOverPanel();
    }

    // check if easy mode is on
    public bool isAcornRespawnActive()
    {
        bool isActive = PlayerPrefs.GetInt(PlayerPrefConstants.ACORN_RESPAWN) != 0;
        return isActive;
    }
}
