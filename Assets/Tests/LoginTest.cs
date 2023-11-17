using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using TMPro;


public class LoginTests
{
    public Login loginScript;
    //登录成功
    private string testUsername = "1";
    private string testPassword = "1";
    //登录失败
    private string test1Username = "4";
    private string test1Password = "4";
    private MySqlConnection testConnection;

    [SetUp]
    public void SetUp()
    {
        
        // 创建 Login 脚本的实例。
        GameObject loginGameObject = new GameObject();
        loginScript = loginGameObject.AddComponent<Login>();

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

        // 设置测试用的 MySQL 连接。
        string connectionString = "server = mysql.sqlpub.com;port = 3306;user = urrruruu;database = urrruruu;password = 90d7a69b35eb68d7;charset=utf8mb4";
        testConnection = new MySqlConnection(connectionString);
        testConnection.Open();

        // 设置提醒和注册页面的 gameObject 为不可见。
        loginScript.remind.gameObject.SetActive(false);
        loginScript.registerPage.gameObject.SetActive(false);
    }

    [UnityTest]
    public IEnumerator LoginSuccessfullyTest()
    {
        // 设置有效凭据的用户名和密码。
        loginScript.username.text = testUsername;
        loginScript.password.text = testPassword;

        // 调用 LoginButton 方法。
        loginScript.LoginButton();

        // 等待异步数据库操作完成（如果有）。
        yield return new WaitForSeconds(1f);

        // 断言场景是否已更改为 "Game"。
        Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Init"));
    }

    [UnityTest]
    public IEnumerator LoginFailTest()
    {
        // 设置无效凭据的用户名和密码。
        loginScript.username.text = test1Username;
        loginScript.password.text = test1Password;

        // 调用 LoginButton 方法。
        loginScript.LoginButton();

        // 等待异步数据库操作完成（如果有）。
        yield return new WaitForSeconds(1f);

        // 断言提醒文本是否设置为 "用户名或密码错误"。
        Assert.AreEqual("用户名或密码错误", loginScript.tip.text);

        // 断言提醒对象是否处于活动状态。
        Assert.IsTrue(loginScript.remind.gameObject.activeSelf);
    }

    [UnityTest]
    public IEnumerator TestValueI()
    {
        // 设置无效凭据的用户名和密码。
        loginScript.username.text = testUsername;
        loginScript.password.text = testPassword;

        // 调用 LoginButton 方法。
        loginScript.LoginButton();

        // 获取名为 "i" 的私有字段的值。
        var iField = typeof(Login).GetField("i", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        int iValue = (int)iField.GetValue(loginScript);

        // 调用 LoginButton 方法。
        //loginScript.BackButton();

        // 断言提醒对象是否处于活动状态。
        Assert.AreEqual(iValue, 2);

        yield return null;
    }


    [TearDown]
    public void TearDown()
    {
        // Clean up resources.
        testConnection.Close();

    }
}



