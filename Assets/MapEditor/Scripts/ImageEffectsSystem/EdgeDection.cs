using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDection : PostEffectBase {

    [SerializeField]
    private Shader edgeDetectShader;
    private Material edgeDectMaterial;

    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;
    public Color edgeColor = Color.black;
    public Color backgroundColor = Color.white;
    public Material material {
    get {
        edgeDectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDectMaterial);
        return edgeDectMaterial;
       }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material != null) {
            material.SetFloat("_EdgeOnly", edgesOnly);
            material.SetColor("_EdgeColor", edgeColor);
            material.SetColor("_BackgroundColor", backgroundColor);

            Graphics.Blit(source, destination, material);
        } else {
            Graphics.Blit(source, destination);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
