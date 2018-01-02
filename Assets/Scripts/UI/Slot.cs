using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler {

    public GameObject item;

    // Use this for initialization
    //void Start () {
    //	
    //}
    //
    //// Update is called once per frame
    //void Update () {
    //	
    //}

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("test");
    }
}
