using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController instance;
    GameObject mainMenuButton;
    GameObject instructionsButton;

    public GameObject instructionsMenu;

    void Awake() {
        instance = this;
    }

    public void onResumeClick() {
        Debug.Log("destroy");
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void onInstructionsClick() {
        Debug.Log("open instructions");
        Instantiate(instructionsMenu, Vector3.zero, Quaternion.identity);
    }


}
