using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


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
    [Header("控制属性")]
    public float Y_Position = 20.5f;
    public float controlObstacle = 0;
    public float controlBlood = 0;
    public float controlWall = 0;
    public float controlStart = 0;

    // Start is called before the first frame update
    void Start()
    {
        map_Init();
        BackMusic= GetComponent<AudioSource>();
        BackMusic.Play();   
    }

    // Update is called once per frame
    void Update()
    {
        generateMap();
    }

  
    void generateMap()
    {
        if(playerTransform.position.y + 18 > Y_Position)
        {
            generateObject(Y_Position);
            for(float i = 0f; i < 18; i++)
            {
                Vector3 p_Left = new Vector3(-8.5f, Y_Position, 0);
                Vector3 p_Right = new Vector3(8.5f, Y_Position, 0);
                Vector3 p_Rightmid = new Vector3(7.5f, Y_Position, 0);
                Vector3 p_Leftmid = new Vector3(-7.5f, Y_Position, 0);

                Instantiate(mid, p_Leftmid, Quaternion.Euler(0, 0, 0));
                Instantiate(mid, p_Rightmid, Quaternion.Euler(0, 0, 0));
                Instantiate(LeftMap, p_Left, Quaternion.Euler(0, 0, 90));
                Instantiate(RightMap, p_Right, Quaternion.Euler(0, 0, -90));
                Y_Position++;
            }
            
        }
    }

    void generateObject(float Y)
    {
        int flag = 0;
        for (float i = Y + 1, k = 0; i < Y + 18; i += 1.5f, flag++)
        {
            
            float[] arr = {-6f,-4.5f, -3f, -1.5f, 0, 1.5f, 3f, 4.5f, 6f};
            bool[] index = new bool[9];
            int mem = 3;
            if (flag % 2 == 1) mem = 1;
            while (mem > 0)
            {
                int j = Random.Range(0, 1000)%9;
                if(adjust(index, j))
                {
                    index[j] = true;
                    mem--;
                }
            }
            for(int j = 0; j < 9; j++)
            {
                if (index[j] && k % 2 == 0)
                {
                    generateObstacle(arr[j], i);
                }
                else if(index[j] && k % 2 == 1)
                {
                    generateWall(arr[j], i);
                }
                k++;
            }
            
            if(i == Y+4 || i == Y + 13)
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
        for (int i = 0; i < 7; i++) Obstacles[i].active = false;
        for (int i = 0; i < 3; i++) Bloods[i].active = false;

        for (float i = -9.5f; i < 20.5f; i+=1)
        {
            Vector3 p_Left = new Vector3(-8.5f, i, 0);
            Vector3 p_Right = new Vector3(8.5f, i, 0);
            Vector3 p_Rightmid = new Vector3(7.5f, i, 0);
            Vector3 p_Leftmid = new Vector3(-7.5f, i, 0);

            Instantiate(mid, p_Leftmid, Quaternion.Euler(0, 0, 90));
            Instantiate(mid, p_Rightmid, Quaternion.Euler(0, 0, -90));
            Instantiate(LeftMap, p_Left, Quaternion.Euler(0,0,90));
            Instantiate(RightMap, p_Right, Quaternion.Euler(0, 0, -90));
        }
        generateObject(0);

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
        int indexa = GetIndex(Random.Range(0, 1000)%6);
        Instantiate(Obstacles[indexa], a, Quaternion.identity);
        
    }

    void generateBlood(float x, float y)
    {
        Vector3 a = new Vector3(x, y, 0);
        int indexa = Random.Range(0, 3);
        Bloods[indexa].active = true;
        Instantiate(Bloods[indexa], a, Quaternion.identity);
    }
    
    void generateWall(float x, float y)
    {
        int t = Random.Range(3, 100)%9;
        int[,] arr = new int[3, 3]; 
        for(float i = 0; i < t; )
        {
            int xi = Random.Range(0, 3);
            int yi = Random.Range(0, 3);
            if (arr[xi,yi] == 0)
            {
                arr[xi,yi] = 1;
                i++;
            }
        }

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (arr[i,j] == 0)
                {
                    float ina = i;
                    float inb = j;
                    Vector3 a = new Vector3(x-0.5f+ina/2, y-0.5f+inb/2, 0);
                    Obstacles[6].active = true;
                    Instantiate(Obstacles[6], a, Quaternion.identity);
                }
            }
        }
        
         
    }
}
