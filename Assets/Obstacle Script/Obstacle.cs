using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(BlinkAndDisappear());
        }
    }

    IEnumerator BlinkAndDisappear()
    {
        for (int i = 0; i < 2; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(0.2f);
            rend.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }
}