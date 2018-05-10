using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private Text healthText;

    private void Start() {
        if (healthBarRect == null) {
            Debug.LogError("Status Indicator: no health bar object referenced.");
        }

        if (healthText == null) {
            Debug.LogError("Status Indicator: no health text object referenced.");
        }
    }

    public void SetHealth(int _curr, int _max) {
        float _value = (float)_curr / _max;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _curr + "/" + _max + " HP";
    }
}
