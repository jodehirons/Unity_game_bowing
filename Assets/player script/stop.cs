using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class stop : MonoBehaviour
{
    public Button stop_button;
    public Button continue_button;
    // Start is called before the first frame update
    public void stop_game()
    {
        Time.timeScale = 0;
        /*stop_button.gameObject.SetActive(false);
        continue_button.gameObject.SetActive(true);*/
    }
    public void continue_game()
    {
        Time.timeScale = 1;
        /*stop_button.gameObject.SetActive(true);
         *        continue_button.gameObject.SetActive(false);*/
    }
    public void Start()
    {
        stop_button.onClick.AddListener(stop_game);
        continue_button.onClick.AddListener(continue_game);
    }
}
