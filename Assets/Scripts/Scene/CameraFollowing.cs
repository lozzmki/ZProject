using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour {

    public enum CameraMode
    {
        CAMERA_OVERLOOK,
        CAMERA_FOLLOW,
    }

    #region Properties
    public float m_TurningSpeed = 180.0f;
    public float m_MinDistance = 8.0f;
    public float m_MaxDistance = 10.0f;
    public bool m_HorRotationInvert = false;
    public bool m_VerRotationInvert = false;
    public CameraMode m_CameraMode = CameraMode.CAMERA_FOLLOW;
    #endregion

    #region Runtime
    private float m_fCurrentHorAngle;
    private float m_fCurrentSpeed;
    private Transform m_trTarget;
    private Vector3 m_lastPosition;
    private float m_fHorRotation;
    private float m_fVerRotation;
    #endregion

    #region Constant
    private const int m_nSceneObjectLayer = 1<<9;
    #endregion

    //// Use this for initialization
    //void Start () {
    //    
    //}

    // Update is called once per frame
    void LateUpdate () {
        if (m_trTarget == null)
            return;
        m_fHorRotation = Input.GetAxis("RH") * m_TurningSpeed * Time.deltaTime;
        m_fVerRotation = Input.GetAxis("RV") * m_TurningSpeed * Time.deltaTime / 2.0f;
        if (m_HorRotationInvert) {
            m_fHorRotation = -m_fHorRotation;
        }
        if (m_VerRotationInvert) {
            m_fVerRotation = -m_fVerRotation;
        }
        switch (m_CameraMode) {
            case CameraMode.CAMERA_OVERLOOK:
                OverLookMode();
                break;
            case CameraMode.CAMERA_FOLLOW:
                FollowMode();
                break;
            default:
                break;
        }

    }

    public void BindToTarget(Transform tr)
    {
        m_trTarget = tr;
    }

    private void OverLookMode()
    {
        Vector3 _pos = m_trTarget.position;
        Vector3 _delta = _pos - transform.position;
        _delta.Scale(new Vector3(1.0f, 0.0f, 1.0f));
        _delta = _delta.normalized + Vector3.down;

        transform.position = _pos - m_MaxDistance * _delta;

        //apply rotation
        if(m_fHorRotation >1e-4 || m_fHorRotation<-1e-4)
            transform.RotateAround(_pos, Vector3.up, m_fHorRotation);

        transform.LookAt(_pos);

        RaycastHit _hit;
        if(Physics.Linecast(_pos, transform.position,out _hit,m_nSceneObjectLayer, QueryTriggerInteraction.Ignore)) {
            transform.position = _hit.point + (_pos - _hit.point)*0.1f;
        }
    }

    private void FollowMode()
    {
        Vector3 _pos = m_trTarget.position;
        Vector3 _delta = _pos - transform.position;
        float _dis = _delta.magnitude;

        if(_dis < m_MinDistance) {
            transform.position = _pos - transform.forward.normalized * m_MinDistance;
        }else if(_dis > m_MaxDistance) {
            transform.position = _pos - transform.forward.normalized * m_MaxDistance;
        }

        //apply rotation
        if (m_fHorRotation > 1e-4 || m_fHorRotation < -1e-4)
            transform.RotateAround(_pos, Vector3.up, m_fHorRotation);

        Vector3 _fwd = transform.forward.normalized;
        float _fy = -_fwd.y;
        _fwd.Scale(new Vector3(1.0f, 0.0f, 1.0f));
        float _fx = _fwd.magnitude;

        if( (m_fVerRotation > 1e-4 && _fx>0.3f) || (m_fVerRotation < -1e-4 && _fy > 0.0f)) {
            transform.RotateAround(_pos, transform.right, m_fVerRotation);
        }
        transform.LookAt(_pos);

        RaycastHit _hit;
        if (Physics.Linecast(_pos, transform.position, out _hit, m_nSceneObjectLayer, QueryTriggerInteraction.Ignore)) {
            transform.position = _hit.point + (_pos - _hit.point) * 0.1f;
        }
    }
}
