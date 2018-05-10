using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;

    float shakeAmount = 0;

    private void Awake() {
        if (mainCam == null) {
            mainCam = Camera.main;
        }
    }

    public void Shake(float amount, float length) {
        shakeAmount = amount;
        InvokeRepeating("StartShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            Shake(0.1f, 0.2f);
        }
    }

    void StartShake() {
        if (shakeAmount > 0) {
            Vector3 camPos = mainCam.transform.position;

            // Offset amount
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }

    void StopShake() {
        CancelInvoke("StartShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
