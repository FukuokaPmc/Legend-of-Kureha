using UnityEngine;
using System.Collections;

public class StageGenerator : MonoBehaviour {
    private GameObject NextRoad;
    public bool Flag;
	// Use this for initialization
	void Start () {
        Flag = false;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //void OnCollisionEnter(Collision collision)
    void OnTriggerEnter(Collider col)
    {
       // if (!Flag)
      //  {
            if (col.gameObject.tag == "Player")
            {
                Vector3 NextPos;
                NextPos = this.transform.position;
                NextPos.z += 50.0f;
                NextRoad = Instantiate(this.gameObject);
                NextRoad.transform.position = NextPos;

                Flag = true;
            }
       // }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}