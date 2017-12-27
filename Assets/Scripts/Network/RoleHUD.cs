using UnityEngine;
using System.Collections;

public class RoleHUD : MonoBehaviour
{
    public float nameHeight = 1.0f;
    private Camera playerCamera = null;
    public string entity_name
    {
        get{
            KBEngine.PropsEntity e = GetComponent<SyncPosRot>().entity;
            if(e != null)
            {
                return e.name;
            }
            return "name";
        }
    }

    // Use this for initialization
    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void OnGUI()
    {
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + nameHeight, transform.position.z);

        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 uiposition = playerCamera.WorldToScreenPoint(worldPosition);

        //得到真实NPC头顶的2D坐标
        uiposition = new Vector2(uiposition.x, Screen.height - uiposition.y);

        //计算NPC名称的宽高
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(entity_name));

        //设置显示颜色
        GUI.color = Color.yellow;

        //绘制NPC名称
        GUI.Label(new Rect(uiposition.x - (nameSize.x / 2), uiposition.y - nameSize.y - 5.0f, nameSize.x, nameSize.y), entity_name);
    }

}
