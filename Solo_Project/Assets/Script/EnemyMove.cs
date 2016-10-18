using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {
    public Transform target;
    protected CharacterController cont;
    protected bool bAwake;
    private float AwakeDistance; //どこまで近づいてきたら起動するか
    private float nCount;
    private Vector3 ChargeVector;
    protected float ChargeTime;  //突進までのインターバル
    protected float HormingTime; //追いかける時間
                                 // Use this for initialization
    protected float ChargeDamage; //突進のダメージ
    protected float ShotDamage;   //弾のダメージ
    void Awake()
    {
        cont = GetComponent<CharacterController>();
        bAwake = false;
        AwakeDistance = 35.0f;
        nCount = 0;
        //ChargeTime = 1;
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
        nCount += 1.0f * Time.deltaTime;
        if (nCount <= ChargeTime)
        {
            this.transform.LookAt(target);
            ChargeVector = (target.position - this.transform.position).normalized;
        }
        else if (nCount <= ChargeTime + HormingTime)
        {
            this.transform.LookAt(target);
            ChargeVector = (target.position - this.transform.position).normalized;
            cont.Move(ChargeVector * fSpeed);
        }
        else
        {
            cont.Move(ChargeVector * fSpeed);
        }
    }

    public void SetTarget(Transform Target)
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

    public float Damage(int nNum)
    {
        if(nNum == 0)
        {
            return ChargeDamage;
        }
        else if(nNum == 1)
        {
            return ShotDamage;
        }
        else
        {
            return 0;
        }
    }
    
}
