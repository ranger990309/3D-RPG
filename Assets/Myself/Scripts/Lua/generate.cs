using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class generate : MonoBehaviour
{

    public GameObject GuaiWu;

    void Start()
    {
        
    }

    [LuaCallCSharp]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Create3();
        }
    }

    public void Create3()
    {
        GameObject.Instantiate(GuaiWu,transform.position,transform.rotation);
    }
}
