using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementEffect : PostEffectBase {
    public Shader _shader;
    [SerializeField]
    private Material _displacementMat;
    
    public Texture displacementTex;
    [Range(0f,0.1f)]
    public float magnitude;
    [Range(0f, 1f)]
    public float speed;
    private Material material {
        get {
            _displacementMat = CheckShaderAndCreateMaterial(_shader, _displacementMat);
            _displacementMat.SetTexture("_DisplacementTex", displacementTex);
            _displacementMat.SetFloat("_magnitude", magnitude);
            _displacementMat.SetFloat("_Speed", speed);
            return _displacementMat;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material != null) {
            Graphics.Blit(source, destination, material);
        }
        
    }
}
