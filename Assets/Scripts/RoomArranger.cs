using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public int x;
    public int y;

    public Point(int xIn, int yIn)
    {
        x = xIn;
        y = yIn;
    }
}

public class Room
{
    public enum Status { Hidden, Seen, Visited, Current }
    // public RoomLayout;
    // public EnemyLayout;
    public Status status = Status.Hidden;
    private bool cleared = true;
    private bool valid = false;
    public int probability = 0;
    public static Point location;
    public const int ROOM_OFFSET = 28;
    public ChunkData[,] chunks = new ChunkData[3, 3];

    public void Activate()
    {
        valid = true;
    }

    public static void ChangeRooms(Door.Direction direction)
    {
        switch (direction)
        {
            case Door.Direction.Up:
                PlayerMover.instance.transform.position = (Vector2)PlayerMover.instance.transform.position + (Vector2.down * ROOM_OFFSET);
                RoomArranger.instance.LoadRoom(0, -1);
                break;
            case Door.Direction.Down:
                PlayerMover.instance.transform.position = (Vector2)PlayerMover.instance.transform.position + (Vector2.up * ROOM_OFFSET);
                RoomArranger.instance.LoadRoom(0, 1);
                break;
            case Door.Direction.Right:
                PlayerMover.instance.transform.position = (Vector2)PlayerMover.instance.transform.position + (Vector2.left * ROOM_OFFSET);
                RoomArranger.instance.LoadRoom(1, 0);
                break;
            case Door.Direction.Left:
                PlayerMover.instance.transform.position = (Vector2)PlayerMover.instance.transform.position + (Vector2.right * ROOM_OFFSET);
                RoomArranger.instance.LoadRoom(-1, 0);
                break;
        }
    }

    public bool IsValid()
    {
        return valid;
    }

    public bool IsCleared()
    {
        return cleared;
    }
}

public class RoomArranger : MonoBehaviour
{

    public const int MAX_DIMENSION = 8;
    // Relative likelihood of adding a neighbor given number of neighbors already
    private int[] probs = new int[] { 0, 10000, -2, -2, -2 };
    public static Room[,] floor = new Room[MAX_DIMENSION, MAX_DIMENSION];
    public Door rightDoor;
    public Door leftDoor;
    public Door upDoor;
    public Door downDoor;
    public GameObject vortex;
    public GameObject roomObjects;
    public static RoomArranger instance;

    // vortexes: 60% chance of 0; 25% chance of 1, 10% chance of 2, 5% chance of 3
    // bomb placement: 
    // wall placement (some block bullets, some don't?)

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Loads room adjacent to player (x + dx, y + dy)
    public void LoadRoom(int dx, int dy)
    {
        SetStatus(Room.location.x, Room.location.y, Room.Status.Visited);
        Room.location.x += dx;
        Room.location.y += dy;
        SetStatus(Room.location.x, Room.location.y, Room.Status.Current);
        // PrintFloor();

        ClearRoom();
        CreateChunks(floor[Room.location.y, Room.location.x].chunks);
        ConfigureDoors();
    }

    // Sets the color of UI map tiles
    private void SetStatus(int x, int y, Room.Status status)
    {
        floor[y, x].status = status;
        Color newColor = new Color();
        switch (status)
        {
            case Room.Status.Hidden:
                newColor = Color.clear;
                break;
            case Room.Status.Seen:
                newColor = Color.gray;
                break;
            case Room.Status.Visited:
                newColor = Color.white;
                break;
            case Room.Status.Current:
                newColor = Color.red;
                break;
        }

        UIMap.map[MAX_DIMENSION - 1 - y, MAX_DIMENSION - 1 - x].color = newColor;
    }

    // Calls ConfigureDoor for all directions
    private void ConfigureDoors()
    {
        ConfigureDoor(rightDoor, new Point(Room.location.x + 1, Room.location.y));
        ConfigureDoor(leftDoor, new Point(Room.location.x - 1, Room.location.y));
        ConfigureDoor(upDoor, new Point(Room.location.x, Room.location.y - 1));
        ConfigureDoor(downDoor, new Point(Room.location.x, Room.location.y + 1));
    }

    // Determines whether each door should be locked, blocked, or open
    private void ConfigureDoor(Door door, Point point)
    {
        if(PointInBounds(point) && floor[point.y, point.x].IsValid())
        {
            if(floor[point.y, point.x].status == Room.Status.Hidden)
                SetStatus(point.x, point.y, Room.Status.Seen);
            door.SetStatus(floor[point.y, point.x].IsCleared() ? Door.Status.Open : Door.Status.Locked);
        }
        else
            door.SetStatus(Door.Status.Blocked);
    }

    // Prints ASCII output of floor
    public void PrintFloor()
    {
        string output = "";
        for(int i = 0; i < MAX_DIMENSION; ++i)
        {
            string line = "";
            for(int j = 0; j < MAX_DIMENSION; ++j)
            {
                if (Room.location.x == j && Room.location.y == i)
                    line += "8";
                else
                    line += floor[i, j].IsValid() ? "1" : "0" /*floor[i, j].probability.ToString()*/;
            }
            output += line + "\n";
        }
        Debug.Log(output);
    }

