
using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow {

    [MenuItem("Window/Example")]
	public static void ShowWindow() {
        GetWindow<MyWindow>("Example");
	}
    string mystring = "Hello world!";
    Color color;

    Object selectedCell;
    void OnGUI()
    {
        
        GUILayout.Label("This is a label", EditorStyles.boldLabel);
        mystring = EditorGUILayout.TextField("inout", mystring);
        
        if (GUILayout.Button("PRINT"))
        {
            Debug.Log(mystring);
        }
        color = EditorGUILayout.ColorField("Color", color);

        selectedCell = EditorGUILayout.ObjectField(selectedCell, typeof(Cell), true);

        if (GUILayout.Button("selection OBJ"))
        {
            var obj = Selection.gameObjects[0];
            if (obj.GetComponent<Cell>())
            {
                selectedCell = obj;
            }
        }
        
    }
}
