using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{

    public GameObject powerupButton;
    public GameObject powerupPanel;

    public Text textDisplay;
    public static GameObject instance;

    public int maxDigits = 6;

    // Start is called before the first frame update
    void Start()
    {
        instance = gameObject;
        textDisplay.text = "";
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDigit(string digit)
    {
        if(textDisplay.text.Length < maxDigits)
            textDisplay.text += digit;
        
        if (textDisplay.text.Length >= maxDigits)
        {
            int index = CodeIndex(textDisplay.text);

            if(index != -1)
            {
                AddPowerup(CodeManager.instance.codes[index]);
                CodeManager.instance.codes.RemoveAt(index);
                ToggleEnabled();
            }

        }
    }

    public int CodeIndex(string text)
    {
        int index = -1;

        for (int i = 0; i < CodeManager.instance.codes.Count; ++i)
        {
            if (CodeManager.instance.codes[i].code == text)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void RemoveDigit()
    {
        if (textDisplay.text.Length > 0)
            textDisplay.text = textDisplay.text.Substring(0, textDisplay.text.Length - 1);
    }

    public void ToggleEnabled()
    {
        textDisplay.text = "";
        instance.SetActive(!instance.activeSelf);
    }

    public void AddPowerup(CodeManager.Powerup powerup)
    {
        GameObject newPowerup = Instantiate(powerupButton, Vector3.zero, Quaternion.identity);
        newPowerup.GetComponentInChildren<PowerupButton>().Initialize(powerup);
        newPowerup.transform.parent = powerupPanel.transform;
    }

}
