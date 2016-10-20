using UnityEngine;
using System.Collections;

public class PhaseSystem : MonoBehaviour {
    private bool Boss; //trueでボス戦
	// Use this for initialization
	void Start () {
        Boss = false;

    }
	
	// Update is called once per frame
	void Update () {
	    
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
