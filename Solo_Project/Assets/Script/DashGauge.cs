using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DashGauge : MonoBehaviour
{
    private Image Gauge;
    private Image Minus;
    private float Dash;
    // Use this for initialization
    void Start()
    {
        Dash = 1.0f;
        Gauge = this.transform.GetChild(1).GetComponent<Image>();
        Minus = this.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Dash -= 0.05f;
            Gauge.fillAmount = Dash;
        }

        Gauge.fillAmount = Dash;

        if (Minus.fillAmount > Dash)
        {
            Minus.fillAmount -= 0.003f;
        }
        else if(Minus.fillAmount < Dash)
        {
            Minus.fillAmount = Dash;
        }
    }

    public void DashChange(float fDash)
    {
        Dash = fDash;
    }
}
