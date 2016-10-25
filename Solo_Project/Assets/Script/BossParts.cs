using UnityEngine;
using System.Collections;

public class BossParts : MonoBehaviour
{
    private int AuroraLast;
    private float Count;
    private bool AuroraFlag;
    // Use this for initialization
    void Start()
    {
        AuroraLast = transform.childCount;
        Count = 0;
        AuroraFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        AuroraLast = transform.childCount;
        if(AuroraLast <= 0)
        {
            Count += 1.0f * Time.deltaTime;
            if(Count >= 1.0f)
            {
                AuroraFlag = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<PlayerMove>().IsCurrentState(PlayerState.Dash))
            {
                col.gameObject.GetComponent<PlayerMove>().ChangeState(PlayerState.Refrect);
            }
        }
    }

    public bool LastAurora()
    {
        return AuroraFlag;
    }
}
