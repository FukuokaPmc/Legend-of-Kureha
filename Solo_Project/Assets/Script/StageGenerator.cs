using UnityEngine;
using System.Collections;

public class StageGenerator : MonoBehaviour {
    private GameObject NextRoad;
    public bool Flag;
    private BoxCollider Trigger;
    private int Count;
	// Use this for initialization
	void Start () {
        Count = 0;
        Flag = false;
        //NextRoad.SetActive(false);
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
                NextPos.z += 100.0f;
                NextRoad = Instantiate(this.gameObject);
                NextRoad.transform.position = NextPos;
                Flag = true;

            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.transform.position.z >= this.transform.position.z + 20.0f)
            Destroy(this.gameObject);
        }
    }
}