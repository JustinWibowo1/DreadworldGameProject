using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaStat : MonoBehaviour
{
    private Image content;
    private float currentFill;
    private float currentValue;
    private float maxValue;

    public float MyMaxValue
    {
        get { return maxValue; }
        set
        {
            maxValue = value;
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
            Debug.LogError("Stamina component requires an Image component on the same GameObject.");
        }
        MyMaxValue = 100;  // Set the maximum value for the stamina bar
        MyCurrentValue = 100;  // Start with full stamina
    }

    private void UpdateBar()
    {
        if (content != null)
        {
            currentFill = currentValue / MyMaxValue;
            content.fillAmount = currentFill;
        }
    }

    void Update()
    {
        // Example of automatic stamina regeneration
        if (currentValue < MyMaxValue)
        {
            MyCurrentValue += Time.deltaTime * 5; // Regenerate stamina over time
        }
    }
}