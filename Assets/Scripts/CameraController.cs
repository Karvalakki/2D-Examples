using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera basic2DCamera;
    public CinemachineFramingTransposer cameraSettings;
    public CinemachineBasicMultiChannelPerlin shakeSettings;

    //Screenshake parameters
    public float duration;
    public float amplitude;
    public float frequensy;

    // Start is called before the first frame update
    void Start()
    {
        //basic2DCamera.m_Lens.OrthographicSize = 3f;

        basic2DCamera = GetComponent<CinemachineVirtualCamera>();
        cameraSettings = basic2DCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        shakeSettings = basic2DCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cameraSettings.m_ScreenX = 0.5f;
        //shakeSettings.m_AmplitudeGain = myAmplitude;


    }

    public void ScreenShake(float myFrequency, float myDuration, float myAmplitude)
    {
        StartCoroutine(ScreenShakeStart(myFrequency, myDuration, myAmplitude));
    }

    public IEnumerator ScreenShakeStart(float myFrequency, float myDuration, float myAmplitude)
    {
        shakeSettings.m_AmplitudeGain = myAmplitude;
        shakeSettings.m_FrequencyGain = myFrequency;
        yield return new WaitForSeconds(myDuration);
        shakeSettings.m_AmplitudeGain = 0f;
    }

    /*public void ScreenShake()
    {
        shakeSettings.m_AmplitudeGain = 1f;
    }*/


    private void Update()
    {
        
    }

    /*
    public void FlipScreenX(bool facingRight)
    {
        if (facingRight)
        {
            cameraSettings.m_ScreenX = 0.2f;
        }

        else if (!facingRight)
        {
            cameraSettings.m_ScreenX = 0.7f;
        }
        
    }*/
}
