using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ObstacleTest
{
    [UnityTest]
    public IEnumerator ObstacleDisappearTest()
    {
        // 创建游戏对象
        GameObject playerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("player"));
        // 获取 Player 脚本组件
        player playerScript = playerGameObject.GetComponent<player>();


        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        GameObject rawImageObject = new GameObject("RawImage");
        RectTransform rectTransform = rawImageObject.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            rectTransform = rawImageObject.AddComponent<RectTransform>();
        }
        rectTransform.sizeDelta = new Vector2(100.0f, 10.0f);
        RawImage rawImageComponent = rawImageObject.AddComponent<RawImage>();

        // 创建障碍物对象
        GameObject obstacleGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Q_Obstacle"));

        Obstacle obstacleScript = obstacleGameObject.GetComponent<Obstacle>();

        // 设置 obstacleScript.rawImage 为模拟的 RawImage
        obstacleScript.rawImage = rawImageComponent;

        // 移动障碍物到与玩家发生碰撞的位置
        obstacleGameObject.transform.position = playerGameObject.transform.position;
        // 等待一帧以确保碰撞已经发生
        yield return new WaitForSeconds(0.5f);
        // 确保碰到障碍物时，消失
        Assert.IsFalse(obstacleGameObject.activeSelf);
        //Assert.IsFalse(obstacleScript.rend.enabled);

        // 清理
        UnityEngine.Object.Destroy(playerGameObject);
        UnityEngine.Object.Destroy(obstacleGameObject);
        UnityEngine.Object.Destroy(rawImageObject);
        UnityEngine.Object.Destroy(rawImageComponent);
    }


    [UnityTest]
    public IEnumerator ObstacleBlink()
    {
        // 创建游戏对象
        GameObject playerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("player"));
        // 获取 Player 脚本组件
        player playerScript = playerGameObject.GetComponent<player>();


        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        GameObject rawImageObject = new GameObject("RawImage");
        RectTransform rectTransform = rawImageObject.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            rectTransform = rawImageObject.AddComponent<RectTransform>();
        }
        rectTransform.sizeDelta = new Vector2(100.0f, 10.0f);
        RawImage rawImageComponent = rawImageObject.AddComponent<RawImage>();

        // 创建障碍物对象
        GameObject obstacleGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Q_Obstacle"));

        Obstacle obstacleScript = obstacleGameObject.GetComponent<Obstacle>();

        // 设置 obstacleScript.rawImage 为模拟的 RawImage
        obstacleScript.rawImage = rawImageComponent;

        // 移动障碍物到与玩家发生碰撞的位置
        obstacleGameObject.transform.position = playerGameObject.transform.position;

        // 使用 LogAssert.Expect 来捕获日志
        LogAssert.Expect(LogType.Log, "Renderer disabled");
        LogAssert.Expect(LogType.Log, "Renderer enabled");

        yield return new WaitForSeconds(0.5f);

        // 清理
        UnityEngine.Object.Destroy(playerGameObject);
        UnityEngine.Object.Destroy(obstacleGameObject);
        UnityEngine.Object.Destroy(rawImageObject);
        UnityEngine.Object.Destroy(rawImageComponent);
    }
}










