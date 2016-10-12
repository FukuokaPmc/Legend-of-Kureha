using UnityEngine;
using System.Collections;

public class Check : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        // if (collision.gameObject.tag)
        Debug.Log("滅びよ");
        if(col.gameObject.tag == "Enemy")
            Destroy(col.gameObject);
    }
}
