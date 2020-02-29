using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://fractalpixels.com/devblog/unity-2D-progress-bars
public class ShapeshiftTimerController : MonoBehaviour
{
    // Outlets
    public Slider slider;
    public Image icon; // shows which role can be switched to ATM

    public GameObject player;
    public float cooldownTime = 2f; // default 2
    public float maxTime = 10f; // default 10

    PlayerController playerScript;
    Sprite iconSprite;

    // State tracking
    float cooldownLeft;

    void Start()
    {
        slider.maxValue = maxTime;
        slider.value = 0f;
        cooldownLeft = cooldownTime;

        playerScript = player.GetComponent<PlayerController>();

        icon.sprite = playerScript.getSprite(playerScript.getCurrentRole());
        icon.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.getCurrentRole() != Role.Player) {    // countdown timer
            slider.value -= Time.deltaTime;
            if (slider.value <= 0) {
                playerScript.resetToPlayerRole();
                icon.sprite = playerScript.getSprite(playerScript.getCurrentRole());
                icon.color = Color.black;
                cooldownLeft = cooldownTime;
            }
        }
        else if (cooldownLeft > 0) {  // wait for cooldown
            cooldownLeft -= Time.deltaTime;
        }
        else {  // look for targets 
            icon.sprite = playerScript.getSprite(playerScript.getClosestRole());
            icon.color = playerScript.getClosestRole() == Role.Player ? Color.black : Color.white;
            slider.value = maxTime; // inefficient
        }
    }

}
