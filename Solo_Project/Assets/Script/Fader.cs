using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    private float fSpeed;
    private bool bCheck;
    private Image image;
    private float fFade;

	// Use this for initialization
	void Start() {
        fSpeed = 0.03f;
        bCheck = false;
        image = GetComponent<Image>();
        fFade = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (bCheck) 
        {
            image.color = new Color(image.color.r, image.color.g,image.color.b, fFade);
            fFade += fSpeed;
        }
	}

    public int Ignite(float speed)
    {
        fSpeed = speed;
        bCheck = true;

        return (int)(255 / (speed * 255));
    }
}
