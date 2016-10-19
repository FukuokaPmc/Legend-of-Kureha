using UnityEngine;
using System.Collections;

public class EnemyShot : MonoBehaviour {
    private float fDamage;
	// Use this for initialization
	void Start () {
        fDamage = 10.0f;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject obj)
    {
        if(obj.tag == "Player")
        {
            Debug.Log("被弾");
           if( !obj.GetComponent<PlayerMove>().IsCurrentState(PlayerState.Step))
            {
                obj.GetComponent<PlayerMove>().HPMinus(fDamage);
            }
        }
    }
}
