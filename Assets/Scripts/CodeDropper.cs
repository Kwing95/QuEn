using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeDropper : MonoBehaviour
{

    public GameObject codePanel;
    public CodeManager.Type powerupType;
    public float potency;

    private CodeManager.Powerup powerup;

    private void OnDestroy()
    {
        GameObject codeHolder = PauseMenu.instance.GetComponent<PauseMenu>().unusedCodes;
        GameObject newCode = Instantiate(codePanel, codeHolder.transform.position, Quaternion.identity);
        newCode.transform.SetParent(codeHolder.transform);
        newCode.GetComponent<CodePanel>().Initialize(powerup.index);
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (powerupType)
        {
            case CodeManager.Type.Damage:
            case CodeManager.Type.Stamina:
            case CodeManager.Type.Defense:
            case CodeManager.Type.Heal:
                powerup = new CodeManager.Buff(powerupType, potency);
                break;
            case CodeManager.Type.Kill:
                powerup = new CodeManager.Kill(GetComponent<UnitStatus>());
                break;
        }
        CodeManager.instance.codes.Add(powerup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
