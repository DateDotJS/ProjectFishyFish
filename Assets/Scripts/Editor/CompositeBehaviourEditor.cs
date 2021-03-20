using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeBehaviour))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //setup
        CompositeBehaviour cb = (CompositeBehaviour)target;

        //check for behaviors
        if (cb.Behaviours == null || cb.Behaviours.Length == 0)
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
            for (int i = 0; i < cb.Behaviours.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                cb.Behaviours[i] = (FlockBehaviour)EditorGUILayout.ObjectField(cb.Behaviours[i], typeof(FlockBehaviour), false, GUILayout.MinWidth(60f));
                cb.Weights[i] = EditorGUILayout.FloatField(cb.Weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Behaviour"))
        {
            AddBehaviour(cb);
            EditorUtility.SetDirty(cb);
        }

        if (cb.Behaviours != null && cb.Behaviours.Length > 0)
        {
            if (GUILayout.Button("Remove Behaviour"))
            {
                RemoveBehaviour(cb);
                EditorUtility.SetDirty(cb);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void AddBehaviour(CompositeBehaviour cb)
    {
        int oldCount = (cb.Behaviours != null) ? cb.Behaviours.Length : 0;
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = cb.Behaviours[i];
            newWeights[i] = cb.Weights[i];
        }
        newWeights[oldCount] = 1f;
        cb.Behaviours = newBehaviours;
        cb.Weights = newWeights;
    }

    void RemoveBehaviour(CompositeBehaviour cb)
    {
        int oldCount = cb.Behaviours.Length;
        if (oldCount == 1)
        {
            cb.Behaviours = null;
            cb.Weights = null;
            return;
        }
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviours[i] = cb.Behaviours[i];
            newWeights[i] = cb.Weights[i];
        }
        cb.Behaviours = newBehaviours;
        cb.Weights = newWeights;
    }
}