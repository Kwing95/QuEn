using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{

    public Text textDisplay;
    public static int MAX_DIGITS = 6;
    public GameObject powerupPanel;
    public GameObject powerupButton;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay.text = "";
    }

    public void ToggleEnabled()
    {
        ClearDigits();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void AddDigit(string digit)
    {
        if (textDisplay.text.Length < MAX_DIGITS)
            textDisplay.text += digit;

        if (textDisplay.text.Length >= MAX_DIGITS)
            EnterCode();
    }

    public void RemoveDigit()
    {
        if (textDisplay.text.Length > 0)
            textDisplay.text = textDisplay.text.Substring(0, textDisplay.text.Length - 1);
    }

    public void ClearDigits()
    {
        textDisplay.text = "";
    }

    private void EnterCode()
    {
        Passcode powerup = Passcode.GetPowerupByCode(textDisplay.text);
        if(powerup != null)
        {
            ClearDigits();
            RedeemCode(powerup);
            // Debug.Log("Successfully redeemed " + powerup.GetCode());
        }
            
    }

    public void RedeemCode(Passcode powerup)
    {
        PrefabManager.instance.inventory.RefreshList();
        //PrefabManager.instance.quickslots.RefreshQuickslots();
        //GameObject newPowerup = Instantiate(powerupButton, Vector3.zero, Quaternion.identity);
        //newPowerup.GetComponentInChildren<PowerupButton>().Initialize(powerup);
        //newPowerup.transform.SetParent(powerupPanel.transform);
    }

}
