using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private bool isPlayerActive;
    private Camera mainCamera;
 
    [SerializeField] private HeadLook headLook;
    [SerializeField] private GameObject pressEnterText,mainMenuButtons,title,storyMode,levelHolder;
    [SerializeField] private Vector3 cameraPositionOnEnter;
    [SerializeField] private float speed;
    void Start()
    {
        int sceneIndex = PlayerPrefs.GetInt("sceneIndex");
        Debug.Log(sceneIndex);

        for (int i = 0; i <= sceneIndex; i++)
        {
            levelHolder.transform.GetChild(i).GetComponent<Button>().interactable = true;
            levelHolder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
        mainCamera = Camera.main;
        title.SetActive(false);
        storyMode.SetActive(false);
        mainCamera.transform.position = new Vector3(0, 0, -101f);
        pressEnterText.SetActive(true);
        mainMenuButtons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isPlayerActive)
        {
            isPlayerActive = true;
            title.SetActive(true);
            pressEnterText.SetActive(false);
            mainMenuButtons.SetActive(true);
            headLook.enabled = true;

        }
        if(isPlayerActive)
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,cameraPositionOnEnter,speed);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
