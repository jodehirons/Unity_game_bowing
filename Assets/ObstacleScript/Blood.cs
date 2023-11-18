using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blood : MonoBehaviour
{
    private Renderer rend;
    public RawImage rawImage;
    public bool flag = true;
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && flag)
        {
            
            if(rawImage.rectTransform.sizeDelta.x < 600)
            {
                rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x + 60, rawImage.rectTransform.sizeDelta.y);
            }
            
            flag = false;
            Destroy(gameObject);
        }
    }

   
}