using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTrigger : MonoBehaviour
{
    public GameObject magicSphere;
    private MagneticSphere magneticSphere;


    // Start is called before the first frame update
    void Start()
    {
        magneticSphere = magicSphere.GetComponent<MagneticSphere>();
    }

    private void OnTriggerEnter(Collider other)
    {
        magneticSphere.updateMaterial(true);
    }

    private void OnTriggerExit(Collider other)
    {
        magneticSphere.updateMaterial(false);
    }
}
