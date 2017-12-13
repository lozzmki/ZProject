using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ReplacementShader : MonoBehaviour {

    public Shader replaceShader;
    public Color OverDrawColor;

    void OnValidate() {
        Shader.SetGlobalColor("_OverDrawColor", OverDrawColor);
    }

    private void OnEnable() {
        if (replaceShader != null) {
            this.GetComponent<Camera>().SetReplacementShader(replaceShader, "");
        }
    }
    private void OnDisable() {
        this.GetComponent<Camera>().ResetReplacementShader();
    }
}
