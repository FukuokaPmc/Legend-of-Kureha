using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour {
    private Image Gauge;
    private Image Minus;
    private float HP;
	// Use this for initialization
	void Start () {
        HP = 1.0f;
        Gauge = this.transform.GetChild(1).GetComponent<Image>();
        Minus = this.transform.GetChild(0).GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.V))
        {
            HP -= 0.1f;
            Gauge.fillAmount = HP;
        }

        if(Minus.fillAmount > HP)
        {
            Minus.fillAmount -= 0.003f;
        }
	}
}
