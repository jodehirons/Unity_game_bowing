using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RawBlood类用于处理血条的显示
/// </summary>
public class RawBlood : MonoBehaviour
{
    public RawImage rawImage; // 这是Raw Image组件的引用  
    public int maxHealth = 600;  // 最大血条长度 
    public float head;
    public TextMeshProUGUI headText;

    /// <summary>
    /// 初始化血条
    /// </summary>
    void Start()
    {
        head = 826f;
    }

    /// <summary>
    /// 每一帧调用血条更新
    /// </summary>
    private void Update()
    {
        Myblood();
    }

    /// <summary>
    /// 更新血条的显示
    /// </summary>
    void Myblood()
    {
        float health = rawImage.rectTransform.sizeDelta.x;
        rawImage.rectTransform.anchoredPosition = new Vector2(health / 2 - head, rawImage.rectTransform.anchoredPosition.y);
        float t = health / 60;
        headText.text = t.ToString();
        
    }
}
