using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] Slider sliderAudio;
    public void SetState(float value)
    {
        sliderAudio.value = value;
    }

    public void ChangeVolume()
    {
        // AudioManager.Instance.GetEffectValue(sliderAudio.value);
    }
}
