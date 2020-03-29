﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
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

    public void onBackToMainClick() {
        SceneManager.LoadScene("Main Menu");
    }

    public void onInstructionsClick() {
        Debug.Log("open instructions");
        Instantiate(instructionsMenu, Vector3.zero, Quaternion.identity);
    }

    public void onPlayClick() {
        SceneManager.LoadScene("Level 00");
    }

    public void onSceneSelectionClick() {
        Debug.Log("Scene selection pressed; TODO");
    }


}