﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When player reaches exit, load next scene.
    // For now though, just reload the scene
    void OnTriggerEnter2D(Collider2D other) {
        if(other is BoxCollider2D && other.gameObject.GetComponent<PlayerController>()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
