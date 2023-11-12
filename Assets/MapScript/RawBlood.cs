using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RawBlood : MonoBehaviour
{
    public RawImage rawImage; // ����Raw Image���������  
    public int maxHealth = 600; // ���Ѫ������  
    public float head;
    public TextMeshProUGUI headText;

    void Start()
    {
        head = 826f;
    }

    private void Update()
    {
        Myblood();
    }

    void Myblood()
    {
        float health = rawImage.rectTransform.sizeDelta.x;
        rawImage.rectTransform.anchoredPosition = new Vector2(health / 2 - head, rawImage.rectTransform.anchoredPosition.y);
        float t = health / 60;
        headText.text = t.ToString();
        
    }
}
