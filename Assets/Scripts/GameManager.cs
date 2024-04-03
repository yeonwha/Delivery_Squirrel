using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private const float minY = -50.0f;     // minimum Y where player can survive

    [SerializeField] Transform startPt;   // Player's start point when game starts or restarts

    private GameObject[] acorns;
    private const int acornTotal = 3;

    private GameObject acorn;
    [SerializeField] private Transform acorn1Pt;
    [SerializeField] private Transform acorn2Pt;
    [SerializeField] private Transform acorn3Pt;

    [SerializeField] private GameObject acornPrefab;


    private Vector3[] acornSpawnPts;

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
        if (player.transform.position.y < minY)
        {
            player.Die();
            player.Respawn(startPt);
        }
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
}
