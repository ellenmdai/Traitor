using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public GameObject endCreditsMenu;

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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other is CapsuleCollider2D && other.gameObject.GetComponent<PlayerController>())
        {
            //GameStats.LevelDeaths = 0;
            //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            //SoundManager.instance.PlayWalkSound();
            GameStats.EndTime = Epoch.Current();
            other.gameObject.transform.position = Vector3.zero;
            other.gameObject.GetComponent<PlayerController>().viewDirection = ViewDirection.Down;
            other.gameObject.GetComponent<PlayerController>().GameComplete = true;
            BlackFade.instance.AnimateFadeOut();
            StartCoroutine(CompleteAnimationAndShowCredits());
        }
    }

    IEnumerator CompleteAnimationAndShowCredits()
    {
        yield return new WaitWhile(() => !BlackFade.instance.isFadeOutComplete());
        //show deaths and time taken in menu with our names and button to send back to Main Menu
        yield return new WaitForSeconds(0.5f);
        endCreditsMenu.SetActive(true);
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
