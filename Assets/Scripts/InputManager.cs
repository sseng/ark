using UnityEngine;
using System.Collections;

public class InputManager{

    public Vector3 MousePosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }        
    }
}