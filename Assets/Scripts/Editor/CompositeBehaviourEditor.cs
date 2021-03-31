using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeBehaviour))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CompositeBehaviour compositeBehaviour = (CompositeBehaviour)target;

        if (compositeBehaviour.Behaviours == null || compositeBehaviour.Behaviours.Length == 0)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
            EditorGUILayout.LabelField("Behaviours", GUILayout.MinWidth(60f));
            EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < compositeBehaviour.Behaviours.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                compositeBehaviour.Behaviours[i] = (FlockBehaviour)EditorGUILayout.ObjectField(compositeBehaviour.Behaviours[i], typeof(FlockBehaviour), false, GUILayout.MinWidth(60f));
                compositeBehaviour.Weights[i] = EditorGUILayout.FloatField(compositeBehaviour.Weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(compositeBehaviour);
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Behaviour"))
        {
            AddBehaviour(compositeBehaviour);
            EditorUtility.SetDirty(compositeBehaviour);
        }

        if (compositeBehaviour.Behaviours != null && compositeBehaviour.Behaviours.Length > 0)
        {
            if (GUILayout.Button("Remove Behaviour"))
            {
                RemoveBehaviour(compositeBehaviour);
                EditorUtility.SetDirty(compositeBehaviour);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void AddBehaviour(CompositeBehaviour compositeBehaviour)
    {
        int oldCount = (compositeBehaviour.Behaviours != null) ? compositeBehaviour.Behaviours.Length : 0;
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = compositeBehaviour.Behaviours[i];
            newWeights[i] = compositeBehaviour.Weights[i];
        }
        newWeights[oldCount] = 1f;
        compositeBehaviour.Behaviours = newBehaviours;
        compositeBehaviour.Weights = newWeights;
    }

    void RemoveBehaviour(CompositeBehaviour compositeBehaviour)
    {
        int oldCount = compositeBehaviour.Behaviours.Length;
        if (oldCount == 1)
        {
            compositeBehaviour.Behaviours = null;
            compositeBehaviour.Weights = null;
            return;
        }
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviours[i] = compositeBehaviour.Behaviours[i];
            newWeights[i] = compositeBehaviour.Weights[i];
        }
        compositeBehaviour.Behaviours = newBehaviours;
        compositeBehaviour.Weights = newWeights;
    }
}
