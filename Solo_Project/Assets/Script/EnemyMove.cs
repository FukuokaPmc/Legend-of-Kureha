using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {
    public Transform target;
    protected CharacterController cont;
    private Rigidbody rigid;
    protected bool bAwake;
    protected float AwakeDistance; //どこまで近づいてきたら起動するか
    private float nCount;
    private Vector3 ChargeVector;
    protected float ChargeTime;  //突進までのインターバル
    protected float HormingTime; //追いかける時間
                                 // Use this for initialization
    protected float ChargeDamage; //突進のダメージ
    protected float ShotDamage;   //弾のダメージ

    private bool bStun; //突進が当たった場合のひるみ発生フラグ
    protected float StunTime; //ひるみ時間
    private bool bHit; //突撃が当たった場合そのまま通過

    protected GameObject Shot;
    protected float HomingTime; //誘導する時間

    public GameObject DeadEffect;

    void Awake()
    {
        cont = GetComponent<CharacterController>();
        //rigid = GetComponent<Rigidbody>();
        bAwake = false;
        AwakeDistance = 60.0f;
        nCount = 0;
        bStun = false;
        StunTime = 1.0f;
        bHit = false;

        //ChargeTime = 1;
    }
	public virtual void Start () {
	
	}
	
	// Update is called once per frame

	public virtual void Update () {
	
	}

    //敵の弾
    protected void EnemyShot()
    {
        Shot.SetActive(true);
        if (!bStun)
        {
             nCount += 1.0f * Time.deltaTime;
             if (true)
             {
                 Shot.transform.LookAt(target);
                Shot.transform.eulerAngles = new Vector3(0.0f, Shot.transform.eulerAngles.y, Shot.transform.eulerAngles.z);
            }
        }
    }

    //敵の突進
    protected void EnemyCharge(float fSpeed)
    {
        Vector3 targetpos;
        targetpos = target.position;
        
        if (!bStun)
        {
            nCount += 1.0f * Time.deltaTime;
            if (nCount <= ChargeTime)
            {
                transform.Rotate(0,0,5);
            }
            else
            {
                targetpos = new Vector3(targetpos.x, targetpos.y + 1.0f, targetpos.z);
                if (target.position.z <= this.transform.position.z && !bHit)
                {
                    this.transform.LookAt(targetpos);
                    ChargeVector = (targetpos - this.transform.position).normalized;

                }
                cont.Move(ChargeVector * fSpeed);
            }
        }
    }

    protected void EnemyStun()
    {
        if (bStun)
        {
            nCount += 1.0f * Time.deltaTime;

            transform.Rotate(new Vector3(10.0f, 0.0f, 0.0f));

            if (nCount >= StunTime)
            {
                bStun = false;
                nCount = 0;
            }
        }
    }

    public void SetTarget(Transform Target)
    {
        target = Target;
    }

    protected bool AwakeEnemy()
    {
        if (this.transform.position.z - target.position.z  < AwakeDistance)
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

    public void Stun()
    {
        Debug.Log("hogehoge");
        bStun = true;
        nCount = 0;
    }

    public void Dead()
    {
        DeadEffect = Instantiate(DeadEffect);
        DeadEffect.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            PlayerMove player = hit.gameObject.GetComponent<PlayerMove>();
            if (!player.IsCurrentState(PlayerState.Dash))
            {
                
                Debug.Log(Damage(0));
                player.HPMinus(Damage(0));
                bHit = true;
            }
            else
            {
                
            }

        }

    }

}
