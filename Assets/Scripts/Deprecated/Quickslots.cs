using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quickslots : MonoBehaviour
{

    public GameObject menuButton;
    public List<GameObject> powerups;
    public GameObject nextPage;
    public Text pageDisplay;

    public int itemsPerPage = 5;
    public int currentPage = 0;

    private List<Passcode> loadedList;

    // Start is called before the first frame update
    void Start()
    {
        loadedList = new List<Passcode>();
        RefreshQuickslots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshQuickslots()
    {
        loadedList = Passcode.GetPasscodeList(Passcode.Status.Entered);

        int startIndex = currentPage * itemsPerPage;
        for (int i = 0; i < itemsPerPage; ++i)
        {
            if (startIndex + i < loadedList.Count)
            {
                powerups[i].SetActive(true);
                powerups[i].GetComponent<PowerupButton>().Initialize(loadedList[startIndex + i]);
            }
            else
                powerups[i].SetActive(false);
        }

        nextPage.SetActive(startIndex + itemsPerPage < loadedList.Count);

    }

    public void NextPage()
    {
        if ((currentPage + 1) * itemsPerPage < loadedList.Count)
            ++currentPage;
        else
            currentPage = 0;

        string s = (currentPage + 1).ToString() + " / " + Mathf.Ceil(loadedList.Count / (float)itemsPerPage).ToString();
        RefreshQuickslots();
    }

}
