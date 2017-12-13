using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawListTest : MonoBehaviour {

    [HideInInspector]
    public List<Member> memberList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void printList()
    {
        foreach(Member mem in memberList)
        {
            print(mem.name+" "+mem.pos);
        }
    }
}
[System.Serializable]
public class Member
{
    public string name;
    public Vector3 pos;
}
