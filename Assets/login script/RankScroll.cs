using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.EventSystems;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using TMPro;

public class RankScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform content;
    public List<Info> infos = new List<Info>();
    public TMP_Text username;
    public Info personRank;
    private Vector3 lastPosition;
    private float offset;
    private float size;
    private float index0y;

    private string sqlSer;
    private MySqlConnection conn;

    public void Start()
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
            int flag = 1;
            while(reader.Read() && i!= 10)
            {
                infos[i].n.text = reader.GetString("team_name");
                infos[i].s.text = reader.GetString("highest_score");
                infos[i].r.text = (i + 1).ToString();
                string a = username.text.ToString();
                string b = reader.GetString("name").ToString();
                bool str = true;
                for(int p = 0; p < Mathf.Min(a.Length,b.Length); p++)
                {
                    if (a[p] != b[p])
                    {
                        str = false;
                    }
                }
                Debug.Log("username:" + a + "*" + b+"*"+ a.Equals(b));
                if (str)
                {
                    Debug.Log("FOUND");
                    infos[i].n.color = Color.red;
                    infos[i].s.color = Color.red;
                    infos[i].r.color = Color.red;
                    personRank.n.text = reader.GetString("team_name");
                    personRank.s.text = reader.GetString("highest_score");
                    personRank.r.text = (i + 1).ToString();
                    flag = 0;
                }
                i++;
            }
            reader.Close();
            if (flag == 1)
            {
                personRank.s.text = "您尚未拥有游玩记录";
                personRank.n.text = "";
                personRank.r.text = "";
            }
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

    public void Awake()
    {
        size = infos[0].GetComponent<RectTransform>().rect.size.y;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        index0y = infos[0].transform.localPosition.y;
    }

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
            // scroll up
            // 使
            
            if (infos[0].transform.localPosition.y >= index0y)
            {
                Debug.Log(infos[0].transform.localPosition.y + "        " + index0y);
                infos[infos.Count - 1].transform.localPosition = infos[infos.Count - 2].transform.localPosition - Vector3.up * size;
            }

            //if (infos[0].transform.localPosition.y > index0y + size)
            //{
            //    Info tmp = infos[0];
            //    tmp.transform.localPosition = infos[infos.Count - 1].transform.localPosition - Vector3.up * size;
            //    for (int i = 1; i < infos.Count; i++)
            //    {
            //        infos[i - 1] = infos[i];
            //    }
            //    infos[infos.Count - 1] = tmp;
            //}
        }
        else if (offset < 0)
        {
            // scroll down
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
