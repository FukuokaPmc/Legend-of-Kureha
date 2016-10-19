using UnityEngine;
using System.Collections;

public class Trail : MonoBehaviour {
    private bool bTimer;
    private float nCount;
	// Use this for initialization
	void Start () {
        bTimer = false;
        nCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (bTimer)
        {
            nCount += 1.0f * Time.deltaTime;
            if(nCount >= 0.5f)
            {
                this.gameObject.SetActive(false);
                bTimer = false;
                nCount = 0;
            }
        }
	}

    public void EraseTimer()
    {
        bTimer = true;
    }

    public void TimerStop()
    {
        bTimer = false;
        nCount = 0;
    }
}
