using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFlash : MonoBehaviour {
    private float Speed;
    private Text text;
    private Color color;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        color = text.color;
        Speed = -1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        color.a += Speed * Time.deltaTime;
        text.color = color;

        if(color.a < 0 || color.a > 1.0f)
        {
            Speed *= -1;
        }

    }
}
