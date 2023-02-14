using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public bool alwaysVisible;
    public float visibleTime;
    public Transform barPoint;

    private Image healthSlider;
    private Transform UIBar;
    private Transform cam;
    private CharacterStates currentStates;
    private float leftTime;

    private void Awake()
    {
        currentStates = GetComponent<CharacterStates>();
        currentStates.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)//更新生命条
    {
        if(currentHealth <= 0)//生命小于0删除生命条
        {
            Destroy(UIBar.gameObject);
        }
        UIBar.gameObject.SetActive(true);//生命条显示
        leftTime = visibleTime;//没攻击久点生命条消失
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;

            if(leftTime <= 0 && !alwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                leftTime -= Time.deltaTime;
            }
        }
    }
}
