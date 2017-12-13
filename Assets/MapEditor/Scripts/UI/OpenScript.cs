using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScript : MonoBehaviour {
    public MonoBehaviour _script;
    public void changeScriptOn() {
        if(_script.enabled == false) {
            _script.enabled = true;
        }else {
            _script.enabled = false;
        }

        
    }
}
