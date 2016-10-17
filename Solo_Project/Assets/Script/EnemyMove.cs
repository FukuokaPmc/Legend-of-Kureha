using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {
    public Transform target;
    protected CharacterController cont;
    protected bool bAwake;
	// Use this for initialization
   void Awake()
    {
        cont = GetComponent<CharacterController>();
        bAwake = false;
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

    void AwakeEnemy()
    {
        bAwake = true;
    }
    
}
