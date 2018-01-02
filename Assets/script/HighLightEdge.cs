using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public struct RendererPackage
{
    Renderer _renderer;

}

[RequireComponent(typeof(Camera))]
public class HighLightEdge : MonoBehaviour
{
    #region Variables

    //高斯模糊的shader
    public Shader outlineShader;
    //纯色的shader
    public Shader baseShader;
    //需要描边的物体渲染器
    public List<Renderer> renderList;
    //描边颜色
    public Color color = Color.red;
    //描边粗细
    public float strength = 2.0f;
    //采样分辨率降低,越低越精细,性能越低
    public int downSample = 0;
    //模糊次数,越高越平滑,性能越低
    public int blurCount = 2;

    #endregion

    public Color OutlineColor
    {
        get { return color; }
        set { color = value; }
    }

    //outlineShader的材质载体
    private Material outlineMaterial;
    //baseShader的材质载体
    private Material baseMaterial;
    //当前摄像机
    //private Camera mCamera;
    //渲染指令控制器
    private CommandBuffer commandbuffer;

    //初始化
    private void Awake()
    {
        outlineMaterial = new Material(outlineShader);
        baseMaterial = new Material(baseShader);
        commandbuffer = new CommandBuffer();


        //mCamera = GetComponent<Camera>();
    }



    //顺序渲染
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (renderList.Count == 0) {
            Graphics.Blit(src, dest);
            return;
        }

        commandbuffer.Clear();
        commandbuffer.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        for (int i = 0; i < renderList.Count; ++i) {
            commandbuffer.DrawRenderer(renderList[i], baseMaterial);
        }
            


        //1.画出黑底,物体color
        baseMaterial.SetColor("_Color", OutlineColor);
        RenderTexture rt1 = RenderTexture.GetTemporary(Screen.width >> downSample, Screen.height >> downSample);
        Graphics.SetRenderTarget(rt1);
        Graphics.ExecuteCommandBuffer(commandbuffer);

        //2.高斯模糊渲染并与原场景缓存进行叠加
        //设置颜色,强度,迭代次数,src贴图
        outlineMaterial.SetFloat("_Strength", strength);
        outlineMaterial.SetInt("_IterCount", blurCount);
        outlineMaterial.SetTexture("_SrcTexture", src);
        outlineMaterial.SetColor("_Color", OutlineColor);
        Graphics.Blit(rt1, dest, outlineMaterial, 0);

        //for (int i = 0; i < renderList.Count; ++i)
        //    commandbuffer.DrawRenderer(renderList[i], );

        RenderTexture.ReleaseTemporary(rt1);
    }
}

//高斯模糊+屏幕后处理实现描边效果
/*
public class HighLightEdge : MonoBehaviour
{

    #region Variables 
    //两个渲染缓冲区
    private RenderTexture rendertexture1;
    private RenderTexture rendertexture2;
    private Mesh _mesh;
    private GameObject gameobj;
    //需要描边的物体
    public object obj;
    //描边shader
    public Shader curShader;
    //shader的载体
    public Material curMaterial;
    //描边的颜色
    public Color color=Color.red;
    //描边的粗细
    public double strength = 3.0f;
    //采样分辨率降低
    public int downSample = 1;

    #endregion

    #region Properties  
    public Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    #endregion

    public void OutlineObj(Mesh m,GameObject obj) {
        _mesh = m;
        gameobj = obj;
    }

    public void CancelOutlineObj() {
        _mesh = null;
        gameobj = null;
    }

    // Use this for initialization  
    void Start()
    {
        if (SystemInfo.supportsImageEffects == false)
        {
            enabled = false;
            return;
        }

        if (curShader != null && curShader.isSupported == false)
        {
            enabled = false;
        }
    }

    void DrawMesh(RenderTexture r)
    {
        CommandBuffer cb = new CommandBuffer();
        //设置渲染目标
        cb.SetRenderTarget(r);
        //黑底渲染
        cb.ClearRenderTarget(true, true, Color.red);
        
        //渲染物体
        
        //Render render = gameobj.GetComponentsInChildren<Renderer>();
        //cb.DrawRenderer(render, gameobj.material);
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //0.判断是否有需要描边的物体,并且要求对象没有被销毁,material也必须存活
        //判断obj销毁后期补全
        
        if (material==null || gameobj == null)
        {
            Graphics.Blit(sourceTexture, destTexture);
            return;
        }
        
        rendertexture1 = RenderTexture.GetTemporary(sourceTexture.width >> downSample, sourceTexture.height >> downSample,0);
        rendertexture2 = RenderTexture.GetTemporary(sourceTexture.width >> downSample, sourceTexture.height >> downSample, 0);

        //1.在rendertexture1中画出mesh,颜色使用color,背景是黑色
        DrawMesh(rendertexture1);
        Graphics.Blit(sourceTexture, destTexture);


        RenderTexture.ReleaseTemporary(rendertexture1);
        RenderTexture.ReleaseTemporary(rendertexture2);

        
        if (material != null && Target_Pos != null)
        {
            material.SetVector("_Target_Pos", Target_Pos);
            material.SetTexture("_MyTexture", t2d);

            Graphics.Blit(sourceTexture, destTexture, material);
        }
        
    }


    void OnDisable()
    {
        if (curMaterial != null)
        {
            DestroyImmediate(curMaterial, true);
        }
    }
}
*/