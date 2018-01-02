using UnityEngine;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIRole : MonoBehaviour
{
    public Text tipInfo;
    public GameObject chooseRoleUI;
    public Image curChooseRoleImg;
    public InputField roleNameInput;
    public Button jumpToCreateBtn;
    public Button enterGameBtn;
    public GameObject roleList;

    private Dictionary<UInt64, Transform> rolePanelDic = new Dictionary<UInt64, Transform>();
    private Dictionary<UInt64, Dictionary<string, object>> roles;
    private float newRolePanelPosY = 0;
    private Transform selectRolePanel = null;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        KBEngine.Event.registerOut("recRoleList", this, "recRoleList");
        KBEngine.Event.registerOut("recCreateRole", this, "recCreateRole"); 
        KBEngine.Event.registerOut("recRemoveRole", this, "recRemoveRole");

        reqRoleList();
    }

    private void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }

    private void info(string txt)
    {
        tipInfo.text = txt;
    }

    public void onClickRole(Image img)
    {
        curChooseRoleImg.gameObject.SetActive(true);
        curChooseRoleImg.sprite = img.sprite;
    }

    public void onClickSelectedRole(Transform role)
    {
        selectRolePanel = role;
        curChooseRoleImg.gameObject.SetActive(true);
        Image avatar = selectRolePanel.FindChild("avatar").GetComponent<Image>();
        curChooseRoleImg.sprite = avatar.sprite; 
    }
    private void openChooseUIPanel(bool open)
    {
        chooseRoleUI.gameObject.SetActive(open);
        enterGameBtn.gameObject.SetActive(!open);
        jumpToCreateBtn.gameObject.SetActive(!open);
    }

    public void onBack()
    {
        if(jumpToCreateBtn.gameObject.active)
            Application.LoadLevel("login");
        else
        {
            openChooseUIPanel(false);
        }
    }

    public void onEnterGame()
    {
        Account account = (Account)KBEngineApp.app.player();
        if(account != null)
        {
            if(selectRolePanel != null)
            {
                UInt64 _roleDbid = UInt64.Parse(selectRolePanel.name);
                AsyncSceneMgr.singleton.loadMainScene(_roleDbid);
            }
            else
            {
                info("请选择一个角色...");
            }
        }
    }

    public void onJumpToCreateRole()
    {
        openChooseUIPanel(true);
    }

    public void onCreateRole()
    {
        if(roles.Count >= 5)
        {
            info("角色数量已达到上限...");
            return;
        }

        if (!curChooseRoleImg.IsActive())
        {
            info("请选择角色...");
            return;
        }

        if (roleNameInput.text == "")
        {
            info("请输入名称...");
            return;
        }

        //change UI state
        openChooseUIPanel(false);

        //request create role
        reqCreateRole();
    }

    public void onRemvoeRole()
    {
        if(selectRolePanel != null)
        {
            UInt64 dbid = UInt64.Parse(selectRolePanel.name);
            reqRemoveRole(dbid);
        }
    }

    public void reqRoleList()
    {
        KBEngine.Event.fireIn("reqRoleList");
    }

    public void recRoleList(Byte res, Dictionary<UInt64, Dictionary<string, object>> roleListInfos)
    {
        if(res == 0) //success
        {
            roles = roleListInfos;
            createRoleListPanels();
            updateRoleListPanelsInfo();
        }
    }

    public void reqCreateRole()
    {
        string name = roleNameInput.text;
        Byte career = (Byte)(curChooseRoleImg.sprite.name == "saber" ? 1 : 2);
        KBEngine.Event.fireIn("reqCreateRole", new object[] { name, career });
    }

    public void recCreateRole(Byte res, Dictionary<string, object> roleInfo)
    {
        if(res == 0) //success
        {
            Transform role_template = roleList.transform.FindChild("role_template");

            //new panel
            Transform role_panel = Instantiate(role_template, roleList.transform);
            role_panel.gameObject.SetActive(true);
            rolePanelDic[(UInt64)roleInfo["dbid"]] = role_panel;
            selectRolePanel = role_panel;

            updateRoleListPanelsInfo();          
        }
    }

    public void reqRemoveRole(UInt64 dbid)
    {
        KBEngine.Event.fireIn("reqRemoveRole", new object[]{ dbid});
    }

    public void recRemoveRole(UInt64 dbid)
    {
        if(dbid != 0) // success
        {
            Transform role_panel = rolePanelDic[dbid];
            GameObject.Destroy(role_panel.gameObject);
            rolePanelDic.Remove(dbid);

            //set select role panel
            if(rolePanelDic.Count > 0)
            {
                foreach(UInt64 _dbid in rolePanelDic.Keys)
                {
                    selectRolePanel = rolePanelDic[_dbid];
                    break;
                }
            }
            else
            {
                selectRolePanel = null;
            }

            //update panel info
            updateRoleListPanelsInfo();
        }
    }

    private void createRoleListPanels()
    {
        Transform role_template = roleList.transform.FindChild("role_template");
        foreach(UInt64 dbid in roles.Keys)
        {
            //new panel
            Transform role_panel = Instantiate(role_template, roleList.transform);
            role_panel.gameObject.SetActive(true);
            rolePanelDic[dbid] = role_panel;
        }
    }

    private void updateRoleListPanelsInfo()
    {
        newRolePanelPosY = 0;
        foreach (UInt64 dbid in rolePanelDic.Keys)
        {
            Transform role_panel = rolePanelDic[dbid];
            Dictionary<string, object> info = roles[dbid];

            //set Pos
            RectTransform rectTrans = role_panel.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(0, newRolePanelPosY);
            newRolePanelPosY -= rectTrans.sizeDelta.y;

            //find component
            Image roleImg = role_panel.FindChild("avatar").gameObject.GetComponent<Image>();
            Text nameTxt = role_panel.FindChild("name").gameObject.GetComponent<Text>();
            Text careerTxt = role_panel.FindChild("career").gameObject.GetComponent<Text>();
            Text levelTxt = role_panel.FindChild("level").gameObject.GetComponent<Text>();

            //set role data
            //roleImg.sprite = curChooseRoleImg.sprite;
            role_panel.gameObject.name = ((UInt64)info["dbid"]).ToString();
            nameTxt.text = "名字: " + (string)info["name"];
            careerTxt.text = "职业: " + ((Byte)info["career"]).ToString();
            levelTxt.text = "等级: " + ((UInt16)info["level"]).ToString();
            if(((Byte)info["career"]) == 1)
            {
                roleImg.sprite = Resources.Load<Sprite>("pic/saber");
            }
            else
            {
                roleImg.sprite = Resources.Load<Sprite>("pic/gunner");
            }
        }
    }
}
