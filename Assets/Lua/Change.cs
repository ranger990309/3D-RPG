using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[Hotfix]
public class Change : MonoBehaviour
{
    public GameObject ground;
    public GameObject ground2;
    public Button LuaButton;

    private void Awake()
    {
        //ButtonBinding();
    }

    void Start()
    {
        //ground.gameObject.SetActive(false);
        //ground2.gameObject.SetActive(true);
        ButtonBinding();
    }

    [LuaCallCSharp]
    void Update()
    {

    }

    [LuaCallCSharp]
    public void ButtonBinding()
    {
        //LuaButton = GameObject.Find("LuaButton").GetComponent<Button>();
        //LuaButton.onClick.AddListener(delegate { Create(); });
    }

    [LuaCallCSharp]
    public void ChangeGround()
    {
    
    }

    //在游戏首页该地图
    public void Create()
    {
        ground.gameObject.SetActive(false);
        ground2.gameObject.SetActive(true);
    }

    public void Create2()
    {
        ground.gameObject.SetActive(true);
        ground2.gameObject.SetActive(false);
    }
}
