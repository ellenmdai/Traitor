using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// https://fractalpixels.com/devblog/unity-2D-progress-bars
public class ShapeshiftTimerController : MonoBehaviour
{
    // Outlets
    public Slider timer;
    public Slider cooldown;
    public Image icon; // shows which role can be switched to ATM
    public GameObject player;
    public float cooldownTime = 2f; // default 2
    public float maxTime = 10f; // default 10
    public UnityEvent CooldownCompleteEvent;
    public UnityEvent TimerCompleteEvent;

    PlayerController playerScript;
    Sprite iconSprite;

    void Start()
    {
        timer.maxValue = maxTime;
        timer.value = 0f;
        cooldown.maxValue = cooldownTime;
        cooldown.value = cooldownTime;

        playerScript = player.GetComponent<PlayerController>();

        icon.sprite = playerScript.getSprite(playerScript.getCurrentRole());
        icon.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.value < maxTime) {    // is transformed; countdown timer
            timer.value -= Time.deltaTime;
            if (timer.value <= 0) {
                timer.value = maxTime;
                TimerCompleteEvent.Invoke();
            }
        }
        if (cooldown.value < cooldownTime) {  // allow player to change roles again
            cooldown.value -= Time.deltaTime;
            if (cooldown.value <= 0) {
                cooldown.value = cooldownTime;
                icon.color = Color.white;
                CooldownCompleteEvent.Invoke();
            }
        }
        // look for targets 
        icon.sprite = playerScript.getSprite(playerScript.getClosestRole());
        icon.color = playerScript.getClosestRole() == playerScript.getCurrentRole() ? Color.black : Color.white;
    }

    // assuming we've already checked it's legal to change, start timer and cooldown
    public void OnChangeRole() {
        Debug.Log("OnChangeRole");
        timer.value = maxTime;
        timer.value -= Time.deltaTime;
        cooldown.value -= Time.deltaTime;
        // hide target NPC icon
        icon.sprite = playerScript.getSprite(playerScript.getCurrentRole());
        icon.color = Color.black;
    }

}