    // Adds a room in the center of a floor and fills it with numRooms rooms
    public void GenerateFloor(int numRooms)
    {
        CreateEmptyFloor();
        floor[MAX_DIMENSION / 2, MAX_DIMENSION / 2] = new Room();
        Room.location = new Point(MAX_DIMENSION / 2, MAX_DIMENSION / 2);
        SetStatus(Room.location.x, Room.location.y, Room.Status.Current);

        AddRoom(new Point(MAX_DIMENSION / 2, MAX_DIMENSION / 2), true);
        for(int i = 0; i < numRooms; ++i)
        {
            Point tile = SelectRoom();
            AddRoom(tile);
            // PrintFloor(); // for step-by-step analysis
        }
        LoadRoom(0, 0);
        // ConfigureDoors();
    }

    // Randomly selects a room based on existing rooms
    private Point SelectRoom()
    {
        List<Point> candidates = new List<Point>();
        int total = 0;

        for (int i = 0; i < MAX_DIMENSION; ++i)
            for (int j = 0; j < MAX_DIMENSION; ++j)
                if (floor[i, j].probability > 0)
                {
                    candidates.Add(new Point(j, i));
                    total += floor[i, j].probability;
                }

        int selection = PRNG.Range(0, total);
        int currentCandidate = 0;

        while(selection > 0)
        {
            selection -= floor[candidates[currentCandidate].y, candidates[currentCandidate].x].probability;
            currentCandidate += 1;
        }

        currentCandidate = currentCandidate % candidates.Count;
        return new Point(candidates[currentCandidate].x, candidates[currentCandidate].y);
    }

    // Update the probabilities of all points neighboring (x, y)
    private void AddRoom(Point point, bool isStart=false)
    {
        // TODO: Initialize enemy and room layout

        // Dummy value; can no longer add a room here because it's taken
        floor[point.y, point.x].Activate();
        floor[point.y, point.x].probability = -1;
        
        floor[point.y, point.x].chunks = isStart ? EmptyChunks() : RandomChunks();

        AdvanceProbability(new Point(point.x + 1, point.y));
        AdvanceProbability(new Point(point.x - 1, point.y));
        AdvanceProbability(new Point(point.x, point.y - 1));
        AdvanceProbability(new Point(point.x, point.y + 1));
    }

    // Returns a 3x3 array of randomized chunks
    private ChunkData[,] RandomChunks()
    {
        ChunkData[,] chunks = new ChunkData[3, 3];
        for(int i = 0; i < 3; ++i)
            for(int j = 0; j < 3; ++j)
            {
                // TODO: Customize probabilities of different chunk properties
                bool canBeVortex = Mathf.Abs(i) == Mathf.Abs(j);
                chunks[i, j] = new ChunkData(PRNG.Range(0, 4), PRNG.Range(0, 4), PRNG.Range(0, 4),
                    PRNG.Range(0, 2) == 0 && canBeVortex, PRNG.Range(0, 4));
            }
        return chunks;
    }

    // Returns an array of empty chunks
    private ChunkData[,] EmptyChunks()
    {
        ChunkData[,] chunks = new ChunkData[3, 3];
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                chunks[i, j] = new ChunkData(0, 0, 0, false, 0);
        return chunks;
    }

    // Advance the probability to reflect an additional neighbor to a space
    // Does nothing if point is out of bounds or point is occupied
    private void AdvanceProbability(Point point)
    {
        if (!PointInBounds(point))
            return;
        
        int oldProbability = floor[point.y, point.x].probability;
        if (oldProbability > -1)
            floor[point.y, point.x].probability = probs[Array.IndexOf(probs, oldProbability) + 1];
    }

    // Return true if and only if (x, y) is in bounds
    private bool PointInBounds(Point point)
    {
        return (point.x >= 0 && point.y >= 0 && point.x < MAX_DIMENSION && point.y < MAX_DIMENSION);
    }

    // Creates a floor of null Rooms
    private void CreateEmptyFloor()
    {
        for(int i = 0; i < MAX_DIMENSION; ++i)
            for(int j = 0; j < MAX_DIMENSION; ++j)
                floor[i, j] = new Room();
    }

    // Remove all chunks
    private void ClearRoom()
    {
        foreach (Transform child in roomObjects.transform)
            Destroy(child.gameObject);
    }

    // Instantiate a 3x3 array of chunks
    private void CreateChunks(ChunkData[,] chunks)
    {
        for(int i = -1; i < 2; ++i)
            for(int j = -1; j < 2; ++j)
                CreateChunk(new Vector2(-8 * i, -8 * j), chunks[i + 1, j + 1]);
    }

    // Instantiate a chunk and configure its properties
    private void CreateChunk(Vector2 position, ChunkData chunk)
    {
        Debug.Log(chunk.isVortex);
        if (chunk.isVortex)
            Instantiate(vortex, position, Quaternion.identity, roomObjects.transform);
        else
        {
            if (chunk.enemy != 0)
            {
                
            }
            GameObject newChunk = Instantiate(PrefabManager.instance.chunkShapes[chunk.shape],
                position, Quaternion.identity, roomObjects.transform);

            newChunk.GetComponent<Chunk>().Initialize(chunk);
        }
    }

}
