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
        
        //NextRoad.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    //void OnCollisionEnter(Collision collision)
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Vector3 NextPos;
            NextPos = this.transform.position;
            NextPos.z += 100.0f;
            NextRoad = Instantiate(this.gameObject);
            NextRoad.transform.position = NextPos;

            
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}