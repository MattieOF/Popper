using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    // Target properties and objects
    private SerializedProperty bounds;
    private SerializedProperty objects;
    private ObjectSpawner _objectSpawner;
    
    // UI state
    private bool _percentageDrawer = false;
    
    private void OnEnable()
    {
        // Find properties and objects
        _objectSpawner = (ObjectSpawner)target;
        bounds = serializedObject.FindProperty("bounds");
        objects = serializedObject.FindProperty("objects");
    }

    public override void OnInspectorGUI()
    {
        // Draw UI here
        // Objects array
        EditorGUILayout.PropertyField(objects);
        
        // Weight UI
        _objectSpawner.CalculateTotalWeight();
        GUILayout.Label($"Total weight: {_objectSpawner.totalWeight}");

        // Percentage UI
        _percentageDrawer = EditorGUILayout.Foldout(_percentageDrawer, "Object Percentages");
        if (_percentageDrawer)
        {
            for (int i = 0; i < _objectSpawner.objects.Count; i++)
            {
                var obj = _objectSpawner.objects[i];
                GUILayout.Label($"Object {i}: {(obj.weight / _objectSpawner.totalWeight) * 100:0.00}%");
            }
        }
        
        // Bound vector
        EditorGUILayout.PropertyField(bounds);

        // Finally, apply modifications
        serializedObject.ApplyModifiedProperties();
    }
}
