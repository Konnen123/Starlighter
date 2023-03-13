using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipText : MonoBehaviour
{
    public static TipText Instance { get; private set; }

    private TextMeshProUGUI tipText;
    private void Awake()
    {
        tipText = GetComponentInChildren<TextMeshProUGUI>();
        Instance = this;
    
    }

    public void SetText(string text)
    {
        tipText.text = text;
    }
    public void SetColor(Color color)
    {
        tipText.color = color;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
