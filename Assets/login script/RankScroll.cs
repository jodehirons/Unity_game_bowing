using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

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

    //构建排行榜信息类
    [System.Serializable]
    class RankMessage
    {
        public int team_id;
        public string team_name;
        public int highest_score;
        public int player1_id;
        public int player2_id;
        public int player_id;
        public string name;
        public string password;
        public double play_time;
        public int played_round;
        public double max_score;
        public int Number_of_cooperation;
    }
    [System.Serializable]
    class MyRank
    {
        public string result;
        public List<RankMessage> data;

    }
    void OnEnable()
    {

        //调用showRank()
        StartCoroutine(showRank());


    }

    public IEnumerator showRank()
    {
        //调用PHP文件获取排行榜信息
        MyRank myRank;
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post("http://43.143.185.60/rank.php", form);
        yield return request.SendWebRequest();
        //将获取的信息转化为Json格式
        myRank = JsonUtility.FromJson<MyRank>(request.downloadHandler.text);

        //将Json格式的信息显示在排行榜上
        int i = 0;
        int sign = 0;
        if (PlayerPrefs.GetString("username") != null)
            username = PlayerPrefs.GetString("username");
        else
            username = "";


        //遍历获取的data数据
        foreach (RankMessage myMessage in myRank.data)
        {
            string a = PlayerPrefs.GetString("username");
            string b = myMessage.name;
            bool str = true;
            if (a != b)
            {
                str = false;
            }
            if (i < 10)
            {
                infos[i].n.text = myMessage.team_name;
                infos[i].s.text = myMessage.highest_score.ToString();
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
            if (str && username != "" && sign != 1)
            {
                personRank.n.text = myMessage.team_name;
                personRank.s.text = myMessage.highest_score.ToString();
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
