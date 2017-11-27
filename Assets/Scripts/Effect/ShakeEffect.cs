using UnityEngine;
using System.Collections;

/*
 * @author: beyondray
 * @Desc: 将此脚本挂在任何物体上都可实现震动效果
 * case1: 挂在摄像机上模拟震屏
 * case2: 挂在地形上模拟大地震动
 * case3: 挂在物体上模拟鬼畜抖动
*/

public class ShakeEffect : MonoBehaviour
{
#region public
    [SerializeField]
    //shake mode
    public ShakeMode shakeMode = ShakeMode.Discrete;
    //shake scope
    public ShakeScope begScope = new ShakeScope(0.15f, 0.15f, 0.15f);
    public ShakeScope endScope = new ShakeScope(0.2f, 0.2f, 0.2f);
    public float scopeGap = 0.025f;
    //shake time
    public float minSingleTime = 0.3f;
    public float maxSingleTime = 0.5f;
    public float shakeTime = Mathf.Infinity;
    //shake freeze
    public ShakeFreeze freeze = new ShakeFreeze(false, false, false);
#endregion

#region userdefine
    [System.Serializable]
    public enum ShakeMode
    {
        Discrete = 0,
        DiscreteSin = 1,
        Continuous = 2,
    };

    [System.Serializable]
    public struct ShakeScope
    {
        public float lenX, lenY, lenZ;
        public ShakeScope(float x, float y, float z)
        {
            lenX = x; lenY = y; lenZ = z;
        }
    }

    [System.Serializable]
    public struct ShakeFreeze
    {
        public bool X, Y, Z;
        public ShakeFreeze(bool x, bool y, bool z)
        {
            X = x; Y = y; Z = z;
        }
    }
#endregion

#region internal
    Coroutine loopShakeCo;
#endregion

    // Use this for initialization
    void Start()
    {
        StartShake();
    }

    public void StartShake()
    {
        if(loopShakeCo == null)
        {
            loopShakeCo = StartCoroutine(LoopRangeShake());
        }
    }

    public void StartShake(float time)
    {
        this.shakeTime = time;
        if (loopShakeCo == null)
        {
            loopShakeCo = StartCoroutine(LoopRangeShake());
        }
    }

    public void StopShake()
    {
        StopCoroutine(loopShakeCo);
        loopShakeCo = null;
    }

    IEnumerator LoopRangeShake()
    {
        float t = 0f;
        while (t < shakeTime || t == Mathf.Infinity)
        {
            float dt = Random.Range(minSingleTime, maxSingleTime);
            t += dt;
            yield return RandomShake(dt);
        }
    }

    IEnumerator RandomShake(float time)
    {
        int x = (int)((endScope.lenX - begScope.lenX) / scopeGap);
        int y = (int)((endScope.lenY - begScope.lenY) / scopeGap);
        int z = (int)((endScope.lenZ - begScope.lenZ) / scopeGap);

        float lenX = begScope.lenX + x * scopeGap;
        float lenY = begScope.lenY + y * scopeGap;
        float lenZ = begScope.lenZ + z * scopeGap;
        yield return StartCoroutine(Shake(lenX, lenY, lenZ, time));
    }


    Vector3 getShakeDir(float maxLen, float time, float t, Vector3 wDir, bool freeze)
    {
        Vector3 shakeDir = Vector3.zero;
        if (!freeze)
        {
            // get direction
            Vector3 lDir = wDir;
            if (transform.parent)
            {
                lDir = transform.parent.InverseTransformDirection(wDir);
            }

            // get offset length
            float len = 0;
            float w = 2 * Mathf.PI / time;
            switch (shakeMode)
            {
                case ShakeMode.Discrete:
                    len = Mathf.Lerp(-maxLen / 2, maxLen / 2, Random.Range(0, 1.0f));
                    break;
                case ShakeMode.DiscreteSin:
                    len = Mathf.Lerp(-maxLen / 2, maxLen / 2, Random.Range(0, 1.0f)) * Mathf.Sin(w * t);
                    break;
                case ShakeMode.Continuous:
                    if (wDir == transform.forward)
                        len = maxLen * Mathf.Sin(w * t + Mathf.PI / 2);
                    else if (wDir == transform.up)
                        len = maxLen * Mathf.Cos(w * t);
                    else
                        len = maxLen * Mathf.Sin(w * t);
                    break;
            }
            shakeDir = len * lDir;
        }
        return shakeDir;
    }

    IEnumerator Shake(float lenX = 0.25f, float lenY = 0.25f, float lenZ = 0.25f, float time = 1f)
    {
        Vector3 orgPos = transform.localPosition;
        Vector3 offset = Vector3.zero;
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            orgPos = transform.localPosition - offset;

            //shake X, Y, Z
            Vector3 shakeX = getShakeDir(lenX, time, t, transform.right, freeze.X);
            Vector3 shakeY = getShakeDir(lenY, time, t, transform.up, freeze.Y);
            Vector3 shakeZ = getShakeDir(lenZ, time, t, transform.forward, freeze.Z);

            //calculate transform
            offset = shakeX + shakeY + shakeZ;
            transform.localPosition = orgPos + offset;

            yield return null;
        }
        transform.localPosition -= offset;
    }

    IEnumerator Shake(ShakeScope ss, float time = 1f)
    {
        yield return Shake(ss.lenX, ss.lenY, ss.lenZ, time);
    }
}
