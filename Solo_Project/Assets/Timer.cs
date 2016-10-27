﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    private float Second;
    private float Min;
    private bool StartFlag;
    private float[] HighScore;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);

        Second = 0.0f;
        Min = 0.0f;
        StartFlag = false;

        HighScore = new float[6];
        HighScore[0] = 2f;
        HighScore[1] = 0f;
        HighScore[2] = 3f;
        HighScore[3] = 30f;
        HighScore[4] = 5f;
        HighScore[5] = 50.6f;
    }
	
	// Update is called once per frame
	void Update () {
	    if(StartFlag)
        {
            Second += 1.0f * Time.unscaledDeltaTime;
            if(Second > 60)
            {
                Min++;
                Second = 0;
            }
        }


        if(SceneManager.GetActiveScene().name != "title" && SceneManager.GetActiveScene().name != "end")
            GameObject.Find("TimerText").GetComponent<TimerText>().GetTimer(Min, Second);

        if(SceneManager.GetActiveScene().name == "title")
        {
            GameObject.Find("HiScore").GetComponent<HiScore>().GetTimer(HighScore);
        }

    }

    public void StartTimer()
    {
        StartFlag = true;
    }

    public void StopTimer()
    {
        StartFlag = false;
    }

    public void ResetTimer()
    {
        Second = 0.0f;
        Min = 0.0f;
    }

    public void CheckHiScore()
    {
        bool bScore = false;
        for(int n = 0; n < HighScore.Length; n+=2)
        {
            if (!bScore)
            {
                if (HighScore[n] > Min)
                {
                    for (int m = HighScore.Length - 2; m > n; m -= 2)
                    {
                        HighScore[m] = HighScore[m - 2];
                        HighScore[m + 1] = HighScore[m - 1];
                    }

                    HighScore[n] = Min;
                    HighScore[n + 1] = Second;
                    bScore = true;
                }
                else if (HighScore[n] == Min && HighScore[n + 1] > Second)
                {
                    for (int m = HighScore.Length - 2; m > n; m -= 2)
                    {
                        HighScore[m] = HighScore[m - 2];
                        HighScore[m + 1] = HighScore[m - 1];
                    }
                    HighScore[n] = Min;
                    HighScore[n + 1] = Second;
                    bScore = true;
                }
            }
        }
    }
}
