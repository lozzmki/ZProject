using UnityEngine;
using KBEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text loginInfo;
    public InputField account;
    public InputField password;
    public Button login;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
        ClientApp.instance.Load();
        installEvents();
    }

    void installEvents()
    {
        KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onLoginSuccess", this, "onLoginSuccess");
        KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        KBEngine.Event.registerOut("onLoginBaseapp", this, "onLoginBaseapp");

    }

    void OnDestroy()
    {
        //KBEngine.Event.deregisterOut(this);
    }

    public void onLogin()
    {
        info("连接到服务端...");
        KBEngine.Event.fireIn("login", account.text, password.text, System.Text.Encoding.UTF8.GetBytes("login test!!") ); 
    }

    void info(String _info)
    {
        loginInfo.text = _info;
    }

    public void onConnectionState(bool success)
    {
        if (!success)
        {
            info("连接错误...");
        }
        else
        {
            info("连接成功, 请稍后...");
        }
        
    }

    public void onLoginFailed(UInt16 code)
    {
        if(code == 20)
        {
            String _info = System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas());
            info("登陆失败..." + _info);
        }
        else if(code == 6)
        {
            info("密码错误...");
        }
        else
        {
            info("登陆失败: " + KBEngineApp.app.serverErr(code));
        }
    }

    public void onLoginSuccess(ulong uuid, int id, object account)
    {
        if(account!= null)
        {
            info("登陆成功...");
            Application.LoadLevel("Role");
        }
    }

    public void onLoginBaseapp()
    {
        info("连接到网关...");
    }

    public void onDisconnected()
    {
        Application.LoadLevel("login");
        // info("你已掉线...");
        //Invoke("onReloginBaseappTimer", 1.0f);
    }

    public void onReloginBaseappTimer()
    {

        KBEngineApp.app.reloginBaseapp();

    }
}
