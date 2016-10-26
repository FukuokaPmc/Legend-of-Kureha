using UnityEngine;
using System.Collections;

public class Check : MonoBehaviour {
    private GameObject Player;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        // if (collision.gameObject.tag)
        Debug.Log("滅びよ");
        if (col.gameObject.tag == "Enemy")
        {
            col.GetComponent<EnemyMove>().Dead();
            Player.GetComponent<PlayerMove>().SightOff();
        }
    }
}
