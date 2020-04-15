using System.Collections;
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
    void OnTriggerEnter2D(Collider2D other) {
        if(other is CapsuleCollider2D && other.gameObject.GetComponent<PlayerController>()) {
            GameStats.TotalDeaths += GameStats.LevelDeaths;
            GameStats.LevelDeaths = 0;
            //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            SoundManager.instance.PlayWalkSound();
            BlackFade.instance.AnimateFadeOut(true);
            StartCoroutine(CompleteAnimationAndLoadLevel());
        }
    }

    IEnumerator CompleteAnimationAndLoadLevel()
    {
        yield return new WaitWhile(() => !BlackFade.instance.isFadeOutComplete());
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
