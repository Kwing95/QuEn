using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{

    public static int maxPowerups = 5;
    public Image icon;
    public List<Sprite> sprites;
    public Passcode powerup;

    private int index;

    // Start is called before the first frame update
    void Awake()
    {

    }

    public void Initialize(Passcode _powerup)
    {
        icon.sprite = sprites[Passcode.IconIndex(_powerup.GetPowerup())];
        powerup = _powerup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        // Add some visualizer for this

        powerup.Activate();
        //PrefabManager.instance.quickslots.RefreshQuickslots();
        //Destroy(transform.parent.gameObject);
    }

    public void AddPowerup()
    {
        
    }

    public void UsePowerup()
    {
        
    }

}
