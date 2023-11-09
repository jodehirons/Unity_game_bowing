using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginClass : MonoBehaviour
{
    //进入前变量
    public TMP_InputField username, password, confirmPassword;
    public Text reminderText;
    public Button loginButton;
    //进入后变量
    public static string myUsername;
    private void Recovery()
    {
        loginButton.interactable = true;
    }
    public void Login()
    {
        //数据库地址、端口、用户名、数据库名、密码
        //string sqlSer = "server = 127.0.0.1;port = 3306;user = root;database = database;password = 2726;charset=utf8";
        string sqlSer = "server = mysql.sqlpub.com;port = 3306;user = urrruruu;database = urrruruu;password = 90d7a69b35eb68d7;charset=utf8";
        //建立连接
        MySqlConnection conn = new MySqlConnection(sqlSer);
        try
        {
            conn.Open();
            Debug.Log("------链接成功------");
            //sql语句
            string sqlQuary = "SELECT * FROM player;";
            //string sqlQuary = "SELECT * FROM test;";

            Debug.Log(sqlQuary);

            MySqlCommand comd = new MySqlCommand(sqlQuary, conn);

            MySqlDataReader reader = comd.ExecuteReader();

            while (reader.Read())
            {
                //通过reader获得数据库信息
                Debug.Log(reader.GetString("name"));
                Debug.Log(reader.GetString("password"));
                if (reader.GetString("name") == username.text && reader.GetString("password")==password.text)
                {
                    Debug.Log("登录成功");
                    SceneManager.LoadScene("SampleScene");
                }
                else
                {
                    Debug.Log("失败");
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
    }

    //public void Register()
    //{
    //    while (reader.Read())
    //    {
    //        if (password.text == confirmPassword.text)
    //        {
    //            .SetString(username.text, username.text);
    //            PlayerPrefs.SetString(username.text + "password", password.text);
    //            reminderText.text = "注册成功！";
    //        }
    //        else
    //        {
    //            reminderText.text = "两次密码输入不一致";
    //        }
    //    }
    //    else
    //    {
    //        reminderText.text = "用户已存在";
    //    }

    //}

}
