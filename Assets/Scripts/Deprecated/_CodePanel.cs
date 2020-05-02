using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{

    public Text index;
    public Text code;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetCode()
    {
        return code.text;
    }

    public void Initialize(Passcode passcode)
    {
        Passcode.Powerup powerup = passcode.GetPowerup();

        index.text = passcode.GetIndex().ToString();
        code.text = passcode.GetCode();
        index.color = Passcode.GetColorByType(passcode);

    }

}
