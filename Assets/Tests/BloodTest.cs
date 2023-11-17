using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using NSubstitute;

[TestFixture]
public class BloodTests
{
    [UnityTest]
    public IEnumerator BloodDisappeardTest()
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

        // 创建血包对象
        GameObject bloodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Blood1"));

        Blood bloodScript = bloodGameObject.GetComponent<Blood>();

        // 设置 obstacleScript.rawImage 为模拟的 RawImage
        bloodScript.rawImage = rawImageComponent;

        // 移动障碍物到与玩家发生碰撞的位置
        bloodGameObject.transform.position = playerGameObject.transform.position;
        // 等待一帧以确保碰撞已经发生
        yield return new WaitForSeconds(0.5f);
        // 确保碰到障碍物时，停止速度和播放音频
        //Assert.AreEqual(playerScript.playerRB.velocity.x, 1.5f);
        //Assert.AreEqual(playerScript.playerRB.velocity.y, 1.5f);
        //Assert.IsFalse(playerScript.playerRB.velocity.y==0);
        //Assert.AreEqual(160.0f, bloodScript.rawImage.rectTransform.sizeDelta.x);
        //Assert.AreEqual(1.0f, playerScript.stopThere);
        Assert.IsTrue(playerScript.BloodAudiio.isPlaying);
        Assert.IsFalse(bloodScript.gameObject.activeSelf);

        // 清理
        UnityEngine.Object.Destroy(playerGameObject);
        UnityEngine.Object.Destroy(bloodGameObject);
        UnityEngine.Object.Destroy(rawImageObject);
        UnityEngine.Object.Destroy(rawImageComponent);
    }
}
