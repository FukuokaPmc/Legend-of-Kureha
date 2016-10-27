using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HiScore : MonoBehaviour {
    private float[] Second;
    private float[] Min;
    private Text[] text;
    // Use this for initialization
    void Start()
    {
        Second = new float[3];
        Min = new float[3];
        text = new Text[3];
        for (int n = 0; n < text.Length; n++)
        {
            text[n] = transform.GetChild(n).GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int n = 0; n < text.Length; n++)
        {
            text[n].text = string.Format("{3}:{0:00}.{1:00}.{2:00}", Min[n], Mathf.FloorToInt(Second[n]), Mathf.FloorToInt(Second[n] % 1 * 100), n + 1);

        }

    }

    public void GetTimer(float[] box)
    {
        int Count = 0;
        for (int n = 0; n < Second.Length; n++)
        {
            Debug.Log(box[0]);
            Min[n] = box[Count];
            Second[n] = box[Count + 1];
            Count += 2;
        }

    }
}
