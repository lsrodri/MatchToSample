using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglemask : MonoBehaviour
{
    public GameObject mask;
    public GameObject sphere;
    public GameObject cylinder;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == sphere)
        {
            mask.GetComponent<Renderer>().enabled = true;
            sphere.GetComponent<Renderer>().enabled = false;
            cylinder.GetComponent<Renderer>().enabled = false;
        }
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == sphere)
        { 
            mask.GetComponent<Renderer>().enabled = false;
            sphere.GetComponent<Renderer>().enabled = true;
            cylinder.GetComponent<Renderer>().enabled = true;
        }        
    }
}
