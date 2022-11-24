using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserDataEditor : EditorWindow
{

    [MenuItem("UserData/UserDataEditor")]
    public static void OpenWindow()
    {
        UserDataEditor window = GetWindow<UserDataEditor>("User data editor");
    }
    public UserData userData;
    private void OnEnable()
    {
        userData = SaveSystem.LoadPlayer();
    }
    private void OnGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            DrawSetting(userData);
            if (check.changed)
            {
                SaveSystem.SavePlayer(userData);
            }
        }
        if (GUILayout.Button("Save data"))
        {
            SaveSystem.SavePlayer(userData);
        }

        if (GUILayout.Button("Reset Data"))
        {
            UserData data = new UserData();
            SaveSystem.SavePlayer(data);
        }
    }
    void DrawSetting(UserData userData)
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty dataProperty = so.FindProperty("userData");
        EditorGUILayout.PropertyField(dataProperty, true);
        so.ApplyModifiedProperties();
    }
    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();
                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) continue;
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }
}
