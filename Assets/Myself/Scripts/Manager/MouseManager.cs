using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

//方便在Unity中直接查看 序列化
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { };

public class MouseManager : Singleton<MouseManager>
{

    RaycastHit hitInfo;


    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    public Texture2D point, dorrway, attack, target, arrow;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this); 
    }

    private void Update()
    {
        SetCursorTexture();
        if (InteractWithUI()) return;
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Camera.main.ScreenPointToRay(Vector3)可以获取一条Camara到Vector3的射线

        if (InteractWithUI())
        {
            Cursor.SetCursor(point,Vector2.zero, CursorMode.Auto);
            return;
        }

        if (Physics.Raycast(ray,out hitInfo))
        {
            //切换鼠标贴图
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);break;
                case "Rock":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto); break;
                case "Portal":
                    Cursor.SetCursor(dorrway, new Vector2(16, 16), CursorMode.Auto); break;
                case "Item":
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto); break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto); break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider!=null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Rock"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }
    bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else return false;
    }
}
