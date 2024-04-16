using AIV_Metroid_Player;
using Cinemachine;
using UnityEngine;
using System.Collections;

public class VirtualCameraController : MonoBehaviour
{

    private CinemachineVirtualCamera vCam;

    private CinemachineBasicMultiChannelPerlin noise;

    private void OnEnable() {
        GlobalEventManager.AddListener(GlobalEventIndex.ShakeCamera, OnShakeCamera);
    }

    private void OnDisable() {
        GlobalEventManager.RemoveListener(GlobalEventIndex.ShakeCamera, OnShakeCamera);
    }

    private void Start() {
        vCam = GetComponent<CinemachineVirtualCamera>();
        vCam.Follow = Player.Get().transform;
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnShakeCamera (GlobalEventArgs message) {
        GlobalEventArgsFactory.ShakeCameraParser(message, out float amplitude, out float frequency, out float duration);
        StartCoroutine(ShakeCamera(duration, amplitude, frequency));
    }

    private IEnumerator ShakeCamera (float duration, float amplitude, float frequency) {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        StopShakingCamera();
    }

    private void StopShakingCamera () {
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
