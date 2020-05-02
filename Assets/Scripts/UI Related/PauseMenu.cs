using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject powerupButton;
    public List<GameObject> powerupButtons;

    public GameObject notification;
    public Text inventory;

    public static GameObject instance;

    public int maxDigits = 6;

    // Start is called before the first frame update
    void Start()
    {
        instance = gameObject;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void AddDigit(string digit)
    {
        if(textDisplay.text.Length < maxDigits)
            textDisplay.text += digit;
        
        if (textDisplay.text.Length >= maxDigits)
            EnterCode();
    }

    
    private void EnterCode()
    {
        int index = CodeIndex(textDisplay.text);

        if (index != -1)
        {
            // If code is tied to location, clear room
            Point location = CodeManager.instance.codes[index].location;
            if (location.x != -1)
                RoomArranger.floor[location.y, location.x].Clear();
            if (location.x == Room.location.x && location.y == Room.location.y)
                MobMaker.KillAllEnemies();
            
            AddPowerup(CodeManager.instance.codes[index]);
            CodeManager.instance.codes.RemoveAt(index);
            ToggleEnabled();
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
    }*/

    public void ToggleEnabled()
    {
        instance.SetActive(!gameObject.activeSelf);
        foreach (GameObject powerupButton in powerupButtons)
            powerupButton.SetActive(!gameObject.activeSelf);
    }

    public void RefreshNotification()
    {
        List<Passcode> results = Passcode.GetPasscodeList(Passcode.Status.Revealed);
        notification.SetActive(results.Count > 0);
    }

    /*
    public void AddPowerup(Passcode.Powerup powerup)
    {
        GameObject newPowerup = Instantiate(powerupButton, Vector3.zero, Quaternion.identity);
        newPowerup.GetComponentInChildren<PowerupButton>().Initialize(powerup);
        newPowerup.transform.parent = powerupPanel.transform;
    }*/

}
