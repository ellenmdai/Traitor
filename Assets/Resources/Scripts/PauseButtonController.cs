using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonController : MonoBehaviour
{
    // public static PauseButtonController instance;
    public GameObject pauseMenuPrefab;
    GameObject scenePauseMenu;
    
    // Open pause menu
    public void onPauseClick() {
        if (scenePauseMenu == null) {
            Time.timeScale = 0f; // pause game
            scenePauseMenu = Instantiate(pauseMenuPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
