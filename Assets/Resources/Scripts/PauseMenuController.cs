using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController instance;
    GameObject mainMenuButton;
    GameObject instructionsButton;

    void Awake() {
        instance = this;
    }

    public void onResumeClick() {
        Debug.Log("destroy");
        Time.timeScale = 1f;
        Destroy(gameObject);
    }


}
