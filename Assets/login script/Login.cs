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
using UnityEditor.MemoryProfiler;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.EventSystems;

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

    /// <summary>
    /// 在启用时执行，初始化登录界面和数据库连接。
    /// </summary>
    void Start()
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

        // mysql连接
        sqlSer = "server = mysql.sqlpub.com;port = 3306;user = urrruruu;database = urrruruu;password = 90d7a69b35eb68d7;charset=utf8mb4";
        conn = new MySqlConnection(sqlSer);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            effectVolume.Play();
        }
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (system.currentSelectedGameObject == username.gameObject)
        //    {
        //        GameObject next = password.gameObject;
        //        system.SetSelectedGameObject(next);
        //        ////倒序
        //        //GameObject last = LastInput(system.currentSelectedGameObject);
        //        //system.SetSelectedGameObject(last);
        //    }
        //}
    }

    /// <summary>
    /// 实现用户登录功能，验证用户名和密码。
    /// </summary>
    public void LoginButton()
    {
        try
        { 
            conn.Open();
            Debug.Log("connect successful");

            string sqlQuary = "SELECT * FROM player;";

            Debug.Log(sqlQuary);

            MySqlCommand comd = new MySqlCommand(sqlQuary, conn);

            MySqlDataReader reader = comd.ExecuteReader();

            int flag = 0;

            while (reader.Read())
            {
                //通过reader获得数据库信息
                Debug.Log(reader.GetString("name"));
                Debug.Log(reader.GetString("password"));
                if (reader.GetString("name") == username.text && reader.GetString("password") == password.text)
                {
                    Debug.Log("登录成功");
                    flag = 1;
                    PlayerPrefs.SetString("username", username.text);
                    PlayerPrefs.SetInt("player_id", reader.GetInt32("player_id"));
                    Debug.Log("username: " + PlayerPrefs.GetString("username"));
                    break;
                }
                else
                {
                    Debug.Log("失败");
                }
            }
            if(flag == 1)
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
            reader.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log("Error:" + e.Message);
        }
        finally
        {
            conn.Close();
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
        } else if(i == 1)
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
    public void RegisterPage_registerButton()
    {
        // 注册功能实现
        try
        {
            conn.Open();
            Debug.Log("connect successful");

            string sqlQuary = "select * from player";

            Debug.Log(sqlQuary);

            MySqlCommand comd = new MySqlCommand(sqlQuary, conn);// 创建MySqlCommand对象，负责对数据库进行操作
            
            MySqlDataReader reader = comd.ExecuteReader();// 读取数据库

            if(usernameReg.text == "" || passwordReg.text == "")
            {
                Debug.Log("失败");

                tip.text = "用户名或密码不能为空！";
                i = 1;
                // 使名为"remind"的rawImage组件出现
                remind.gameObject.SetActive(true);
            }
            else
            {
                int flag = 0;
                while (reader.Read())
                {
                    Debug.Log(reader.GetString("name"));
                    Debug.Log(reader.GetString("password"));
                    if (reader.GetString("name") == usernameReg.text)
                    {
                        Debug.Log("失败");

                        tip.text = "该用户已存在！";

                        i = 1;
                        flag = 1;
                        // 使名为"remind"的rawImage组件出现
                        remind.gameObject.SetActive(true);
                        break;
                    }
                }
                reader.Close();
                if(flag == 0)
                {
                    Debug.Log("成功");

                    string sqlQuary2 = "insert into player(name,password,play_time,played_round,max_score,Number_of_cooperation) values ('" + usernameReg.text + "','" + passwordReg.text + "',0,0,0,0)";

                    Debug.Log(sqlQuary2);

                    MySqlCommand comd2 = new MySqlCommand(sqlQuary2, conn);// 创建MySqlCommand对象，负责对数据库进行操作

                    int rowsAffected = comd2.ExecuteNonQuery(); // 执行SQL查询  

                    Debug.Log("插入的行数：" + rowsAffected);

                    // 注册成功提示
                    tip.text = "注册成功";
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Error:" + e.Message);
        }
        finally
        {
            conn.Close();
        }
        remind.gameObject.SetActive(true);
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
