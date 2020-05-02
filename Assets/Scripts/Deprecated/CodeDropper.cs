using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeDropper : MonoBehaviour
{

    public GameObject codePanel;
    public Passcode.Powerup powerupType;
    public float potency;

    private Passcode powerup;

    private void OnDestroy()
    {
        //GameObject codeHolder = PauseMenu.instance.GetComponent<PauseMenu>().unusedCodes;
        //GameObject newCode = Instantiate(codePanel, codeHolder.transform.position, Quaternion.identity);
        //newCode.transform.SetParent(codeHolder.transform);
        //newCode.GetComponent<CodePanel>().Initialize(powerup);
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (powerupType)
        {
            case Passcode.Powerup.Damage:
            case Passcode.Powerup.Stamina:
            case Passcode.Powerup.Defense:
            case Passcode.Powerup.Heal:
                // powerup = new Passcode.Buff(powerupType, potency, new Point(-1, -1));
                break;
            case Passcode.Powerup.Kill:
                powerup = new Passcode(Passcode.Powerup.Kill, new Point(-1, -1), gameObject);
                // powerup = new Passcode.Kill(GetComponent<UnitStatus>());
                break;
        }
        // Passcode.Powerup.codes.Add(powerup); // passcode does this automatically in constructor
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
