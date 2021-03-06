﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour {
    private int nTimer;
    private bool bStart;
    private int nCount;
    private Transform fade;
    private Scene scene;
    private Timer timer;
    private bool bGameOver;
	// Use this for initialization
	void Start () {
        bStart = false;
        nCount = 0;
        fade = this.gameObject.transform.GetChild(0);
        if (scene.name != "end")
        {
            timer = GameObject.Find("Timer").GetComponent<Timer>();
        }

        bGameOver = false;
    }
	
	// Update is called once per frame
	void Update () {
        scene = SceneManager.GetActiveScene();
        if (Input.anyKeyDown && scene.name != "Stage1")
        {
            nTimer = fade.GetComponent<Fader>().Ignite(0.03f);
            bStart = true;
        }
        if(bStart)
        {
            nCount++;
            if (nCount >= nTimer)
            {
                
                if (scene.name == "title")
                {
                    SceneManager.LoadScene("Stage1");
                }
                /* if (scene.name == "Stage1")
                     SceneManager.LoadScene("end");
                 if (scene.name == "end")
                     SceneManager.LoadScene("title");*/
                if (scene.name == "Stage1")
                {
                    if (bGameOver)
                    {
                        SceneManager.LoadScene("end");
                    }
                    else
                    {
                        SceneManager.LoadScene("Result");
                    }
                }
                if (scene.name == "Result" || scene.name == "end")
                    SceneManager.LoadScene("title");
                if (scene.name == "BossTest")
                    SceneManager.LoadScene("end");
                
            }
                
        }
	}

    public void SceneChange()
    {
        nTimer = fade.GetComponent<Fader>().Ignite(0.03f);
        bStart = true;
        bGameOver = false;
    }

    public void GameOver()
    {
        nTimer = fade.GetComponent<Fader>().Ignite(0.03f);
        bStart = true;
        bGameOver = true;
    }
}
