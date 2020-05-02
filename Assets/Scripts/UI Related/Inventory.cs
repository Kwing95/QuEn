using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public int linesPerPage = 5;
    private int currentPage = 0;
    private List<Passcode> loadedList;
    private bool filterUsed = false;
    public Text listDisplay;
    public Text usedLabel;

    // Start is called before the first frame update
    void Awake()
    {
        loadedList = new List<Passcode>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        loadedList = Passcode.GetPasscodeList(GetFilter(filterUsed));

        string newText = "";
        int startIndex = currentPage * linesPerPage;

        for (int i = startIndex; i < startIndex + linesPerPage; ++i)
            if(i >= loadedList.Count)
                newText += "           \n";
            else
                newText = newText + "<color='#" + ColorUtility.ToHtmlStringRGB(Passcode.GetColorByType(loadedList[i])) +
                    "'>[" + loadedList[i].GetIndex().ToString() + "]</color>[" + loadedList[i].GetCode() + "]\n";

        listDisplay.text = newText;
    }

    public void ToggleUsed()
    {
        filterUsed = !filterUsed;
        usedLabel.text = filterUsed ? "USED" : "NEW";
        currentPage = 0;
        // filter = Passcode.Status.Made; // test
        loadedList = Passcode.GetPasscodeList(GetFilter(filterUsed));
        RefreshList();
    }

    public List<Passcode.Status> GetFilter(bool used)
    {
        if(used)
            return new List<Passcode.Status> { Passcode.Status.Entered, Passcode.Status.Used };
        else
            return new List<Passcode.Status> { Passcode.Status.Revealed };
    }

    public void PreviousPage()
    {
        currentPage = Mathf.Max(0, currentPage - 1);
        RefreshList();
    }

    public void NextPage()
    {
        if ((currentPage + 1) * linesPerPage < loadedList.Count)
            ++currentPage;
        RefreshList();
    }

}
