using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{

    void loadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void loadLevel00() {
        loadLevel("Level 00");
    }

    
    public void loadLevel01() {
        loadLevel("Level 01");
    }

    
    public void loadLevel02() {
        loadLevel("Level 02");
    }
    
    public void loadLevel03() {
        loadLevel("Level 03");
    }

    
    public void loadLevel04() {
        loadLevel("Level 04");
    }

    
    public void loadLevel05() {
        loadLevel("Level 05");
    }

    public void onBackClick() {
        Destroy(gameObject);
    }


}
