#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MaterialRemapper : ScriptableObject
{
    [MenuItem("Custom Tools/Remap Materials")]
    static void RemapMaterials()
    {
        Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Cube.mat"); // Modify path as required
        if (newMaterial == null)
        {
            Debug.LogError("New material not found. Check the path.");
            return;
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "default") // Check if the game object is named "default"
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material[] materials = renderer.sharedMaterials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].name.StartsWith("PathFace")) // Check if the material is named "PathFace"
                        {
                            materials[i] = newMaterial; // Assign the new material
                        }
                    }

                    renderer.sharedMaterials = materials; // Apply the changes
                }
                else
                {
                    Debug.LogWarning($"GameObject {obj.name} does not have a Renderer component.");
                }
            }
        }
    }
}
#endif