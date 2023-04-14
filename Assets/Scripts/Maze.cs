using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Maze : MonoBehaviour
{

    public GameObject cubePrefab; // Assign the original cube prefab in the inspector
    public GameObject pathCubePrefab; // Assign the path cube prefab in the inspector

    public GameObject startEndPrefab;
    public GameObject throughPrefab;
    public GameObject cornerPrefab;

    private List<GameObject> path = new List<GameObject>();

    private float cubeHeight;
    private bool[,] visited; // A 2D array to store whether each cube has been visited
    private int pathLength; // A counter to keep track of the path length

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



        ReplacePathCubes();

    }

    public void ReplacePathCubes()
    {
        GameObject replacer;

        for (int i = 0; i < path.Count; i++)
        {
            var currentCube = path[i];
            var currentPosition = currentCube.transform.position;

            if (i == 0 || i == path.Count - 1)
            {
                replacer = Instantiate(startEndPrefab, currentPosition, Quaternion.identity);
            }
            else
            {
                var previousCube = path[i - 1];
                var previousPosition = previousCube.transform.position;

                var nextCube = path[i + 1];
                var nextPosition = nextCube.transform.position;

                // Check if the current cube is on the same row as the previous and next cubes
                if (Mathf.Approximately(currentPosition.y, previousPosition.y) && Mathf.Approximately(currentPosition.y, nextPosition.y))
                {
                    replacer = Instantiate(throughPrefab, currentPosition, Quaternion.identity);
                }
                // Check if the current cube is on the same column as the previous and next cubes
                else if (Mathf.Approximately(currentPosition.x, previousPosition.x) && Mathf.Approximately(currentPosition.x, nextPosition.x))
                {
                    replacer = Instantiate(throughPrefab, currentPosition, Quaternion.Euler(0f, 90f, 0f));
                }
                else
                {
                    // The current cube is a corner
                    var angle = Vector3.SignedAngle(Vector3.right, nextPosition - previousPosition, Vector3.up);
                    replacer = Instantiate(cornerPrefab, currentPosition, Quaternion.Euler(0f, angle, 0f));
                }
            }

        }

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

        path.Add(pathCube);

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
            var newY = y + direction.y;
            var newX = x + direction.x;

            // Check if the new position is within the bounds of the matrix and has not been visited
            if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5 && !visited[newX, newY])
            {
                // Call the recursive function to generate the path from the new position
                GeneratePath(newX, newY);
            }
        }

    }

}

