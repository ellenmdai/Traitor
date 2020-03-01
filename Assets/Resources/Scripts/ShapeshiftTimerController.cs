using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** HOW TO USE
The ShapeshiftInfo GameObject (which this script is attached to) uses UnityEvents to
talk with the Player object. It's kinda tricky to configure, so this is what you do:
** NOTE: when setting GameObjects as parameters, MAKE SURE they're from the scene, not the 
    prefabs!!! AKA drag from the heirarchy or in the menu, use the Scene, not Assets tab. 

1. Drag the OverlayBar prefab into the scene. Change Render Camera to the scene's Main Camera.
2. Add a new UI > EventSystem object to the scene.
3. In ShapeshiftInfo's script component, set Player to the Scene's player
4. For CooldownCompleteEvent, add a thing to the list and select 'runtime only.' Set it 
    to be the scene's Player and select onCooldownComplete for the function.
5. Do the same for TimerCompleteEvent, except with onTimerComplete.
6. In the Player object's script component, for ChangeRoleEvent, set it to runtime only,
    add the ShapeshiftInfo object, and set the function to OnChangeRole.
**/

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
        Role closestRole = playerScript.getClosestRole();
        icon.sprite = playerScript.getSprite(closestRole);
        icon.color = (closestRole == playerScript.getCurrentRole() || closestRole == Role.Player) 
            ? Color.black : Color.white;
    }

    // assuming we've already checked it's legal to change, start timer and cooldown
    public void OnChangeRole() {
        // Debug.Log("OnChangeRole");
        timer.value = maxTime;
        timer.value -= Time.deltaTime;
        cooldown.value -= Time.deltaTime;
        // hide target NPC icon
        // icon.sprite = playerScript.getSprite(playerScript.getCurrentRole());
        // icon.color = Color.black;
    }

}
