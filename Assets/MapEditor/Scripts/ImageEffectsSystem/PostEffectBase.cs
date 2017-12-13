using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CheckResource();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void CheckResource() {
        bool isSupported = checkSupported();
        if (!isSupported) {
            NotSupported();
        }
    }
    protected bool checkSupported() {
        if(SystemInfo.supportsImageEffects == false) {
            Debug.LogWarning("This platform dose not support image effect or render textures.");
            return false;
        }
        return true;
    }
    protected void NotSupported() {
        enabled = false;
    }
    protected Material CheckShaderAndCreateMaterial(Shader shader ,Material material) {
        if(shader.isSupported && material && material.shader == shader) {
            return material;
        }
        if (!shader.isSupported) {
            Debug.LogError("shader is not supported!");
            return null;
        } else {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material) {
                return material;
            }else {
                return null;
            }
        }
    }
}
