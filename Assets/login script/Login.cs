using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;
using UnityEngine.Analytics;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

/// <summary>
/// 登录类，处理登录和注册功能，管理数据库连接等。
/// </summary>
public class Login : MonoBehaviour
{
    // 登录账号与密码
    public TMP_InputField username;
    public TMP_InputField password;
    // 注册账号与密码
    public TMP_InputField usernameReg;
    public TMP_InputField passwordReg;
    // 所需跳转页面
    public RawImage remind; // 提醒界面
    public RawImage registerPage; // 注册界面
    public TMP_Text tip;// 提醒界面展示文字
    // mysql
    private string sqlSer;
    private MySqlConnection conn;
    // 控制返回层数
    private int i = 2;
    // 音乐控制
    public AudioSource audioSource;
    public AudioSource effectVolume;
    private bool isPlaying = true;
    // 数据库界面
    public GameObject rankList;
    private bool isData = false;
    // 设置界面
    public RawImage settingPage;
    private bool isSetting = false;
    // 事件系统
    EventSystem system;
    // 游戏模式选择界面
    public RawImage gameModePage;
    // 队名输入界面
    public RawImage teamNamePage;
    public TMP_InputField teamName;
    // 教程界面
    public RawImage turorialPage;

    [System.Serializable]
    class MyMessage
    {
        public string result;
        public string message;
    }
    //建立一个玩家信息类用于获取玩家的信息
    [System.Serializable]
    class PlayerMessage
    {
        public int player_id;
        public string name;
        public string password;
        public double play_time;
        public int played_round;
        public double max_score;
        public int Number_of_cooperation;

    }
    //建立登录信息类用于获取PHP传来的json数据
    [System.Serializable]
    class LoginMessage
    {
        public string result;
        public PlayerMessage data;
    }
    /// <summary>
    /// 在启用时执行，初始化登录界面和数据库连接。
    /// </summary>
    void OnEnable()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("username", username.text);
        remind.gameObject.SetActive(false); // 提醒界面隐藏
        registerPage.gameObject.SetActive(false); // 注册界面隐藏
        rankList.SetActive(false); // 数据库界面隐藏
        settingPage.gameObject.SetActive(false); // 设置界面隐藏
        gameModePage.gameObject.SetActive(false); // 游戏模式选择界面隐藏
        teamNamePage.gameObject.SetActive(false); // 队名输入界面隐
        turorialPage.gameObject.SetActive(false); // 教程页面隐藏
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            effectVolume.Play();
        }


    }

    //如果loginButton被按下，则执行LoginButton函数    
    public void OnLoginButtonClick()
    {
        StartCoroutine(LoginButton());
    }

    /// <summary>
    /// 实现用户登录功能，验证用户名和密码。
    /// </summary>
    public IEnumerator LoginButton()
    {

        int flag = 0;
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);
        // 创建HTTP请求


        UnityWebRequest request = UnityWebRequest.Post("http://43.143.185.60/login.php", form);
        //设置请求头
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 获取PHP传入的内容
            string response = request.downloadHandler.text;
            // 处理服务器返回的 JSON 格式的响应
            // 将response作为json格式读出，然后转换为字典格式
            LoginMessage loginData = JsonUtility.FromJson<LoginMessage>(response);
            if (loginData.result == "success")
            {
                Debug.Log("登录成功");
                flag = 1;

                // Handle successful login
                PlayerPrefs.SetString("username", loginData.data.name);
                PlayerPrefs.SetInt("player_id", loginData.data.player_id);
            }
            else
            {
                Debug.Log("Login failed");
                // Handle failed login
            }
        }
        if (flag == 1)
        {
            // 跳转到游戏模式选择界面
            gameModePage.gameObject.SetActive(true);
        }
        else
        {
            // 让remindText的文本内容为"用户名或密码错误"
            tip.text = "用户名或密码错误";
            i = 2;
            // 使名为"remind"的rawImage组件出现
            remind.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 控制返回界面逻辑，隐藏提醒或注册界面。
    /// </summary>
    public void BackButton()
    {
        turorialPage.gameObject.SetActive(false);
        // 使名为"remind"的rawImage组件不可见
        if (i == 2)
        {
            remind.gameObject.SetActive(false);
            registerPage.gameObject.SetActive(false);
        }
        else if (i == 1)
        {
            remind.gameObject.SetActive(false);
        }
        i = 2;
    }
    /// <summary>
    /// 控制跳转至注册界面逻辑。
    /// </summary>
    public void RegisterButton()
    {
        // 使注册界面出现
        registerPage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 实现注册功能，验证用户输入并向数据库插入新用户信息。
    /// </summary>
    public IEnumerator RegisterPage_registerButton()
    {
        // 注册功能实现

        if (usernameReg.text == "" || passwordReg.text == "")
        {
            Debug.Log("失败");

            tip.text = "用户名或密码不能为空！";
            i = 1;
            // 使名为"remind"的rawImage组件出现
            remind.gameObject.SetActive(true);
        }
        else
        {
            string url = "http://43.143.185.60/register.php";

            WWWForm form = new WWWForm();
            form.AddField("usernameReg", usernameReg.text);
            form.AddField("passwordReg", passwordReg.text);

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                //将response作为json格式读出，然后转换为字典格式

                string response = request.downloadHandler.text;
                // 处理服务器返回的 JSON 格式的响应
                // 将response作为json格式读出，然后转换为字典格式
                MyMessage responsedata = JsonUtility.FromJson<MyMessage>(response);

                //使用返回的json判断读取数据是否成功
                if (responsedata.result == "success")
                {
                    tip.text = "注册成功";
                }
                else
                {
                    Debug.Log("失败");

                    tip.text = "该用户已存在！";
                    i = 1;
                    // 使名为"remind"的rawImage组件出现
                    remind.gameObject.SetActive(true);
                }

            }
            else
            {
                Debug.Log("失败");

                tip.text = "服务器访问失败！";
                i = 1;
                // 使名为"remind"的rawImage组件出现
                remind.gameObject.SetActive(true);
            }

        }
        remind.gameObject.SetActive(true);
    }
    /// <summary>
    /// 控制注册按钮逻辑，跳转至注册功能。
    /// </summary>
    public void Register()
    {
        StartCoroutine(RegisterPage_registerButton());
    }

    /// <summary>
    /// 控制音乐播放状态。
    /// </summary>
    public void MusicControl()
    {
        // 切换播放状态
        if (isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }

        // 更新播放状态
        isPlaying = !isPlaying;
    }

    /// <summary>
    /// 控制数据库界面显示状态。
    /// </summary>
    public void RankListControl()
    {
        // 更新排行榜展示状态
        isData = !isData;

        // 切换排行榜展示状态
        rankList.SetActive(isData);
    }

    /// <summary>
    /// 控制设置界面显示状态。
    /// </summary>
    public void SettingControl()
    {
        // 更新设置界面展示状态
        isSetting = !isSetting;

        // 切换设置界面展示状态
        settingPage.gameObject.SetActive(isSetting);
    }

    /// <summary>
    /// 控制游戏模式跳转至队名输入界面。
    /// </summary>
    public void GameModeToTeamName()
    {
        // 使游戏模式选择界面不可见
        gameModePage.gameObject.SetActive(false);
        // 使队名输入界面可见
        teamNamePage.gameObject.SetActive(true);

    }

    /// <summary>
    /// 控制队名输入界面跳转至游戏场景。
    /// </summary>
    public void TeamNameToGame()
    {
        if (teamName.text == "")
        {
            teamName.text = "队伍" + UnityEngine.Random.Range(0, 1000);
        }
        // 使队名输入界面不可见
        teamNamePage.gameObject.SetActive(false);
        // 跳转到游戏场景
        PlayerPrefs.SetString("teamname", teamName.text);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // 以下为教程界面跳转逻辑
    public void TutorialToGame()
    {
        // 使教程界面可见
        turorialPage.gameObject.SetActive(true);
    }
}
