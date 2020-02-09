using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;  // singleton
    // ref to player cuz scrolling dependent on player's location on screen
    public GameObject player; 
    public Transform bgSpawnPoint;
    public GameObject[] bgTilePrefabs;  // bg tiles to produce
  
    public float scrollSpeed;
    public Transform destroyPoint;  // where off-screen tiles should be destroyed to free up memory

    private int numTilesSpawned;    // for logic and scorekeeping
    private float progressSinceLastSpawn; // to track when to spawn new tile
    private float tileWidth;
    void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        numTilesSpawned = 0;
        progressSinceLastSpawn = 0;
        tileWidth = 2*bgTilePrefabs[0].GetComponent<SpriteRenderer>().bounds.extents.x;
        Debug.Log("tileWidth: " + tileWidth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) {
            progressSinceLastSpawn += scrollSpeed;
        }
        Debug.Log(progressSinceLastSpawn % tileWidth);
        if (progressSinceLastSpawn >= tileWidth) {      // SLIGHTLY GLITCHY--small gaps btwn tiles
            SpawnBgTile();
        }
    }

    void SpawnBgTile() {
        progressSinceLastSpawn = 0;
        GameObject randomBgTilePrefab = bgTilePrefabs[Random.Range(0, bgTilePrefabs.Length)];
        GameObject newTile = Instantiate(randomBgTilePrefab, bgSpawnPoint.position, Quaternion.identity);
        newTile.GetComponent<bgTileController>().destroyPoint = destroyPoint;
        newTile.GetComponent<bgTileController>().scrollSpeed = scrollSpeed;
        numTilesSpawned++;
    }
}
