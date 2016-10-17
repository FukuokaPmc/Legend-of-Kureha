using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {
    public Transform target;
    protected CharacterController cont;
    protected bool bAwake;
    private float AwakeDistance; //どこまで近づいてきたら起動するか
	// Use this for initialization
   void Awake()
    {
        cont = GetComponent<CharacterController>();
        bAwake = false;
        AwakeDistance = 35.0f;
    }
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

    //敵の弾
    protected void EnemyShot(int nMaxShot)
    {

    }

    //敵の突進
    protected void EnemyCharge(float fSpeed)
    {
        this.transform.LookAt(target);
        cont.Move((target.position - this.transform.position).normalized * fSpeed);
    }

    void SetTarget(Transform Target)
    {
        target = Target;
    }

    protected bool AwakeEnemy()
    {
        if (this.transform.position.z - AwakeDistance < target.position.z )
        {
            bAwake = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
