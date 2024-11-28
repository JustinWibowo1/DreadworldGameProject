using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthStat : MonoBehaviour
{
    private Image content;
    private float currentFill;
    private float currentValue;
    private float maxvalue;

    public float MyMaxValue
    {
        get { return maxvalue; }
        set
        {
            maxvalue = value;
            UpdateBar();
        }
    }

    public float MyCurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = Mathf.Clamp(value, 0, MyMaxValue);
            UpdateBar();
        }
    }

    void Start()
    {
        content = GetComponent<Image>();
        if (content == null)
        {
            Debug.LogError("Stat component requires an Image component on the same GameObject.");
        }
        MyMaxValue = 100;  // Ensure this is set after content is assigned
        MyCurrentValue = 100;  // Set an initial current value
    }

    private void UpdateBar()
    {
        if (content != null)
        {
            currentFill = currentValue / MyMaxValue;
            content.fillAmount = currentFill;
        }
    }
}