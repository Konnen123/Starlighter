using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandleTransform;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color handleActiveColor;


    private Image backgroundImage, handleImage;
    private Color defaultColor;
    private Color handleDefaultColor;
    private Toggle toggle;
    private Vector2 handlePosition;

    private int isSoundActive;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        isSoundActive = PlayerPrefs.GetInt("Sound");


        if (isSoundActive == 0)
            toggle.isOn = true;
        else
            toggle.isOn = false;
       

        handlePosition = uiHandleTransform.anchoredPosition;
        
        backgroundImage = uiHandleTransform.parent.GetComponent<Image>();
        handleImage = uiHandleTransform.GetComponent<Image>();

        defaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;
        
        toggle.onValueChanged.AddListener(OnSwitch);
        
           
    }

    private void OnEnable()
    {
        OnSwitch(toggle.isOn);
    }

    void OnSwitch(bool on)
    {
        if (on)
        {
            isSoundActive = 0;
            PlayerPrefs.SetInt("Sound",isSoundActive);
            backgroundImage.color = activeColor;
            handleImage.color = handleActiveColor;
            uiHandleTransform.anchoredPosition = handlePosition * -1;

            AudioListener.pause = false;
        }
        else
        {
            isSoundActive = 1;
            PlayerPrefs.SetInt("Sound",isSoundActive);
            backgroundImage.color = defaultColor;
            handleImage.color = handleDefaultColor;
         
            uiHandleTransform.anchoredPosition = handlePosition ;
            
            AudioListener.pause = true;
        }
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
