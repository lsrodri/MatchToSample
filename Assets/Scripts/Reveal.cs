//Shady
using UnityEngine;

[ExecuteInEditMode]
public class Reveal : MonoBehaviour
{
    [SerializeField] Material Mat;
    [SerializeField] Material MatOffpath;
    [SerializeField] Light SpotLight;
	
	void Update ()
    {
        Mat.SetVector("MyLightPosition",  SpotLight.transform.position);
        Mat.SetVector("MyLightDirection", -SpotLight.transform.forward);
        Mat.SetFloat ("MyLightAngle", SpotLight.spotAngle);

        MatOffpath.SetVector("MyLightPosition", SpotLight.transform.position);
        MatOffpath.SetVector("MyLightDirection", -SpotLight.transform.forward);
        MatOffpath.SetFloat("MyLightAngle", SpotLight.spotAngle);
    }//Update() end
}//class end