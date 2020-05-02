using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrefabs : MonoBehaviour
{

    public static MenuPrefabs instance;

    public Text bloomText;
    public Text soundText;
    public Text musicText;
    public Text zoomText;
    public Text saltText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
