using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshPro healthNumber;
    
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        //healthNumber = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        healthNumber.text = health.ToString();
    }

    public void SetCurrentHealth(int health)
    {
        slider.value = health;
        healthNumber.text = health.ToString();
    }
}
