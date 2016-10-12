using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    private Transform player;
    public int posY;
    public int posZ;
    private float lastPlPos;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        posY = 2;
        posZ = -6;
        lastPlPos = player.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos;

        
        pos = player.position;
        pos.y += posY;
        if (lastPlPos <= player.position.z)
        {
            pos.z += posZ;
            lastPlPos = player.position.z;
        }
        else
            pos.z = this.transform.position.z;


        this.transform.position = pos;
            
	}

    public void ChangeCamera(int nY,int nZ)
    {
        posY = nY;
        posZ = nZ;
    }
}
