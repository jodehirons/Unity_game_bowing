using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血液类，处理与玩家碰撞时的逻辑，用于血量控制。
/// </summary>
public class Blood : MonoBehaviour
{
    private Renderer rend;
    public RawImage rawImage;
    public bool flag = true;

    /// <summary>
    /// 在启用时执行，获取组件引用。
    /// </summary>
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    /// <summary>
    /// 当与其他物体发生碰撞时触发，处理碰撞逻辑。
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && flag)
        {
            // 碰撞后禁用当前游戏对象
            gameObject.SetActive(false);

            // 如果血液图像的 x 尺寸小于 600，则增加其尺寸
            if (rawImage.rectTransform.sizeDelta.x < 600)
            {
                rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x + 60, rawImage.rectTransform.sizeDelta.y);
            }

            flag = false;
        }
    }
}
