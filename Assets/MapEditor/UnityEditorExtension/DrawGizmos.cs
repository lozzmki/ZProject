using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DrawGizmos : MonoBehaviour {

    void OnDrawGizmos()
    {
        
        var collider = this.GetComponent<BoxCollider>();
        var size = new Vector3(collider.size.x * transform.localScale.x,
            collider.size.y * transform.localScale.y,
            collider.size.z * transform.localScale.z);
        Gizmos.color = new Color(0f, 1f, 0.6f, 1f);
        Gizmos.DrawWireCube(transform.position+ collider.center, size);

        Gizmos.color = new Color(0f, 1f, 0.6f, 0.3f);
        Gizmos.DrawCube(transform.position+collider.center , size);
    }

    // OnDrawGizmosSelect()类似于OnDrawGizmos()，它会在当该组件所属的物体被选中时被调用
    
    void OnDrawGizmosSelected()
    {
        var collider = this.GetComponent<BoxCollider>();
        var size = new Vector3(collider.size.x * transform.localScale.x,
            collider.size.y * transform.localScale.y,
            collider.size.z * transform.localScale.z);
        Gizmos.color = new Color(1f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + collider.center, size);

        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position + collider.center, size);
    }
}
