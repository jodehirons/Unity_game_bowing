using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.SceneManagement;
using System.Data;
using UnityEngine.Networking;

public class UIinGame : MonoBehaviour
{
    public GameObject ranklist;
    public GameObject reminder;
    public GameObject teamamatePage;
    public TMP_Text reminderText;
    public TMP_InputField teammateText;
    private string sqlSer;
    private MySqlConnection conn;
    public GameObject turorialPage;
    public GameObject settingsPage;
    [System.Serializable]
    class MyMessage
    {
        public string result;
        public string message;
    }
    // Start is called before the first frame update
    void Start()
    {
        ranklist.SetActive(false);
        reminder.SetActive(false);
        teamamatePage.SetActive(false);
        turorialPage.SetActive(false);
        settingsPage.SetActive(false);
        sqlSer = "server = mysql.sqlpub.com;port = 3306;user = urrruruu;database = urrruruu;password = 90d7a69b35eb68d7;charset=utf8mb4";
        conn = new MySqlConnection(sqlSer);
}

    // Update is called once per frame
    void Update()
    {
    }

    public void rankShow()
    {
        if(ranklist.activeSelf == true)
        {
            ranklist.SetActive(false);
            return;
        }
        else
        {
            ranklist.SetActive(true);
        }
    }


    public void backForReminder()
    {
        reminder.SetActive(false);
    }

    public void pushScore()
    {
        if (teamamatePage.activeSelf == true)
        {
            teamamatePage.SetActive(false);
            return;
        }
        else
        {
            teamamatePage.SetActive(true);
        }
    }

    public void withPasserby()
    {
        StartCoroutine(SendupdataRequest());
    }

    public IEnumerator SendupdataRequest()
    {
        string url = "http://43.143.185.60/updatascore.php";

        WWWForm form = new WWWForm();
        form.AddField("teamname", PlayerPrefs.GetString("teamname"));
        form.AddField("username", PlayerPrefs.GetString("username"));
        form.AddField("score", PlayerPrefs.GetInt("score"));
        form.AddField("player_id", PlayerPrefs.GetInt("player_id"));
        form.AddField("team_id", PlayerPrefs.GetInt("team_id"));

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            MyMessage updataData = JsonUtility.FromJson<MyMessage>(response);
            Debug.Log(response);
            // 处理服务器返回的 JSON 格式的响应
            if (updataData.message == "上传成功")
            {
                reminder.SetActive(true);
                reminderText.text = "上传成功";
                teamamatePage.SetActive(false);
            }
            else if(updataData.message == "分数不如之前")
            {
                reminder.SetActive(true);
                reminderText.text = "分数不如之前";
                teamamatePage.SetActive(false);
            }
            else if (updataData.message == "上传失败，队名与玩家使用过")
            {
                reminder.SetActive(true);
                reminderText.text = "上传失败，队名与玩家使用过";
            }
            else if (updataData.message == "新的队伍")
            {
                reminder.SetActive(true);
                reminderText.text = "上传成功";
                teamamatePage.SetActive(false);
                Debug.Log("新的队伍");
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    public void withPlayer2()
    {
        if (teammateText.text == "")
        {
            reminder.SetActive(true);
            reminderText.text = "打点字";
            return;
        }
        StartCoroutine(SendupdataPlayerRequest());
    }
    public IEnumerator SendupdataPlayerRequest()
    {
        string url = "http://43.143.185.60/updatawithplayer.php";
        WWWForm form = new WWWForm();
        form.AddField("teammateName", teammateText.text); // 替换为要注册的队友的名字
        form.AddField("teamname", PlayerPrefs.GetString("teamname"));
        form.AddField("player_id", PlayerPrefs.GetInt("player_id"));
        form.AddField("score", PlayerPrefs.GetInt("score"));
        form.AddField("username", PlayerPrefs.GetString("username"));

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log(response); // 输出服务器返回的响应
            if (response == "Player1已与路人使用过")
            {
                Debug.Log("Player1已与路人使用过");
                reminder.SetActive(true);
                reminderText.text = "上传失败，队名与路人使用过，别太风流";
            }
            else if (response == "更新分数成功")
            {
                Debug.Log("更新分数成功");
                reminder.SetActive(true);
                reminderText.text = "上传成功";
                teamamatePage.SetActive(false);
            }
            else if (response == "分数未更新")
            {
                Debug.Log("分数未更新");
                reminder.SetActive(true);
                reminderText.text = "分不如之前";
                teamamatePage.SetActive(false);
            }
            else if (response == "上传失败，队名与其他玩家使用过")
            {
                reminder.SetActive(true);
                reminderText.text = "上传失败，队名与其他玩家使用过，注意道德";
            }
            else if (response == "上传成功")
            {
                reminder.SetActive(true);
                reminderText.text = "上传成功";
                teamamatePage.SetActive(false);
            }
            else if (response == "莫有这人捏")
            {
                reminder.SetActive(true);
                reminderText.text = "莫有这人捏";
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            // 处理错误
        }
    }
    /// <summary>
    ///  设置界面展示
    /// </summary>
    public void SettingsPage()
    {
        if (settingsPage.activeSelf == true)
        {
            settingsPage.SetActive(false);
            return;
        }
        else
        {
            settingsPage.SetActive(true);
        }
    }
    /// <summary>
    /// 教程界面展示
    /// <summary>
    public void TutorialPage()
    {
        if (turorialPage.activeSelf == true)
        {
            turorialPage.SetActive(false);
            return;
        }
        else
        {
            turorialPage.SetActive(true);
        }
    }
}
