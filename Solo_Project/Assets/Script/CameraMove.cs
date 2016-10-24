using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    private Transform player;
    private float lastPlPos;
    private float lastPlayerX;
    public float posY;
    public float posZ;
    private float Speed;

    //ボス戦・演出用
    private Transform Boss;
    private bool[] Product;
    private Vector3 BossCamPos;
    private float CameraMoveSize;
    private float TimeCount;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        lastPlPos = player.position.z;
        lastPlayerX = player.position.x;
        Speed = 2.0f;

        posY = 3.0f;
        posZ = -6.0f;

        Boss = GameObject.FindWithTag("Boss").transform;
        Product = new bool[2];
        Product[0] = true;
        Product[1] = false;

        BossCamPos = new Vector3(0, 3, -5);
        CameraMoveSize = 0.01f;
        TimeCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos;
        Vector3 cameraPos;

        if (!PhaseSystem.Boss)
        {
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
        else
        {
            cameraPos = (Boss.position - player.position).normalized * -6.0f;
            cameraPos.y += 4.0f;
            if (Product[0])
            {
                if (!Product[1] && Vector3.Distance(this.transform.position, Boss.transform.position) >= 5.0f)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, Boss.transform.position, 0.1f);
                }
                else //if(Vector3.Distance(this.transform.position, cameraPos + player.position) >= 1.0f)
                {
                    if (TimeCount <= 1.5f)
                    {
                        this.transform.Translate(new Vector3(0, 1.0f, 0) * Time.deltaTime);
                        TimeCount += 1.0f * Time.deltaTime; 
                        Product[1] = true;
                    }
                    else
                    {
                        this.transform.position = Vector3.Lerp(this.transform.position, cameraPos + player.position, CameraMoveSize);
                        CameraMoveSize += 0.0001f;
                    }
                }

                if(Boss.GetComponent<BossMove>().IsCurrentState(BossState.Wait))
                {
                    Product[0] = false; 
                }
            }
            else
            {
                this.transform.position = player.position + cameraPos;
            }
            this.transform.LookAt(Boss.transform);
        }
            

    }

    public void BossProdEnd()
    {
        Product[0] = false;
    }

    public void SetBoss()
    {
        Boss = GameObject.FindWithTag("Boss").transform;
    }

}
