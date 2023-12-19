using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static float shakeStrength;
    public static float shakeTime;
    public bool shakeCameraOnEnabled;
    public float shakeOnEnableTime;
    public float shakeOnEnableStrength;

    private void OnEnable()
    {
        if (shakeCameraOnEnabled)
        {
            shakeStrength = shakeOnEnableStrength;
            shakeTime = shakeOnEnableTime;
        }
    }

    private void LateUpdate()
    {
        if(shakeTime > 0.0f && !GameManager.gameManager.paused)
        {
            shakeTime -= Time.deltaTime;
            transform.position += Random.rotation * Vector3.right * Random.Range(0, shakeStrength);
        }
    }

    public static void SetShake(float ShakeTime, float ShakeStrength)
    {
        shakeTime = ShakeTime;
        shakeStrength = ShakeStrength;
    }
}
