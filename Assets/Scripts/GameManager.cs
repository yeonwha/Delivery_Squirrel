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

    private GameObject[] acorns;
    private const int acornTotal = 3;

    private GameObject acorn;
    [SerializeField] private Transform acorn1Pt;
    [SerializeField] private Transform acorn2Pt;
    [SerializeField] private Transform acorn3Pt;

    [SerializeField] private GameObject acornPrefab;


    private Vector3[] acornSpawnPts;

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
        if (player.transform.position.y < scaredY)
        {
            player.Falling();

            if(player.transform.position.y < minY)
            {
                //player.Die();
                PlayerDeath();
            }
        }
    }

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

    void OnRestartGame()
    {
        SceneManager.LoadScene(0);
        //player.Respawn(startPt);
        PlaceAcorns();
    }

    IEnumerator WaitForFiveSec()
    {
        yield return new WaitForSeconds(5.0f);
    }

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

        //yield return;
    }

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

        //yield return;
    }

    void OnPlayerDead()
    {
        StartGameLostSequence();
        //player.Die();
    }

    void PlaceGoldenAcorn()
    {
        golden_acorn.SetActive(true);
    }

    void OnBackHome()
    {
        //ui.ShowGameOverPanel();
        StartGameWonSequence();
    }


    void StartGameLostSequence()
    {
        // play sad sound effect
        // popup?
        ui.ShowGameOverPanel();
    }

    void StartGameWonSequence()
    {
        // play happy sound effect
        ui.ShowGameOverPanel();
    }

    public bool isAcornRespawnActive()
    {
        bool isActive = PlayerPrefs.GetInt(PlayerPrefConstants.ACORN_RESPAWN) != 0;
        return isActive;
    }
}
