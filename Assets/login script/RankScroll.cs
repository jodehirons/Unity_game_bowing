using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// 处理排行榜显示的滚动功能。
/// </summary>
public class RankScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform content;
    public List<Info> infos = new List<Info>();
    private Vector3 lastPosition;
    private float offset;
    private float size;
    private float index0y;
    //public TMP_Text username;
    private string username;
    public Info personRank;
    private string sqlSer;
    private MySqlConnection conn;

    void OnEnable()
    {
        // 连接数据库
        sqlSer = "server = mysql.sqlpub.com;port = 3306;user = urrruruu;database = urrruruu;password = 90d7a69b35eb68d7;charset=utf8mb4";
        conn = new MySqlConnection(sqlSer);
        try
        {
            conn.Open();
            Debug.Log("连接成功");
            string sql = "SELECT * FROM teams t JOIN player p ON p.player_id= t.player1_id\r\nORDER BY highest_score DESC;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            int i = 0;
            int sign = 0;
            Debug.Log("rank: username : " + username);
            if (PlayerPrefs.GetString("username") != null)
                username = PlayerPrefs.GetString("username");
            else
                username = "";

            while (reader.Read())
            {
                string a = username.ToString();
                string b = reader.GetString("name").ToString();
                bool str = true;
                for (int p = 0; p < Mathf.Min(a.Length, b.Length); p++)
                {
                    if (a[p] != b[p])
                    {
                        str = false;
                    }
                }
                if(i< 10)
                {
                    infos[i].n.text = reader.GetString("team_name");
                    infos[i].s.text = reader.GetString("highest_score");
                    infos[i].r.text = (i + 1).ToString();
                    // 判断玩家是否在前十名
                    Debug.Log("username:" + a + "*" + b + "*" + a.Equals(b));
                    if (str && username != "")
                    {
                        Debug.Log("FOUND");
                        infos[i].n.color = Color.red;
                        infos[i].s.color = Color.red;
                        infos[i].r.color = Color.red;
                    }
                }
                if (str && username!="")
                {
                    personRank.n.text = reader.GetString("team_name");
                    personRank.s.text = reader.GetString("highest_score");
                    personRank.r.text = (i + 1).ToString();
                    sign = 1;
                }
                i++;
            }
            if (sign == 0 || username == "")
            {
                personRank.s.text = "您尚未拥有游玩记录";
                personRank.n.text = "";
                personRank.r.text = "";
            }
            reader.Close();
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }

    /// <summary>
    /// 初始化 RankScroll 组件。
    /// </summary>
    public void Awake()
    {
        size = infos[0].GetComponent<RectTransform>().rect.size.y;
    }

    /// <summary>
    /// 当拖动开始时调用。
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        index0y = infos[0].transform.localPosition.y;
    }

    /// <summary>
    /// 当对象被拖动时调用。
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        offset = eventData.position.y - lastPosition.y;
        lastPosition = eventData.position;

        for (int i = 0; i < infos.Count; i++)
        {
            infos[i].transform.localPosition += Vector3.up * offset;
        }

        if (offset > 0)
        {
            // 向上滚动逻辑
            if (infos[0].transform.localPosition.y >= index0y)
            {
                // 实现向上滚动的逻辑
                infos[infos.Count - 1].transform.localPosition = infos[infos.Count - 2].transform.localPosition - Vector3.up * size;
            }
        }
        else if (offset < 0)
        {
            // 向下滚动逻辑
        }
    }

    /// <summary>
    /// 当拖动结束时调用。
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 当拖动结束时的逻辑
    }
}
