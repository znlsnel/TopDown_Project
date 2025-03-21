using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CinemachineCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimeRemaining; 

    private bool isInit = false;
	private void Awake() 
	{
        if (!isInit) Init(); 
	}


    public void Init()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        perlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        isInit = true;
    }

    public void ShakeCameara(float duration, float amplitude, float frequency)
    {
        if (!isInit) Init(); 

        if (shakeTimeRemaining > duration)
            return;

        shakeTimeRemaining = duration;

        perlin.AmplitudeGain = amplitude;
        perlin.FrequencyGain = frequency;
    }

	private void Update()
	{
		if (shakeTimeRemaining > 0)
        { 
			shakeTimeRemaining -= Time.deltaTime;
            if (shakeTimeRemaining <= 0f)
                StopShake();
        }
	}
	public void StopShake()
    {
        shakeTimeRemaining = 0f;

        perlin.FrequencyGain = 0f;
        perlin.AmplitudeGain = 0f;
	}

}
