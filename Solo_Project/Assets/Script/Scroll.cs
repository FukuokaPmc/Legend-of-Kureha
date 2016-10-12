using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {
    private Transform player;
    private float playerMove;
    private float lastPlPos;
	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").transform;
        lastPlPos = player.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos;
        if(lastPlPos <= player.position.z)
        {
            pos = player.position;
            pos.z += -6;
            this.transform.position = pos;
            lastPlPos = player.position.z;
        }
    }
}
