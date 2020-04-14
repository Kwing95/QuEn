using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{

    public static int maxPowerups = 5;
    public Image icon;
    public List<Sprite> sprites;
    public CodeManager.Powerup powerup;

    public static List<GameObject> powerups;
    private int index;

    // Start is called before the first frame update
    void Awake()
    {
        if (powerups == null)
            powerups = new List<GameObject>();

        if (powerups.Count >= maxPowerups)
        {
            Destroy(gameObject);
            return;
        }

        index = powerups.Count;
        powerups.Add(gameObject);
    }

    private void OnDestroy()
    {

        for (int i = index + 1; i < powerups.Count; ++i)
        {
            powerups[i].GetComponent<PowerupButton>().index -= 1;
            //powerups[i].transform.position = powerups[i].transform.position + (100 * Vector3.left);
        }

        powerups.RemoveAt(index);
    }

    public void Initialize(CodeManager.Powerup _powerup)
    {
        icon.sprite = sprites[_powerup.spriteIndex];
        powerup = _powerup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("Activated powerup!");
        // Add some visualizer for this

        powerup.Activate();
        Destroy(transform.parent.gameObject);
    }

}
