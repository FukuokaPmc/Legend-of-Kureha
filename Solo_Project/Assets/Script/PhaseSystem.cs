using UnityEngine;
using System.Collections;

public class PhaseSystem : MonoBehaviour {
    public static bool Boss; //trueでボス戦
    public static bool BossProduct; //ボスの登場演出が終わったかどうか
	// Use this for initialization
	void Start () {
        Boss = false;
        BossProduct = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Boss = true;
        }
    }

    public void BossStage()
    {
        Boss = true;
        //Camera.main.GetComponent<CameraMove>().SetBoss();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>().SetBoss();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stage"))
        {
            Destroy(obj.gameObject);
        }

    }

}
