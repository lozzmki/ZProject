using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BloodSlider : MonoBehaviour {
    //上层血量条,瞬间变化
    public Slider blood1;
    //下层血量条，动态变化
    public Slider blood2;
    //当前血量
    [SerializeField]
    private float _value = 1;
    [SerializeField]
    private float _smoothTime = 0.5f;
    public void Start() {
        blood1.value = _value;
        blood2.value = _value;

    }
    //对外接口，设置血量
    public void SetValue(float v) {
        if (v < 0) { v = 0; }
        else if (v > 1) { v = 1; }

        blood1.value = v;
        _value = v;
        StartCoroutine(blood2Anim());
    }
    IEnumerator blood2Anim() {
        float _time = 0;
        float startValue = blood2.value;
        float endValue = _value;
        while (_time < _smoothTime) {
            _time += Time.deltaTime;
            float value = Mathf.Lerp(startValue, endValue, _time / _smoothTime);
            blood2.value = value;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        blood2.value = _value;
    }
    //test
    public void Sub_20_Blood() {
        SetValue(_value - 0.2f);
    }
}
