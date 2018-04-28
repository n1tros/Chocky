using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlayerHealthBar : MonoBehaviour
{

    RawImage _healthBarRawImage;

    // Use this for initialization
    void Start()
    {
        _healthBarRawImage = GetComponent<RawImage>();
    }

    public void HealthUpdate(float currentHealth)
    {
        float xValue = -(currentHealth / 2f) - 0.5f;
        _healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }


}
