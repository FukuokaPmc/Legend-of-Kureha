using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour {
    private int nTimer;
    private bool bStart;
    private int nCount;
    private Transform fade;
    private Scene scene;
	// Use this for initialization
	void Start () {
        bStart = false;
        nCount = 0;
        fade = this.gameObject.transform.GetChild(0);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {
            nTimer = fade.GetComponent<Fader>().Ignite(0.03f);
            bStart = true;
        }
        if(bStart)
        {
            nCount++;
            if (nCount >= nTimer)
            {
                scene = SceneManager.GetActiveScene();
                if (scene.name == "title")
                    SceneManager.LoadScene("Stage1");
                if (scene.name == "Stage1")
                    SceneManager.LoadScene("end");
                if (scene.name == "end")
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
    }
}
