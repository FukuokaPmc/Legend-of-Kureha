using UnityEngine;
using System.Collections;

public class StageGenerator : MonoBehaviour {
    private GameObject NextRoad;
    public bool Flag;
    private BoxCollider Trigger;
    public static int StageCount;
    public GameObject BossStage;
    public GameObject BossModel;
    public GameObject BossEff;
    private PhaseSystem phase;
    private int StageMax;
	// Use this for initialization
	void Start () {
        //StageCount = 0;
        Flag = false;
        phase = GameObject.FindGameObjectWithTag("Phase").GetComponent<PhaseSystem>();
        StageMax = 5;
    }
	
	// Update is called once per frame
	void Update () {

    }

    //void OnCollisionEnter(Collision collision)
    void OnTriggerEnter(Collider col)
    {
        if (!Flag)
        {
            if (col.gameObject.tag == "Player")
            {
                Vector3 NextPos;
                NextPos = this.transform.position;
                if (StageCount < StageMax)
                {
                    NextPos.z += 100.0f;
                    NextRoad = Instantiate(this.gameObject);
                    NextRoad.transform.position = NextPos;
                    StageCount++;
                }
                else
                {
                    NextPos.z += 140.0f;
                    BossStage = Instantiate(BossStage);
                    BossStage.transform.position = NextPos;
                    BossModel = Instantiate(BossModel);
                    BossModel.transform.position = BossStage.transform.position;
                    StageCount++;
                }

                if(StageCount == StageMax - 1)
                {
                    NextPos.z += 190.0f;
                    NextPos.y += 5.0f;
                    BossEff = Instantiate(BossEff);
                    BossEff.transform.position = NextPos;
                }
                Flag = true;
                

            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(StageCount > StageMax)
            {
                phase.BossStage();
            }
            if (col.transform.position.z >= this.transform.position.z + 20.0f)
            Destroy(this.gameObject);
        }

    }
}