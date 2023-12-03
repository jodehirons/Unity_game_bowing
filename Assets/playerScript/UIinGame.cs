using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.SceneManagement;
using System.Data;

public class UIinGame : MonoBehaviour
{
    public GameObject ranklist;
    public GameObject reminder;
    public GameObject teamamatePage;
    public TMP_Text reminderText;
    public TMP_InputField teammateText;
    private string sqlSer;
    private MySqlConnection conn;

    // Start is called before the first frame update
    void Start()
    {
        ranklist.SetActive(false);
        reminder.SetActive(false);
        teamamatePage.SetActive(false);

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
        teamamatePage.SetActive(true);
    }

    public void withPasserby()
    {
        try
        {
            conn.Open();
            Debug.Log("连接成功");
            // 寻找player1使用过该队名记录
            string sql = "select * from player p \r\n" + 
                        "join teams t on p.player_id = t.player1_id or p.player_id = t.player2_id \r\n"+ 
                        " where team_name = '"+ PlayerPrefs.GetString("teamname") +"' and name = '"+  PlayerPrefs.GetString("username") +"'\r\n"+
                        " order by highest_score desc;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Player1已使用过该队名，且也是与路人
                if (reader.IsDBNull("player2_id"))
                {
                    Debug.Log("Player1已使用过该队名，且也是与路人");
                    // 比较分数
                    int max_score = reader.GetInt32("highest_score");
                    reader.Close();
                    if (max_score < PlayerPrefs.GetInt("score"))
                    {
                        // 更新分数
                        sql = "update teams set highest_score = " + PlayerPrefs.GetInt("score") + " where team_name = '" + PlayerPrefs.GetString("teamname") + "';";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        Debug.Log("更新分数成功");
                        reminder.SetActive(true);
                        reminderText.text = "上传成功";
                        teamamatePage.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("分数未更新");
                        reminder.SetActive(true);
                        reminderText.text = "分不如之前";
                        teamamatePage.SetActive(false);
                    }
                    // 插入新纪录到rank_record
                    sql = "insert into rank_record (player_id,score,upload_time,team_id) values (" + PlayerPrefs.GetInt("player_id") + ", " + PlayerPrefs.GetInt("score") + ", '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + PlayerPrefs.GetInt("team_id") + ");";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    Debug.Log("Player1已使用过该队名，但不是与路人");
                    reminder.SetActive(true);
                    reminderText.text = "上传失败，队名与玩家使用过";
                }
            }
            else
            {
                Debug.Log("Player1未使用过该队名");
                // 插入新纪录
                reader.Close();
                sql = "insert into teams (team_name, player1_id, player2_id, highest_score) values ('" + PlayerPrefs.GetString("teamname") + "', " + PlayerPrefs.GetInt("player_id") + ", null, " + PlayerPrefs.GetInt("score") + ");";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                // 获取team_id
                sql = "select * from teams where team_name = '" + PlayerPrefs.GetString("teamname") + "' and player1_id = " + PlayerPrefs.GetInt("player_id")  + ";";
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                reader.Read();
                PlayerPrefs.SetInt("team_id", reader.GetInt32("team_id"));
                reader.Close();
                // 插入新纪录到rank_record
                sql = "insert into rank_record (player_id,score,upload_time,team_id) values (" + PlayerPrefs.GetInt("player_id") + ", " + PlayerPrefs.GetInt("score") + ", '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + PlayerPrefs.GetInt("team_id") + ");";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                reminder.SetActive(true);
                reminderText.text = "上传成功";
                teamamatePage.SetActive(false);
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
        }
        finally
       {
            conn.Close();
            teamamatePage.SetActive(false);
       }
    }

    public void withPlayer2()
    {
        try
        {
            conn.Open();
            Debug.Log("连接成功");
            if (teammateText.text == "")
            {
                reminder.SetActive(true);
                reminderText.text = "打点字";
                return;
            }
            string sql = "select * from player where name = '" + teammateText.text + "';";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int player2_id = reader.GetInt32("player_id");
                reader.Close();
                // 寻找player1使用过该队名记录
                sql = "select * from player p \r\n" +
                            "join teams t on p.player_id = t.player1_id or p.player_id = t.player2_id \r\n" +
                            " where team_name = '" + PlayerPrefs.GetString("teamname") + "' and name = '" + PlayerPrefs.GetString("username") + "'\r\n" +
                            " order by highest_score desc;";
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                // Player1已使用过该队名，且与路人
                while (reader.Read())
                {
                    if (reader.IsDBNull("player2_id") || reader.IsDBNull("player1_id"))
                    {
                        Debug.Log("Player1已与路人使用过");
                        reminder.SetActive(true);
                        reminderText.text = "上传失败，队名与路人使用过，别太风流";
                        return;
                    }
                    // 与player2一起玩过
                    else if (reader.GetInt32("player2_id") == player2_id || reader.GetInt32("player1_id") == player2_id)
                    {
                        Debug.Log("Player1已与player2组队过");
                        // 比较分数
                        int max_score = reader.GetInt32("highest_score");
                        reader.Close();
                        // 插入新纪录到rank_record
                        sql = "insert into rank_record (player_id,score,upload_time,team_id) values (" + PlayerPrefs.GetInt("player_id") + ", " + PlayerPrefs.GetInt("score") + ", '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + PlayerPrefs.GetInt("team_id") + ");";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        if (max_score < PlayerPrefs.GetInt("score"))
                        {
                            // 更新分数
                            sql = "update teams set highest_score = " + PlayerPrefs.GetInt("score") + " where team_name = '" + PlayerPrefs.GetString("teamname") + "';";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                            Debug.Log("更新分数成功");
                            reminder.SetActive(true);
                            reminderText.text = "上传成功";
                            teamamatePage.SetActive(false);
                            return;
                        }
                        else
                        {
                            Debug.Log("分数未更新");
                            reminder.SetActive(true);
                            reminderText.text="分不如之前";
                            teamamatePage.SetActive(false);
                            return; 
                        }
                    }
                    else
                    {
                        reminder.SetActive(true);
                        reminderText.text = "上传失败，队名与其他玩家使用过，注意道德";
                        return;
                    }
                }
                // 与player2未玩过
                // 插入新纪录到teams
                reader.Close();
                sql = "insert into teams (team_name, player1_id, player2_id, highest_score) values ('" + PlayerPrefs.GetString("teamname") + "', " + PlayerPrefs.GetInt("player_id") + ", " + player2_id + ", " + PlayerPrefs.GetInt("score") + ");";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                // 获取team_id
                sql = "select * from teams where team_name = '" + PlayerPrefs.GetString("teamname") + "' and player1_id = " + PlayerPrefs.GetInt("player_id") + " and player2_id = " + player2_id + ";";
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                reader.Read();
                PlayerPrefs.SetInt("team_id", reader.GetInt32("team_id"));
                reader.Close();
                // 插入新纪录到rank_record
                sql = "insert into rank_record (player_id,score,upload_time,team_id) values (" + PlayerPrefs.GetInt("player_id") + ", " + PlayerPrefs.GetInt("score") + ", '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + PlayerPrefs.GetInt("team_id") + ");";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                reminder.SetActive(true);
                reminderText.text = "上传成功";
                teamamatePage.SetActive(false);
            }
            else
            {
                reminder.SetActive(true);
                reminderText.text = "莫有这人捏";
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            conn.Close();
            teamamatePage.SetActive(false);
        }
    }
}
