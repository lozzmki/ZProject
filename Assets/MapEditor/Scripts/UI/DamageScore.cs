using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class CreateScore {

}
public class DamageScore : MonoBehaviour {
    public int Socre;
    [SerializeField]
    float planeWid;
    [SerializeField]
    Material matpreb;
    [SerializeField]
    Color scoreColor;
    [SerializeField]
    public Transform planePreb;
    [SerializeField]
    Transform[] planes;
    [SerializeField]
    List<Material> mats;
    [SerializeField]
    Texture[] socreTexs;
    public void CreateScore(int number) {
        string numStr = number.ToString();

        planes = new Transform[numStr.Length];
        for(int i = 0; i < numStr.Length; i++) {
            var plane = Instantiate(planePreb);
            var mat = new Material(matpreb);
            mat.SetTexture("_MainTex", socreTexs[numStr[i] - '0']);
            mat.SetColor("_Color", scoreColor);
            plane.GetComponent<MeshRenderer>().sharedMaterial = mat;
            planes[i] = plane;      
        }

        var mid = planes.Length / 2;
        for(int i = 0; i < planes.Length; i++) {
            planes[i].transform.parent = this.transform;
            planes[i].localPosition = new Vector3((i - mid) * planeWid, 0, 0);
        }
    }

    private void Start() {
        CreateScore(Socre);
    }
}
