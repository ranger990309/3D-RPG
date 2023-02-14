using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using XLua;

public class HotFixScript : MonoBehaviour
{
    private LuaEnv luaEnv;

    private void Awake()
    {
        luaEnv= new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'Test'");
    }

    private byte[] MyLoader(ref string filepath)
    {
        string absPath = @"D:\_unity\Projects\My Five 3D RPG Game\LuaScripts\" + filepath + ".lua.txt";
        return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(absPath));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        luaEnv.DoString("require 'LuaDispose'");
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}
