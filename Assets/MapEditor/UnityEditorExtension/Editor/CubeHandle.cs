using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class CubeHandle : Editor {

    public static Vector3 CurrentHandlePosition = new Vector3(-100000, 0, 0);
    public static bool IsMouseInValidArea = false;
    static GameObject _tag;
    static Vector3 m_OldHandlePosition = new Vector3(-100000,0,0);

	static CubeHandle()
    {
       // SceneView.onSceneGUIDelegate -= OnSceneUI;
       // SceneView.onSceneGUIDelegate += OnSceneUI;
    }
    private void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneUI;
    }
    static void OnSceneUI(SceneView sceneView)
    {
        UpdateHandlePosition();
        UpdateIsMouseInValidArea(sceneView.position);
        UpdateRepaint();

        DrawCubePreview();
    }
    static void UpdateIsMouseInValidArea(Rect sceneViewRect)
    {
        bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35;
        if(isInValidArea != IsMouseInValidArea)
        {
            IsMouseInValidArea = isInValidArea;
            SceneView.RepaintAll();
        }
    }
    static void UpdateHandlePosition()
    {
        if(Event.current == null)
        {
            return;
        }
        Vector2 mousPosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);

        Ray ray = HandleUtility.GUIPointToWorldRay(mousPosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Cell")))
        {
            CurrentHandlePosition = hit.collider.gameObject.transform.position;
        }
    }
    static void UpdateRepaint()
    {
        if(CurrentHandlePosition != m_OldHandlePosition)
        {
            SceneView.RepaintAll();
            m_OldHandlePosition = CurrentHandlePosition;
        }
    }
    static void DrawCubePreview()
    {
        if(IsMouseInValidArea == false)
        {
            return;
        }
        Handles.color = new Color(0, 1, 0, 1);
        DrawHandlesCube(CurrentHandlePosition);
    }
    static void DrawHandlesCube(Vector3 center)
    {
        if (CubeHandle._tag == null)
        {
            _tag = Instantiate(GameObject.Find("GameStone").GetComponent<ResrouceManager>().P_TouchedTag);
        }
        
        _tag.transform.position = center;
    }
}
