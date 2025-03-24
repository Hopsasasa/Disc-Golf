using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataPersistenceManager))]
public class customInspectorDataPersistence : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataPersistenceManager script = (DataPersistenceManager)target;

        if (GUILayout.Button("Delete Saved Data"))
        {
            script.DeleteSave();
        }

    }

}
