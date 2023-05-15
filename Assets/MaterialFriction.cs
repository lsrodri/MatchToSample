using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFriction : MonoBehaviour
{

    HapticPlugin device = null;

    // Start is called before the first frame update
    void Start()
    {
        device = (HapticPlugin)Object.FindObjectOfType(typeof(HapticPlugin));
        foreach (Transform child in transform)
        {
            Debug.Log(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        
    }

}
