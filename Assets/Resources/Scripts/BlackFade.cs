using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFade : MonoBehaviour
{
    public static BlackFade instance;

    Animator animator;
    bool fadeOutDone = false;

    private void Awake()
    {
        instance = this;
        fadeOutDone = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateFadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeOutComplete()
    {
        fadeOutDone = true;
    }

    public bool isFadeOutComplete()
    {
        return fadeOutDone;
    }
}
