using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RgbEffect : PostEffectBase {
    public Shader _rgbShader;
    public Texture rgbTex;
    private Material _rgbMat;

    private Material material {
        get {
            _rgbMat = CheckShaderAndCreateMaterial(_rgbShader, _rgbMat);
            _rgbMat.SetTexture("_rgbTex", rgbTex);
            return _rgbMat;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material != null) {
            Graphics.Blit(source, destination, _rgbMat);
        }else {
            Debug.LogError("mat is null");
        }
       

    }

}
