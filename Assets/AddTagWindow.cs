using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class AddTagWindow : EditorWindow
{
    private string tagName = "";

    [MenuItem("Custom/Add Tag")]
    public static void ShowWindow()
    {
        GetWindow<AddTagWindow>("Add Tag");
    }

    private void AddTagToChildrenRecursively(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            // Check if the child already has the tag
            if (!child.CompareTag("Touchable"))
            {
                // Add the tag to the child
                child.gameObject.tag = "Touchable";
            }

            // Recursively call this method on the child's children
            AddTagToChildrenRecursively(child.gameObject);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter tag name:", EditorStyles.boldLabel);
        tagName = EditorGUILayout.TextField(tagName);

        if (GUILayout.Button("Add Tag"))
        {
            // Find all the objects in the scene
            GameObject[] objects = FindObjectsOfType<GameObject>();

            // Filter out objects that already have the tag
            GameObject[] filteredObjects = objects.Where(obj => !obj.CompareTag(tagName)).ToArray();

            // Add the tag to each filtered object
            foreach (GameObject obj in filteredObjects)
            {
                AddTagToChildrenRecursively(obj);
            }

            // Save the changes to the scene
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

            // Repaint the scene view to show the changes
            SceneView.RepaintAll();

            // Close the window
            Close();
        }
    }
}
