using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PRNG : MonoBehaviour
{

    public Text text;

    public static List<string> adjectives;
    public static List<string> animals;

    // minute, hour, day, week, month, year
    public static int[] secondConversions = new int[6] { 60, 3600, 86400, 604800, 2592000, 31104000 };

    private static int nonce = 1;
    private static int rawSeed = 0;
    private static int moddedSeed = 0;
    private static string lobbyName = "";

    // Return a random int between min and max
    public static int Range(int min, int max, bool useRaw=false)
    {
        UpdateNonce(useRaw);
        return min + (int)((max - min) * UnityEngine.Random.value);
    }

    // Return a random float between min and max
    public static float Range(float min, float max, bool useRaw=false)
    {
        UpdateNonce(useRaw);
        return min + ((max - min) * UnityEngine.Random.value);
    }

    public static void ResetNonce()
    {
        nonce = 1;
    }

    // Calls InitState with current nonce, then increments nonce
    public static void UpdateNonce(bool useRaw)
    {
        if (rawSeed == 0)
            UnixSeed();

        UnityEngine.Random.InitState((useRaw ? rawSeed : moddedSeed) * nonce);
        nonce += 1;
    }

    public static void ForceSeed(int newSeed)
    {
        Debug.Log("Seed set to " + newSeed.ToString());

        rawSeed = newSeed;
        moddedSeed = newSeed;
        ResetNonce();
    }

    // Initializes the seed with current Unix time
    // granularity: Index in secondConversions array for making the seed reflect different lengths of time
    public static void UnixSeed(int granularity = 0)
    {
        int divisor = secondConversions[granularity];

        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int unixTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        rawSeed = unixTime / secondConversions[0];
        moddedSeed = rawSeed * (unixTime / divisor);
        SetLobby();
    }

    // Returns a random string representing the current seed
    public static void SetLobby()
    {
        if (adjectives == null)
            InitializeLists();

        lobbyName = (adjectives[Range(0, adjectives.Count)] + " " +
            adjectives[Range(0, adjectives.Count)] + " " +
            animals[Range(0, adjectives.Count)]).ToUpper();
    }

    public static string GetLobbyName()
    {
        return lobbyName;
    }

    private static void InitializeLists()
    {
        adjectives = FileToList(@"Assets/Resources/adjectives.txt");
        animals = FileToList(@"Assets/Resources/animals.txt");
    }

    private static List<string> FileToList(string path)
    {
        List<string> returnValue = new List<string>();
        StreamReader inStream = new StreamReader(path);

        while (!inStream.EndOfStream)
            returnValue.Add(inStream.ReadLine());

        inStream.Close();
        return returnValue;
    }
}