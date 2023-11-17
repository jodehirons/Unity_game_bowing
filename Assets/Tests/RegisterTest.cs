using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class RegisterTest
{
    public Login loginScript;
    //注册成功样例
    private string testUsername = "3";
    private string testPassword = "3";
    //注册失败样例
    private string test1Username = "3";
    private string test1Password = "3";
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
    public IEnumerator RegisterCorrectTest()
    {
        // 设置有效凭据的用户名和密码。
        loginScript.usernameReg.text = test1Username;
        loginScript.passwordReg.text = test1Password;

        // 调用 LoginButton 方法。
        loginScript.RegisterButton();
        loginScript.RegisterPage_registerButton();

        // 等待异步数据库操作完成（如果有）。
        yield return new WaitForSeconds(1f);

        // 是否成功
        Assert.AreEqual("注册成功", loginScript.tip.text);
        Assert.IsTrue(loginScript.remind.gameObject.activeSelf);
    }

    [UnityTest]
    public IEnumerator RegisterFailedTest()
    {
        // 设置有效凭据的用户名和密码。
        loginScript.usernameReg.text = testUsername;
        loginScript.passwordReg.text = testPassword;

        // 调用 LoginButton 方法。
        loginScript.RegisterButton();
        loginScript.RegisterPage_registerButton();

        // 等待异步数据库操作完成（如果有）。
        yield return new WaitForSeconds(1f);

        // 是否成功
        Assert.AreEqual("该用户已存在！", loginScript.tip.text);
        Assert.IsTrue(loginScript.remind.gameObject.activeSelf);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up resources.
        testConnection.Close();

    }
}
