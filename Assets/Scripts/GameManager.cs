using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool isAcornRespawn = false; 


    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.BACK_HOME, OnBackHome);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<bool>.AddListener(GameEvent.ACORN_RESPAWN, OnAcornRespawn);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.BACK_HOME, OnBackHome);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<bool>.RemoveListener(GameEvent.ACORN_RESPAWN, OnAcornRespawn);
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

    IEnumerator WaitForFiveSec()
    {
        yield return new WaitForSeconds(5.0f);
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

        if (isAcornRespawn)
        {
            for (int i = 0; i < acornTotal; i++)
            {
                if (acorns[i] == null)
                {
                    StartCoroutine(WaitForFiveSec());
                    acorn = Instantiate(acornPrefab) as GameObject;
                    acorn.transform.position = acornSpawnPts[i];
                    acorns[i] = acorn;
                }
            }
        }

        //if (player.transform.position.y >= 44.5 && ui.acorns < 1)
        //{
        //    PlaceGoldenAcorn();
        //}
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

    void OnAcornRespawn(bool isOn)
    {
        isAcornRespawn = isOn;
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
}
