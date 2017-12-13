using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawListTest))]
public class DrawListWindow : Editor {

    DrawListTest m_Target;
    public override void OnInspectorGUI()
    {
        m_Target = (DrawListTest)target;

        DrawDefaultInspector();
        DrawMembersInspector();
        
    }
    void DrawMembersInspector()
    {
        GUILayout.Space(5);
        GUILayout.Label("States", EditorStyles.boldLabel);

        for(int i = 0; i < m_Target.memberList.Count; i++)
        {
            DrawMember(i);
        }
        DrawAddStateButton();
    }
    void DrawAddStateButton()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add new Member"))
        {
            m_Target.memberList.Add(new Member());
        }
        DrawPrintButton();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    void DrawPrintButton()
    {
        if (GUILayout.Button("print list"))
        {
            m_Target.printList();
        }
    }
    void DrawMember(int index) {
        if(index <0 || index >= m_Target.memberList.Count)
        {
            return;
        }
        SerializedProperty listIterator = serializedObject.FindProperty("memberList");

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Name", EditorStyles.label, GUILayout.Width(60));

            EditorGUI.BeginChangeCheck();
            string newName = EditorGUILayout.TextField(m_Target.memberList[index].name,GUILayout.Width(120));
            Vector3 newPosition = EditorGUILayout.Vector3Field("", m_Target.memberList[index].pos);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Target, "Modify State");

                m_Target.memberList[index].name = newName;
                m_Target.memberList[index].pos = newPosition;
                EditorUtility.SetDirty(m_Target);
             }
            if (GUILayout.Button("Remove"))
            {
                EditorApplication.Beep();
                if (EditorUtility.DisplayDialog("删除提示","Really? Do you really want to remove the state"
                    + m_Target.memberList[index] + "?","Yes","No") == true)
                {
                    Undo.RecordObject(m_Target, "Delete State");
                    m_Target.memberList.RemoveAt(index);
                    EditorUtility.SetDirty(m_Target);
                }

            }
            
        }
        GUILayout.EndHorizontal();
    }
}
