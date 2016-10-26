using UnityEngine;
using System.Collections;

public class EnemyShot : MonoBehaviour {
    public float fDamage;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject obj)
    {
        if(obj.tag == "Player")
        {
            Debug.Log("被弾");
           if( !obj.GetComponent<PlayerMove>().IsCurrentState(PlayerState.Step) && !obj.GetComponent<PlayerMove>().InvaridCheck())
            {
                obj.GetComponent<PlayerMove>().HPMinus(fDamage);
            }
        }
    }
}
