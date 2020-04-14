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

    public void Initialize(int _index)
    {
        CodeManager.Powerup powerup = CodeManager.instance.codes[_index];

        index.text = _index.ToString();
        code.text = powerup.code;
        switch (powerup.spriteIndex)
        {
            case 0:
                index.color = Color.red;
                break;
            case 1:
                index.color = Color.yellow;
                break;
            case 2:
                index.color = new Color(255, 0, 255);
                break;
            case 3:
                index.color = Color.green;
                break;
            case 4:
                index.color = new Color(0, 153, 255);
                break;
        }

    }

}
