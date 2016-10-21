using UnityEngine;
using System.Collections;

public class PhaseSystem : MonoBehaviour {
    public static bool Boss; //trueでボス戦
	// Use this for initialization
	void Start () {
        Boss = false;

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
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stage"))
        {
            Destroy(obj.gameObject);
        }

        
    }

}
