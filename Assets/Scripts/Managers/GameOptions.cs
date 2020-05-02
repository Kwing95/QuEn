using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{

    public static GameOptions instance;

    public static float scalingFactor = 1;

    public static bool useSalt = false;
    public static bool useBloom = true;
    public static int soundVolume = 5;
    public static int musicVolume = 5;
    public static int cameraZoom = 5;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        scalingFactor = Screen.height / 500f;

        if (MenuPrefabs.instance != null)
            RefreshOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshOptions()
    {
        MenuPrefabs.instance.zoomText.text = "ZOOM\n(" + cameraZoom.ToString() + ")";
        MenuPrefabs.instance.soundText.text = "SOUND\n(" + soundVolume.ToString() + ")";
        MenuPrefabs.instance.musicText.text = "MUSIC\n(" + musicVolume.ToString() + ")";
        MenuPrefabs.instance.bloomText.text = "BLOOM\n(" + (useBloom ? "ON" : "OFF") + ")";
        MenuPrefabs.instance.saltText.text = "SALT\n(" + (useSalt ? "ON" : "OFF") + ")";
    }

    public void AdjustZoom()
    {
        cameraZoom = (cameraZoom + 1) % 10;
        RefreshOptions();
    }

    public void AdjustSound()
    {
        soundVolume = (soundVolume + 1) % 10;
        RefreshOptions();
    }

    public void AdjustMusic()
    {
        musicVolume = (musicVolume + 1) % 10;
        RefreshOptions();
    }

    public void ToggleBloom()
    {
        useBloom = !useBloom;
        RefreshOptions();
    }

    public void ToggleSalt()
    {
        useSalt = !useSalt;
        RefreshOptions();
    }

}
