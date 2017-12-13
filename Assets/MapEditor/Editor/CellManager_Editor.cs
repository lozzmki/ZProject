using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CellManager))]
public class CellMachine_Editor : Editor {

    public override void OnInspectorGUI() {
        CellManager obj = (CellManager)target;
        DrawDefaultInspector();
        if (GUILayout.Button("生成网格")) {
            obj.generateMap();
        }
        if (GUILayout.Button("清除网格")) {
            obj.clearMap();
        }

        if (GUILayout.Button("在边界填充方块")) {
            obj.BuildBorder();
        }
    }
}
[CustomEditor(typeof(Cell))]
public class Cell_Editor : Editor {

    public override void OnInspectorGUI() {
        Cell obj = (Cell)target;
        DrawDefaultInspector();
        if (GUILayout.Button("增加实体")) {
            obj.BuildBox_Editor();
        }
    }
}

//TEST ，用于血条
[CustomEditor(typeof(BloodSlider))]
public class BloodSlider_Editor :Editor {
    public override void OnInspectorGUI() {
        BloodSlider obj = (BloodSlider)target;
        DrawDefaultInspector();
        if (GUILayout.Button("扣20滴血")) {
            obj.Sub_20_Blood();
        }
    }
}