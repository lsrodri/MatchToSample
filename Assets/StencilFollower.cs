using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilFollower : MonoBehaviour
{

    public GameObject grabber;
    // Start is called before the first frame update
    public float multiplier = 1f;
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(grabber.transform.localPosition.x / multiplier, transform.position.y, grabber.transform.localPosition.z / multiplier);
    }


}
