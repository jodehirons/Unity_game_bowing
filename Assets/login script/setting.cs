using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setting : MonoBehaviour
{
    public Slider mainSlider;
    public Slider bgmSlider;
    public Slider effectSlider;
    public AudioSource bgmVolume;
    public AudioSource effectVolume;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 根据滑动条的值来设置音量
        if (bgmSlider != null )
        {
            bgmVolume.volume = mainSlider.value * bgmSlider.value;
        }
        if (effectSlider != null)
        {
            effectVolume.volume = mainSlider.value * effectSlider.value;
        }
    }
}
