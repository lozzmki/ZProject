using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MapWindow : EditorWindow {

    static CellManager _cellManager;
    [MenuItem("Window/GameEditor/MapGenerator")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<MapWindow>(false, "Map Generator");
        window.minSize = new Vector2(200, 100);

        _cellManager = GameObject.Find("CellContainer").GetComponent<CellManager>();
    }
    bool genMapGroup = true;
    bool cellGroup = false;
    bool ioGroup = false;
    int width = 0, height = 0;
    int materialID = -1;
    SCENEOBJ_TYPE selectedType = SCENEOBJ_TYPE.NONE;
    string mapFileName = null;
    private void OnGUI()
    {
        if (!_cellManager) { _cellManager = GameObject.Find("CellContainer").GetComponent<CellManager>(); }
        GUILayout.Label("Map Information", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("width & height:");
        GUILayout.Label(_cellManager.Width + " X " + _cellManager.Height);
        GUILayout.EndHorizontal();

        
        genMapGroup = EditorGUILayout.BeginToggleGroup("Regenerate Map", genMapGroup);
        {   
            GUILayout.BeginVertical();
            width = EditorGUILayout.IntField("width", width);
            height = EditorGUILayout.IntField("height", height);
            if (GUILayout.Button("ReGenerate"))
            {
                if (width >= 0 && height >= 0)
                {
                    _cellManager.RegenerateMap_Editor(width, height);
                }
            }
            if(GUILayout.Button("Fix Error"))
            {
                _cellManager.ReadMapFromEditor();
            }
            GUILayout.EndVertical();
            
        }
        EditorGUILayout.EndToggleGroup();

        cellGroup = EditorGUILayout.BeginToggleGroup("Tile", cellGroup);
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("BuildingType: ");
            selectedType = (SCENEOBJ_TYPE)EditorGUILayout.EnumPopup(selectedType);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation: ");
            if (GUILayout.Button("0")) {
                foreach (var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>() && obj.GetComponent<Cell>().GetBuilding)
                    {
                        obj.GetComponent<Cell>().GetBuilding.GetComponent<SceneObj>().GetObjRotate = 0;
                    }
                }
            }
            if (GUILayout.Button("90"))
            {
                foreach (var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>() && obj.GetComponent<Cell>().GetBuilding)
                    {
                        obj.GetComponent<Cell>().GetBuilding.GetComponent<SceneObj>().GetObjRotate = 1;
                    }
                }
            }
            if (GUILayout.Button("180"))
            {
                foreach (var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>() && obj.GetComponent<Cell>().GetBuilding)
                    {
                        obj.GetComponent<Cell>().GetBuilding.GetComponent<SceneObj>().GetObjRotate = 2;
                    }
                }
            }
            if (GUILayout.Button("270"))
            {
                foreach (var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>() && obj.GetComponent<Cell>().GetBuilding)
                    {
                        obj.GetComponent<Cell>().GetBuilding.GetComponent<SceneObj>().GetObjRotate = 3;
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Build"))
            {
                foreach(var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>())
                    {
                        obj.GetComponent<Cell>().BuildBox(selectedType);
                    }
                }
            }
            if (GUILayout.Button("UnBuild"))
            {
                List<Cell> cells = new List<Cell>();
                foreach (var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>() || obj.GetComponentInParent<SceneObj>())
                    {
                        if (obj.GetComponentInParent<SceneObj>())
                        {
                            cells.Add(obj.GetComponentInParent<SceneObj>().m_FatherCell);
                        }
                        else
                        {
                            cells.Add(obj.GetComponent<Cell>());
                        }
                    }
                }
                foreach(var cell in cells)
                {
                    cell.BuildBox(SCENEOBJ_TYPE.NONE);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            //GUILayout.Label("Material ID: ");
            materialID = EditorGUILayout.IntField("Material ID: ", materialID);
            if (GUILayout.Button("Use"))
            {
                foreach (var obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Cell>())
                    {
                        obj.GetComponent<Cell>().ChangeFoorMat(materialID);
                    }
                }
            }
            GUILayout.EndHorizontal();

            ioGroup = EditorGUILayout.BeginToggleGroup("IO", ioGroup);
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextField("map file name:", mapFileName);
                if (GUILayout.Button("..."))
                {
                    mapFileName = EditorUtility.OpenFilePanel("Open Map File", "", "txt");
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Output Map"))
                {
                    mapEditor.Output.OutputBinary(mapFileName);
                }
                if (GUILayout.Button("Read Map"))
                {
                    mapEditor.Output.ReadBinrayMap(mapFileName);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                
            }
            

        }
        
    }
}
