using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private bool isPlayerActive;
    private Camera mainCamera;
 
    [SerializeField] private HeadLook headLook;
    [SerializeField] private GameObject pressEnterText,mainMenuButtons,title,storyMode,levelHolder,optionMenu,player,eventSystem;   
    [SerializeField] private Vector3 cameraPositionOnEnter;
    [SerializeField] private float speed;


    private bool isStoryMode;
    private bool isStoryModeOff;
    private bool isOptionMode;
    private bool isOptionModeOff;

    private Vector3 currentPlayerPos;

    void Start()
    {
        currentPlayerPos = player.transform.parent.position;
        int isSoundActive = PlayerPrefs.GetInt("Sound");
        if (isSoundActive == 0)
            AudioListener.pause = false;
        else
            AudioListener.pause = true;
        
        LevelData levelData = SaveSystem.LoadLevel();
        int sceneIndex = 0;
        if (levelData != null)
            sceneIndex = levelData.level;

        for (int i = 0; i <= sceneIndex; i++)
        {
            levelHolder.transform.GetChild(i).GetComponent<Button>().interactable = true;
            levelHolder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
        mainCamera = Camera.main;
        title.SetActive(false);
        storyMode.SetActive(false);
        optionMenu.SetActive(false);
        mainCamera.transform.position = new Vector3(0, 0, -101f);
        pressEnterText.SetActive(true);
        mainMenuButtons.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPlayerActive)
        {
            Debug.Log("ok");
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.Return) && !isPlayerActive)
        {
            isPlayerActive = true;
            title.SetActive(true);
            pressEnterText.SetActive(false);
            mainMenuButtons.SetActive(true);
            headLook.enabled = true;
            StartCoroutine(DisableAnimOnTitle());

        }
        if(isPlayerActive)
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,cameraPositionOnEnter,speed);

        if (isStoryMode)
        {
            float fRef = 3;
            float x = Mathf.SmoothDamp(player.transform.parent.position.x, 2, ref fRef, .2f);
            
           player.transform.parent.position = new Vector3(x, player.transform.parent.position.y, player.transform.parent.position.z);
        }

        if (isStoryModeOff)
        {
            float fRef = -2f;
            float x = Mathf.SmoothDamp(player.transform.parent.position.x, currentPlayerPos.x, ref fRef, .2f);
            x = Mathf.Clamp(x, currentPlayerPos.x, 5);
            
            player.transform.parent.position = new Vector3(x, player.transform.parent.position.y, player.transform.parent.position.z);
        }
        if (isOptionMode)
        {
            float fRef = -1.5f;
            float x = Mathf.SmoothDamp(player.transform.parent.position.x, -5.44f, ref fRef, .2f);
            
            player.transform.parent.position = new Vector3(x, player.transform.parent.position.y, player.transform.parent.position.z);
        }
        if (isOptionModeOff)
        {
            float fRef = 3f;
            float x = Mathf.SmoothDamp(player.transform.parent.position.x, currentPlayerPos.x, ref fRef, .2f); 
            x = Mathf.Clamp(x,-5,currentPlayerPos.x);
            
            player.transform.parent.position = new Vector3(x, player.transform.parent.position.y, player.transform.parent.position.z);
        }
            
    }

    public void GoToStoryMenu()
    {
       
        
        mainMenuButtons.GetComponent<Animator>().Play("GoToStory");
        storyMode.SetActive(true);
        storyMode.GetComponent<Animator>().Play("ShowStoryMode");
        
        isStoryMode = true;

        StartCoroutine(SetActive());
    }

    public void GoToOptionsMenu()
    {
        mainMenuButtons.GetComponent<Animator>().Play("GoToOptions");
        optionMenu.SetActive(true);
        optionMenu.GetComponent<Animator>().Play("ShowOptionsMode");
        
        isOptionMode = true;
    
 


         StartCoroutine(SetOptionsActive());
    }
    
    public void HideOptionsMode()
    {
      
        mainMenuButtons.SetActive(true);
        player.SetActive(true);
        mainMenuButtons.GetComponent<Animator>().Play("ShowMenuFromOptions");
        optionMenu.GetComponent<Animator>().Play("HideOptionMenu");
        isOptionModeOff = true;
 

        StartCoroutine(SetMainMenuActiveFromOptions());
    }

    public void HideStoryMode()
    {
      
        mainMenuButtons.SetActive(true);
        player.SetActive(true);
        mainMenuButtons.GetComponent<Animator>().Play("ShowMenu");
        storyMode.GetComponent<Animator>().Play("HideStoryMode");
        isStoryModeOff = true;
 

        StartCoroutine(SetMainMenuActive());
    }
    
    IEnumerator SetOptionsActive()
    {
        yield return null;
        eventSystem.SetActive(false);
        yield return new WaitForSeconds(2f);
        eventSystem.SetActive(true);
        mainMenuButtons.SetActive(false);
        isOptionMode = false;
        player.SetActive(false);
    }

    IEnumerator SetActive()
    {
        yield return null;
        eventSystem.SetActive(false);
        yield return new WaitForSeconds(2f);
        eventSystem.SetActive(true);
        mainMenuButtons.SetActive(false);
        isStoryMode = false;
        player.SetActive(false);
    }
    IEnumerator SetMainMenuActive()
    {
        yield return null;
        eventSystem.SetActive(false);
        yield return new WaitForSeconds(2f);
        eventSystem.SetActive(true);
        storyMode.SetActive(false);
        isStoryModeOff = false;
        player.transform.parent.position = currentPlayerPos;
  

    }
    IEnumerator SetMainMenuActiveFromOptions()
    {
        yield return null;
        eventSystem.SetActive(false);
        yield return new WaitForSeconds(2f);
        eventSystem.SetActive(true);
        optionMenu.SetActive(false);
        isOptionModeOff = false;
        player.transform.parent.position = currentPlayerPos;
  

    }

    IEnumerator DisableAnimOnTitle()
    {
        //make sure the animation is done once
        yield return new WaitForSeconds(3f);
        title.GetComponent<Animator>().enabled = false;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
