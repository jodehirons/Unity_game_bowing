using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class AsyncLoginTests
{
    private List<Login> loginScripts;
    private List<MySqlConnection> testConnections;

    [SetUp]
    public void SetUp()
    {
        loginScripts = new List<Login>();
        testConnections = new List<MySqlConnection>();

        for (int i = 0; i < 5; i++) // 假设需要初始化3个Login实例
        {
            // 创建 Login 脚本的实例。
            GameObject loginGameObject = new GameObject();
            Login loginScript = loginGameObject.AddComponent<Login>();

            // 创建用于用户名和密码的 TMP_InputField 实例。
            GameObject usernameGameObject = new GameObject();
            GameObject passwordGameObject = new GameObject();
            loginScript.username = usernameGameObject.AddComponent<TMP_InputField>();
            loginScript.password = passwordGameObject.AddComponent<TMP_InputField>();

            // 创建用于用户名和密码（注册用）的 TMP_InputField 实例。
            GameObject usernameRegGameObject = new GameObject();
            GameObject passwordRegGameObject = new GameObject();
            loginScript.usernameReg = usernameRegGameObject.AddComponent<TMP_InputField>();
            loginScript.passwordReg = passwordRegGameObject.AddComponent<TMP_InputField>();

            // 创建用于提醒和注册页面的 RawImage 实例。
            GameObject remindGameObject = new GameObject();
            GameObject registerPageGameObject = new GameObject();
            loginScript.remind = remindGameObject.AddComponent<RawImage>();
            loginScript.registerPage = registerPageGameObject.AddComponent<RawImage>();

            // 创建用于数据库和设置界面。
            GameObject rankListObject = new GameObject();
            GameObject settingPageObject = new GameObject();
            loginScript.rankList = rankListObject;
            loginScript.settingPage = settingPageObject.AddComponent<RawImage>();

            // 创建用于数据库和设置界面。
            GameObject gameModePage = new GameObject();
            GameObject teamNamePage = new GameObject();
            loginScript.gameModePage = gameModePage.AddComponent<RawImage>();
            loginScript.teamNamePage = teamNamePage.AddComponent<RawImage>();

            // 创建用于提示的 TMP_Text 实例。
            GameObject tipGameObject = new GameObject();
            loginScript.tip = tipGameObject.AddComponent<TextMeshProUGUI>();  // 使用 TextMeshProUGUI 作为 TMP_Text 的子类

            // 设置提醒和注册页面的 gameObject 为不可见。
            loginScript.remind.gameObject.SetActive(false);
            loginScript.registerPage.gameObject.SetActive(false);

            loginScripts.Add(loginScript);

            // 设置测试用的 MySQL 连接。
            string connectionString = "server = mysql.sqlpub.com;port = 3306;user = urrruruu;database = urrruruu;password = 90d7a69b35eb68d7;charset=utf8mb4";
            MySqlConnection testConnection = new MySqlConnection(connectionString);
            testConnection.Open();

            testConnections.Add(testConnection);
        }
    }

    [TearDown]
    public void TearDown()
    {
        // 清理资源。
        foreach (var connection in testConnections)
        {
            connection.Close();
        }

        foreach (var loginScript in loginScripts)
        {
            Object.Destroy(loginScript.gameObject);
        }
    }

    [UnityTest]
    public IEnumerator LoginListTest()
    {
        List<Task> loginTasks = new List<Task>();

        // 同时开始多个登录
        for (int i = 0; i < loginScripts.Count; i++)
        {
            loginTasks.Add(PerformAsyncLogin(loginScripts[i], i));
            // 在每个异步登录任务完成后进行断言
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Init"));
        }
        // 使用 Task.WhenAll 等待所有登录完成
        yield return Task.WhenAll(loginTasks);

        //Assert.IsTrue(loginScripts.All(loginScript => loginScript.fla));
    }
    private async Task PerformAsyncLogin(Login loginScript, int index)
    {
        // 模拟异步登录
        loginScript.username.text = index.ToString();
        loginScript.password.text = index.ToString();

        // 调用 LoginButton 方法（假设 LoginButton 是异步的）。
        loginScript.LoginButton();
        await Task.Delay(20);
        Debug.Log("登陆dafdofoiajca");

        // 在每个异步登录任务完成后进行断言
        //Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Init"));
    }
}

