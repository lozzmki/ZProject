using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Entity))]
public class EntityInspector : Editor {

    SerializedObject m_Object;
    SerializedProperty MaxHp, MaxEn, Melee, Range, Armor, Speed;

    private void OnEnable()
    {
        m_Object = new SerializedObject(target);
        MaxHp = m_Object.FindProperty("MaxHp");
        MaxEn = m_Object.FindProperty("MaxEn");
        Melee = m_Object.FindProperty("Melee");
        Range = m_Object.FindProperty("Range");
        Armor = m_Object.FindProperty("Armor");
        Speed = m_Object.FindProperty("Speed");
    }

    public override void OnInspectorGUI()
    {
        if(Application.isPlaying)
            base.OnInspectorGUI();
        else {
            EditorGUILayout.PropertyField(MaxHp);
            EditorGUILayout.PropertyField(MaxEn);
            EditorGUILayout.PropertyField(Melee);
            EditorGUILayout.PropertyField(Range);
            EditorGUILayout.PropertyField(Armor);
            EditorGUILayout.PropertyField(Speed);
            m_Object.ApplyModifiedProperties();
        }
        
    }
}
