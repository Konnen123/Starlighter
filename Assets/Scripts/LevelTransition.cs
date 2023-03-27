using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public static LevelTransition Instance;
    [SerializeField] private GameObject circleWipe;
    private Animator animator;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;


            StartCoroutine(waitResponse());
        }

    }

    IEnumerator waitResponse()
    {
        yield return null;
        animator = circleWipe.GetComponent<Animator>();
        if (PreviousLevel.Instance.lastLevel == String.Empty)
        {
            circleWipe.GetComponent<CanvasGroup>().alpha = 0;
        }
     
    }
    private void OnDestroy()
    {
        PreviousLevel.Instance.lastLevel = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string act)
    {
        StartCoroutine(StartAnimation(act));
    }

    public void playCircleWipeStart()
    {
        animator.Play("CircleWipeStart");
        animator.SetTrigger("End");
    }
    public void playCircleWipeEnd()
    {
     
        animator.Play("CircleWipeStart");
    }

    IEnumerator StartAnimation(string act)
    {
        circleWipe.GetComponent<CanvasGroup>().alpha=1;
        //circleWipe.SetActive(true);
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene(act);

    }

}
