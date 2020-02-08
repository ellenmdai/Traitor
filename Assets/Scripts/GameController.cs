using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;  // singleton
    // ref to player cuz scrolling dependent on player's location on screen
    public GameObject player; 
    public Transform bgSpawnPoint;
    public GameObject bgTilePrefab;
    public GameObject[] bgTiles;   // bg tiles to produce

    public float timeElapsed; // state tracking
    void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
    }

    void SpawnBgTile() {
        Instantiate(bgTilePrefab, bgSpawnPoint.position, Quaternion.identity);
    }
}
