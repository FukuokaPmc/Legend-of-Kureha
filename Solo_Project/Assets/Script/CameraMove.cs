using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    private Transform player;
    private float lastPlPos;
    private float lastPlayerX;
    public float posY;
    public float posZ;
    private float Speed;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        lastPlPos = player.position.z;
        lastPlayerX = player.position.x;
        Speed = 2.0f;

        posY = 3.0f;
        posZ = -6.0f;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos;
        Vector3 cameraPos;

        pos = player.position;
        //if(pos.y <= 5.0f)
            pos.y += posY;
        if (lastPlPos - 4.0f <= player.position.z)
        {
            pos.z += posZ;
            lastPlPos = player.position.z;
        }
        else
            pos.z = this.transform.position.z;

        this.transform.position = pos;


        cameraPos = player.position;
       // if (cameraPos.y <= 5.0f)
            cameraPos.y += 3.0f;
        this.transform.LookAt(cameraPos);
            

    }

}
