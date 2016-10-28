using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerText : MonoBehaviour {
    private float Second;
    private float Min;
    private Text text;
    // Use this for initialization
    void Start () {
        Second = 0.0f;
        Min = 0.0f;
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("{0:00}.{1:00}.{2:00}", Min, Mathf.FloorToInt(Second), Mathf.FloorToInt(Second % 1 * 100));

        if (SceneManager.GetActiveScene().name == "Result")
        {
            transform.localPosition = new Vector3(0, -30, 0);
        }
        else
        {
            transform.localPosition = new Vector3(438, 250, 0);
        }

    }

    public void GetTimer(float min,float Sec)
    {
        Second = Sec;
        Min = min;

    }
}
