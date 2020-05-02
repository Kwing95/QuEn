using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour
{

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if(text != null)
            RefreshLobby();
    }

    public void RefreshLobby()
    {
        if(text != null)
        {
            PRNG.ResetNonce();
            PRNG.UnixSeed();
            text.text = PRNG.GetLobbyName();
        }
    }

    public void EnableObject(GameObject go)
    {
        go.SetActive(true);
    }

    public void DisableObject(GameObject go)
    {
        go.SetActive(false);
    }

    public void SceneChange(string scene)
    {
        Fader.instance.FadeTransition(() => SceneManager.LoadScene(scene));
    }

    public void StartCustomGame()
    {
        int newSeed = 0;
        try
        {
            newSeed = Convert.ToInt32(text.text);
        }
        catch (Exception)
        {
            newSeed = 0;
        }
        PRNG.ForceSeed(newSeed);
        Fader.instance.FadeTransition(() => SceneManager.LoadScene("StandardGame"));
    }
    
}
