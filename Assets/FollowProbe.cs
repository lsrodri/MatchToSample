using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProbe : MonoBehaviour
{
    GameObject target;

    void Start()
    {
        // Get the renderer of the game object
        target = GameObject.Find("Grabber");
    }

    void Update()
    {
        transform.position = target.transform.position;
    }
}
