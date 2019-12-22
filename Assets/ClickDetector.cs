using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickDetector : MonoBehaviour
{

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        //button.OnPointerDown(OnPress);
        //button.OnPointerDown(OnPress);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPress()
    {
        PlayerMover.instance.OnPress();
    }

    private void OnRelease()
    {
        PlayerMover.instance.OnRelease();
    }

}
