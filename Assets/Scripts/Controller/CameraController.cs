using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private CinemachineVirtualCamera m_CinemachineVirtualCamera;

    private float m_ShakeTime;

    private float m_ShakeTimer;

    private float m_Intensity;

    private void Awake()
    {
        Instance = this;

        m_CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float intensity , float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            m_CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        m_ShakeTime = time;

        m_ShakeTimer = time;

        m_Intensity = intensity;
    }

    public void RandomShake(float minIntensity, float maxIntensity, float minTime, float maxTime)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            m_CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        m_Intensity = Random.Range(minIntensity, maxIntensity);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = m_Intensity;

        m_ShakeTime = Random.Range(minTime, maxTime);

        m_ShakeTimer = m_ShakeTime;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_ShakeTimer > 0.0f)
        {
            m_ShakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                m_CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(m_Intensity, 0.0f, 1 - (m_ShakeTimer / m_ShakeTime));
        }
    }

    void FixedUpdate()
    {
    }
}
