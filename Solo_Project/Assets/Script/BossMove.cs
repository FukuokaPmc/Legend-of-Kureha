using UnityEngine;
using System.Collections;

public class BossMove : MonoBehaviour {
    private Animation anime;
	// Use this for initialization
	void Start () {
        anime = GetComponent<Animation>();

    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            anime.CrossFade("Magic");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anime.CrossFade("Dead1");
        }
    }
}
