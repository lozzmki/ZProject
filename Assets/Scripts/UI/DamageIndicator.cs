using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator {

    public static Material s_nums;
    public static Material GetNumMat()
    {
        if (s_nums == null)
            s_nums = Resources.Load<Material>("UI/nums");
        return s_nums;
    }

    public static int GetIntLength(int num){
        int i = 1;
        for (; num>=10;) {
            num /= 10;
            i++;
        }
        return i;
    }

    public static void CreateFloatingText(Vector3 pos, int damage, bool isDamage = true)
    {
        int _length = GetIntLength(damage);

        Vector3[] _vertices = new Vector3[_length<<2];
        Vector2[] _uvs = new Vector2[_length << 2];
        int[] _triangles = new int[_length * 6];
        //Color[] _colors = new Color[_length << 2];

        Material _mat = GetNumMat();
        float _unitX = _mat.mainTexture.width / 20000.0f;
        float _unitY = _mat.mainTexture.height / 4000.0f;
        float _numX = 0;
        float _numY = 0;
        if (!isDamage)
            _numY = 0.5f;

        GameObject _obj = new GameObject();

        _obj.transform.position = pos;
        //_obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _obj.AddComponent<AutoFade>().m_fSetTime = 1.0f;
        _obj.AddComponent<MeshFilter>();
        _obj.AddComponent<MeshRenderer>().material = _mat;
        _obj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Mesh _mesh = _obj.GetComponent<MeshFilter>().mesh;

        for (int i= _length-1; i>=0; i--) {
            int _base = i * 4;
            _numX = damage % 10;
            damage /= 10;

            _vertices[_base + 0] = new Vector3(_unitX * i, 0);
            _vertices[_base + 1] = new Vector3(_unitX * i, _unitY);
            _vertices[_base + 2] = new Vector3(_unitX * (i + 1), _unitY);
            _vertices[_base + 3] = new Vector3(_unitX * (i + 1), 0);

            _uvs[_base + 0] = new Vector2(_numX/10, _numY);
            _uvs[_base + 1] = new Vector2(_numX / 10, _numY + 0.5f);
            _uvs[_base + 2] = new Vector2((_numX+1) / 10, _numY + 0.5f);
            _uvs[_base + 3] = new Vector2((_numX+1) / 10, _numY);

            int _trBase = i * 6;
            _triangles[_trBase + 0] = _base + 0;
            _triangles[_trBase + 1] = _base + 1;
            _triangles[_trBase + 2] = _base + 2;
            _triangles[_trBase + 3] = _base + 0;
            _triangles[_trBase + 4] = _base + 2;
            _triangles[_trBase + 5] = _base + 3;
        }


        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.uv = _uvs;
        _mesh.triangles = _triangles;
        
    }

    
}
