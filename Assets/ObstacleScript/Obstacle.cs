using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 障碍物类，处理与玩家碰撞时的逻辑，实现障碍物的消失和闪烁效果。
/// </summary>
public class Obstacle : MonoBehaviour
{
    private Renderer rend;
    public RawImage rawImage;
    public bool flag = true;

    /// <summary>
    /// 在启用时执行，获取渲染器组件的引用。
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
            // 开始一个协程来实现障碍物的闪烁和消失效果
            StartCoroutine(BlinkAndDisappear());

            // 如果 rawImage 的 x 尺寸大于 0，则减小其尺寸
            if (rawImage.rectTransform.sizeDelta.x > 0)
                rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x - 60, rawImage.rectTransform.sizeDelta.y);

            // 设置 flag 为 false，避免重复触发
            flag = false;
        }
    }

    /// <summary>
    /// 实现障碍物闪烁并最终消失的协程逻辑。
    /// </summary>
    /// <returns>IEnumerator 类型的协程</returns>
    public IEnumerator BlinkAndDisappear()
    {
        for (int i = 0; i < 2; i++)
        {
            // 关闭渲染器，实现闪烁效果
            rend.enabled = false;
            Debug.Log("Renderer disabled");
            yield return new WaitForSeconds(0.1f);

            // 开启渲染器，还原可见状态
            rend.enabled = true;
            Debug.Log("Renderer enabled");
            yield return new WaitForSeconds(0.1f);
        }

        // 将游戏对象设为非激活状态，实现消失效果
        gameObject.SetActive(false);
    }
}
