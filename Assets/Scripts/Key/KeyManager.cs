using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;
    [SerializeField] private TextMeshProUGUI keyCounterUI;

    private int keyCount;
    private void Awake()
    {
        Instance = this;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    public void UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            keyCounterUI.text = keyCount.ToString();
        }
    }

    public void AddKey()
    {
        keyCount++;
        keyCounterUI.text = keyCount.ToString();
    }
}