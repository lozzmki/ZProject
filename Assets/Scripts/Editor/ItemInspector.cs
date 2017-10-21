using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
class ItemInspector : Editor
{
    SerializedObject m_Object;
    SerializedProperty m_sName, m_Type, m_arg1, m_arg2, m_arg3, m_arg4;

    private void OnEnable()
    {
        m_Object = new SerializedObject(target);
        m_sName = m_Object.FindProperty("m_ItemName");
        m_Type = m_Object.FindProperty("m_Type");
        m_arg1 = m_Object.FindProperty("m_AttackSpeed");
        m_arg2 = m_Object.FindProperty("m_EnergyCost");
        m_arg3 = m_Object.FindProperty("m_Damage");
        m_arg4 = m_Object.FindProperty("m_Armor");

    }

    public override void OnInspectorGUI()
    {

        m_Object.Update();
        EditorGUILayout.PropertyField(m_sName);
        EditorGUILayout.PropertyField(m_Type);


        switch (m_Type.enumValueIndex) {
            case (int)ItemType.ITEM_PRIMARY:
                EditorGUILayout.PropertyField(m_arg1);
                EditorGUILayout.PropertyField(m_arg2);
                EditorGUILayout.PropertyField(m_arg3);
                break;
            case (int)ItemType.ITEM_MELEE:
                EditorGUILayout.PropertyField(m_arg1);
                EditorGUILayout.PropertyField(m_arg3);
                break;
            case (int)ItemType.ITEM_ARMOR:
                EditorGUILayout.PropertyField(m_arg4);
                break;
            case (int)ItemType.ITEM_PARTS:
                EditorGUILayout.PropertyField(m_arg3);
                break;
            default:
                break;
        }
        base.OnInspectorGUI();
        m_Object.ApplyModifiedProperties();
    }

}