using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    static float Second;
    static float Min;
    private bool StartFlag;
    static public float[] HighScore;
    static bool Load;
	// Use this for initialization
	void Start () {
        if (!Load)
        {
            if (!PlayerPrefs.HasKey("HiScore 1-1"))
            {
                HighScore = new float[6];
                HighScore[0] = 2f;
                HighScore[1] = 0f;
                HighScore[2] = 3f;
                HighScore[3] = 30f;
                HighScore[4] = 5f;
                HighScore[5] = 50.6f;
                
            }
            else
            {
                HighScore[0] = PlayerPrefs.GetFloat("HiScore 1-1");
                HighScore[1] = PlayerPrefs.GetFloat("HiScore 1-2");
                HighScore[2] = PlayerPrefs.GetFloat("HiScore 2-1");
                HighScore[3] = PlayerPrefs.GetFloat("HiScore 2-2");
                HighScore[4] = PlayerPrefs.GetFloat("HiScore 3-1");
                HighScore[5] = PlayerPrefs.GetFloat("HiScore 3-2");
            }
            Load = true;
        }
        //Second = 0.0f;
        //Min = 0.0f;
        if (SceneManager.GetActiveScene().name != "Stage1")
        {
            StartFlag = false;
        }
        else
        {
            StartFlag = true;
        }

        if(SceneManager.GetActiveScene().name == "title")
        {
            Second = 0.0f;
            Min = 0.0f;

            PlayerPrefs.SetFloat("HiScore 1-1", HighScore[0]);
            PlayerPrefs.SetFloat("HiScore 1-2", HighScore[1]);
            PlayerPrefs.SetFloat("HiScore 2-1", HighScore[2]);
            PlayerPrefs.SetFloat("HiScore 2-2", HighScore[3]);
            PlayerPrefs.SetFloat("HiScore 3-1", HighScore[4]);
            PlayerPrefs.SetFloat("HiScore 3-2", HighScore[5]);
            PlayerPrefs.Save();
        }
        
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
