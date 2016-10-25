using UnityEngine;
using System.Collections;

public class BossParts : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
}
