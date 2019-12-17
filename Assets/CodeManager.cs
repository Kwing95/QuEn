using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeManager : MonoBehaviour
{

    private int unixTime;
    private int nonce = 0;
    private List<string> codes;

    // Start is called before the first frame update
    void Start()
    {        
        UpdateTime();
        GenerateCodes(10);
        PrintCodes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCodes(int numCodes)
    {
        codes = new List<string>();

        Debug.Log("seed = " + unixTime.ToString());
        Random.InitState(unixTime);

        for (int i = 0; i < numCodes; ++i)
            codes.Add(Mathf.RoundToInt(999999 * Random.value).ToString().PadLeft(6, '0'));
    }

    public void PrintCodes()
    {
        foreach(string elt in codes)
            Debug.Log(elt);
    }

    public void UpdateNonce()
    {
        Random.InitState(unixTime);

    }

    // Returns current time, rounded to the nearest minute
    public void UpdateTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        unixTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds / 60;
    }
}
