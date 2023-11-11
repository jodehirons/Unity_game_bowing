using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
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
            StartCoroutine(BlinkAndDisappear());
            if (rawImage.rectTransform.sizeDelta.x > 0)
                rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x - 60, rawImage.rectTransform.sizeDelta.y);
            flag = false;
        }
    }

    IEnumerator BlinkAndDisappear()
    {
        for (int i = 0; i < 2; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(0.1f);
            rend.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    }
}