using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = DataManager.instance.time;
        float hours = Mathf.Floor(time/3600f);
        float minutes = Mathf.Floor(time/60f) % 60;
        float seconds = Mathf.Floor(time) % 60;

        timeText.text = hours.ToString("0") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
