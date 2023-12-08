using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MapGenerate : MonoBehaviour
{
    [Header("外部属性")]
    public Transform playerTransform;
    public GameObject LeftMap;
    public GameObject RightMap;
    public GameObject mid;
    public GameObject[] Obstacles;
    public GameObject[] Bloods;
    public AudioSource BackMusic;
    public GameObject Valley;
    public GameObject Background;
    [Header("控制属性")]
    public float Y_Position = 20.5f;
    public int controlValley;
    public float Y_Back;

    public Slider mainSlider;
    public Slider bgmSlider;

    // Start is called before the first frame update
    void Start()
    {
        map_Init();
        BackMusic = GetComponent<AudioSource>();
        BackMusic.Play();
        Y_Back = 5;
    }

    // Update is called once per frame
    void Update()
    {
        generateMap();

        // 根据滑动条的值来设置音量
        if (bgmSlider != null)
        {
            BackMusic.volume = mainSlider.value * bgmSlider.value;
        }
    }


    void generateMap()
    {
        if (playerTransform.position.y + 20 > Y_Back)
        {
            Vector3 a = new Vector3(0, Y_Back, 0);
            Instantiate(Background, a, Quaternion.identity);
            Y_Back += 10;
        }
        if (playerTransform.position.y + 18 > Y_Position)
        {
            controlValley++;
            if (controlValley % 3 == 2)
            {
                Vector3 a = new Vector3(0, Y_Position + 9, 0);
                Instantiate(Valley, a, Quaternion.identity);
                generateObject(Y_Position, 1);
            }
            else if (controlValley % 5 == 1)
            {
                float now_Y = Y_Position + 10;
                for (float i = -6.5f; i <= 6.5f; i += 0.5f)
                {
                    if (i == 3)
                    {
                        Vector3 a = new Vector3(i + 1f, now_Y, 0);
                        i += 2f;
                        Bloods[4].active = true;
                        Instantiate(Bloods[4], a, Quaternion.identity);
                    }
                    else if (i == -3)
                    {
                        Vector3 a = new Vector3(i + 1f, now_Y, 0);
                        i += 2f;
                        Bloods[5].active = true;
                        Instantiate(Bloods[5], a, Quaternion.identity);
                    }
                    else
                    {
                        Obstacles[7].active = true;
                        Vector3 a = new Vector3(i, now_Y, 0);
                        Vector3 b = new Vector3(i, now_Y + 0.5f, 0);
                        Vector3 c = new Vector3(i, now_Y - 0.5f, 0);
                        Instantiate(Obstacles[7], a, Quaternion.identity);
                        Instantiate(Obstacles[7], b, Quaternion.identity);
                        Instantiate(Obstacles[7], c, Quaternion.identity);
                    }
                }

                generateObject(Y_Position, 2);
            }
            else generateObject(Y_Position, 0);
            for (int i = 0; i < 18; i++)
            {
                Vector3 p_Left = new Vector3(-8.5f, Y_Position, 0);
                Vector3 p_Right = new Vector3(8.5f, Y_Position, 0);
                Vector3 p_Rightmid = new Vector3(7.5f, Y_Position, 0);
                Vector3 p_Leftmid = new Vector3(-7.5f, Y_Position, 0);
                if (i % 5 == 0)
                {
                    Instantiate(Obstacles[Random.Range(0, 3) + 8], new Vector3(-8f, Y_Position, 0), Quaternion.Euler(0, 0, 0));
                    Instantiate(Obstacles[Random.Range(0, 3) + 11], new Vector3(8f, Y_Position, 0), Quaternion.Euler(0, 0, 0));
                }
                Instantiate(mid, p_Leftmid, Quaternion.Euler(0, 0, 0));
                Instantiate(mid, p_Rightmid, Quaternion.Euler(0, 0, 0));
                Instantiate(LeftMap, p_Left, Quaternion.Euler(0, 0, 90));
                Instantiate(RightMap, p_Right, Quaternion.Euler(0, 0, -90));
                Y_Position++;
            }

        }
    }

    void generateObject(float Y, float gg)
    {
        int flag = 0;
        for (float i = Y + 1, k = 0; i < Y + 19; i += 2f, flag++)
        {
            if ((gg == 2 || gg == 1) && i > Y + 7)
            {
                i += 4f;
                gg = 0;
            }
            float[] arr = { -6f, -4.5f, -3f, -1.5f, 0, 1.5f, 3f, 4.5f, 6f };
            bool[] index = new bool[9];
            int mem = 3;
            if (flag % 2 == 1) mem = 1;
            while (mem > 0)
            {
                int j = Random.Range(0, 1000) % 9;
                if (adjust(index, j))
                {
                    index[j] = true;
                    mem--;
                }
            }
            for (int j = 0; j < 9; j++)
            {
                if (index[j] && k % 2 == 0)
                {
                    generateObstacle(arr[j], i);
                }
                else if (index[j] && k % 2 == 1)
                {
                    generateWall(arr[j], i);
                }
                k++;
            }

            if (i == Y + 4 || i == Y + 13)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!index[j])
                    {
                        generateBlood(arr[j], i);
                        break;
                    }
                }
            }

        }

    }

    bool adjust(bool[] a, int b)
    {
        if (a[b]) return false;
        return true;
    }

    void map_Init()
    {
        for (int i = 0; i < 8; i++) Obstacles[i].active = false;
        for (int i = 0; i < 6; i++) Bloods[i].active = false;

        for (float i = -9.5f; i < 20.5f; i += 1)
        {
            Vector3 p_Left = new Vector3(-8.5f, i, 0);
            Vector3 p_Right = new Vector3(8.5f, i, 0);
            Vector3 p_Rightmid = new Vector3(7.5f, i, 0);
            Vector3 p_Leftmid = new Vector3(-7.5f, i, 0);
            if ((int)i % 6 == 0)
            {
                Instantiate(Obstacles[Random.Range(0, 3) + 8], new Vector3(-8f, i, 0), Quaternion.Euler(0, 0, 0));
                Instantiate(Obstacles[Random.Range(0, 3) + 11], new Vector3(8f, i, 0), Quaternion.Euler(0, 0, 0));
            }
            Instantiate(mid, p_Leftmid, Quaternion.Euler(0, 0, 90));
            Instantiate(mid, p_Rightmid, Quaternion.Euler(0, 0, -90));
            Instantiate(LeftMap, p_Left, Quaternion.Euler(0, 0, 90));
            Instantiate(RightMap, p_Right, Quaternion.Euler(0, 0, -90));
        }
        generateObject(0, 0);

    }

    int GetIndex(int x)
    {
        if (Obstacles[x].active) return x;
        Obstacles[x].active = true;
        return x;
    }

    void generateObstacle(float x, float y)
    {

        Vector3 a = new Vector3(x, y, 0);
        int indexa = GetIndex(Random.Range(0, 1000) % 7);
        Instantiate(Obstacles[indexa], a, Quaternion.identity);

    }

    void generateBlood(float x, float y)
    {
        Vector3 a = new Vector3(x, y, 0);
        int indexa = Random.Range(0, 100) % 4;
        Bloods[indexa].active = true;
        Instantiate(Bloods[indexa], a, Quaternion.identity);
    }

    void generateWall(float x, float y)
    {
        int t = Random.Range(3, 100) % 9;
        int[,] arr = new int[3, 3];
        for (float i = 0; i < t;)
        {
            int xi = Random.Range(0, 3);
            int yi = Random.Range(0, 3);
            if (arr[xi, yi] == 0)
            {
                arr[xi, yi] = 1;
                i++;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (arr[i, j] == 0)
                {
                    float ina = i;
                    float inb = j;
                    Vector3 a = new Vector3(x - 0.5f + ina / 2, y - 0.5f + inb / 2, 0);
                    Obstacles[7].active = true;
                    Instantiate(Obstacles[7], a, Quaternion.identity);
                }
            }
        }


    }
}
