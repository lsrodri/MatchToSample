using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SpawnCubes : MonoBehaviour
{

    public GameObject cubePrefab; // Assign the original cube prefab in the inspector
    public GameObject pathCubePrefab; // Assign the path cube prefab in the inspector
    public GameObject Haptics;
    public GameObject ProbeProxy;
    public GameObject spotlightFollower;



    private float cubeHeight;
    private bool[,] visited; // A 2D array to store whether each cube has been visited
    private int pathLength; // A counter to keep track of the path length
    private Transform grabber;
    private bool spotlightEnabled;


    void Start()
    {
        // Use a nested loop to create a 5x5 matrix of cubes
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                // Instantiate a new cube
                var cube = Instantiate(cubePrefab);

                // Set the position and scale of the cube based on the loop indices
                cube.transform.position = new Vector3(x, 0, y);
                cube.transform.localScale = Vector3.one;

                // Set the parent of the cube to the parent GameObject
                cube.transform.SetParent(transform);

                // Add a MeshRenderer component to the cube and set the material
                var meshRenderer = cube.AddComponent<MeshRenderer>();
             
            }
        }

        // Initialize the visited array and set all values to false
        visited = new bool[5, 5];
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                visited[x, y] = false;
            }
        }

        // Initialize the path length counter
        pathLength = 0;

        // Call the recursive function to generate the path
        GeneratePath(0, 0);

        //StartCoroutine(wakeUpHaptics());
    }

    // A recursive function to generate the path
    void GeneratePath(int x, int y)
    {
        // Check if the path has reached the maximum length
        if (pathLength >= 6)
        {
            transform.position = new Vector3(-4.569737f, -2.991227f, -1.190004f);
            transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            
            return;
        }

        // Check if the current position has 2 or more neighboring cubes that have already been visited
        // This is to prevent the path accidentally forming non-continuous path, like 2 cubes on top of another 2
        int visitedNeighbors = 0;
        if (x > 0 && visited[x - 1, y]) visitedNeighbors++;
        if (x < 4 && visited[x + 1, y]) visitedNeighbors++;
        if (y > 0 && visited[x, y - 1]) visitedNeighbors++;
        if (y < 4 && visited[x, y + 1]) visitedNeighbors++;
        if (visitedNeighbors >= 2)
        {
            return;
        }

        /*
        // Check if the current position is part of a straight line of 3 or more cubes that have already been visited
        bool straightLine = false;
        if (x > 1 && visited[x - 1, y] && visited[x - 2, y]) straightLine = true;
        if (x < 3 && visited[x + 1, y] && visited[x + 2, y]) straightLine = true;
        if (y > 1 && visited[x, y - 1] && visited[x, y - 2]) straightLine = true;
        if (y < 3 && visited[x, y + 1] && visited[x, y + 2]) straightLine = true;
        if (straightLine)
        {
            return;
        }
        */

        // Check if the current position is part of a straight line of 4 or more cubes that have already been visited
        bool straightLine = false;
        if (x > 2 && visited[x - 1, y] && visited[x - 2, y] && visited[x - 3, y]) straightLine = true;
        if (x < 2 && visited[x + 1, y] && visited[x + 2, y] && visited[x + 3, y]) straightLine = true;
        if (y > 2 && visited[x, y - 1] && visited[x, y - 2] && visited[x, y - 3]) straightLine = true;
        if (y < 2 && visited[x, y + 1] && visited[x, y + 2] && visited[x, y + 3]) straightLine = true;
        if (straightLine)
        {
            return;
        }

        // Mark the current cube as visited
        visited[x, y] = true;


        cubeHeight = cubePrefab.transform.localScale.y;

        //Instantiate a new path cube prefab at the position of the current cube
        var pathCube = Instantiate(pathCubePrefab,
           new Vector3(transform.GetChild(x * 5 + y).position.x, transform.GetChild(x * 5 + y).position.y - cubeHeight / 2, transform.GetChild(x * 5 + y).position.z),
           Quaternion.identity);

        // Set the parent of the path cube to the container game object
        pathCube.transform.SetParent(transform);

        // Destroy the original cube
        Destroy(transform.GetChild(x * 5 + y).gameObject);

        // Increment the path length counter
        pathLength++;

        // Generate a random order of directions to try
        var directions = new List<Vector2Int> {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
          }.OrderBy(d => Guid.NewGuid()).ToList();

        // Try each direction in the random order
        foreach (var direction in directions)
        {
          
            // Calculate the new position
            var newX = x + direction.x;
            var newY = y + direction.y;

            // Check if the new position is within the bounds of the matrix and has not been visited
            if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5 && !visited[newX, newY])
            {
                // Call the recursive function to generate the path from the new position
                GeneratePath(newX, newY);
            }
        }

    }

    void Update()
    {
        if (spotlightEnabled)
        {
            spotlightFollower.transform.position = new Vector3(grabber.transform.position.x, grabber.transform.position.y + 2.642466f, grabber.transform.position.z);
        }
    }

    IEnumerator wakeUpHaptics()
    {

        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(1);
        //Instantiate(Haptics);
        // Instantiate a new prefab as a sibling of the current game object
        var prefabInstance = Instantiate(Haptics, new Vector3(0f, 0f, 0f), transform.rotation, transform.parent);

        grabber = prefabInstance.transform.Find("Grabber");
        grabber.transform.localScale = new Vector3(1f, 1f, 1f);

        if(ProbeProxy)
        {
            Instantiate(ProbeProxy);
        } 
        else if (spotlightFollower)
        {
            spotlightEnabled = true;
            
        }
        
    }
}

