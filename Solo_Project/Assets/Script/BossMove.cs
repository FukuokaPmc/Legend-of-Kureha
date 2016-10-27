using UnityEngine;
using System.Collections;

public enum BossState
{
    Sleep,
    Awake,
    Wait,
    Charge,
    Shock,
    Blast,
    Break,
    Laser,
    Dead,
}

public class BossMove : StateSystem<BossMove, BossState>
{
    private Animation anime;
    private CharacterController control;
    private Scene_Manager scene;
    private bool bAwake;
    private PhaseSystem phase;
    private GameObject Player;
    private BossParts parts;
    private GameObject Barrier;
    private float AttackCount;
    private GameObject chargeEff;
    private GameObject shockWaveEff;
    // Use this for initialization
    void Start()
    {
        anime = GetComponent<Animation>();
        scene = GameObject.Find("SceneManager").GetComponent<Scene_Manager>();
        //phase = GameObject.Find("PhaseSystem").GetComponent<PhaseSystem>();
        Player = GameObject.FindGameObjectWithTag("Player");
        parts = GameObject.FindGameObjectWithTag("BossParts").GetComponent<BossParts>();
        Barrier = GameObject.Find("Barrier");
        Barrier.SetActive(false);
        bAwake = false;
        AttackCount = 0;

        chargeEff = transform.GetChild(5).gameObject;
        chargeEff.GetComponent<ParticleSystem>().Clear();
        shockWaveEff = transform.GetChild(6).gameObject;
        shockWaveEff.GetComponent<ParticleSystem>().Clear();

        Initialize();
    }

    public void Initialize()
    {;
        stateList.Add(new stateSleep(this));
        stateList.Add(new stateAwake(this));
        stateList.Add(new stateWait(this));

        stateList.Add(new stateCharge(this));
        stateList.Add(new stateShock(this));
        stateList.Add(new stateBlast(this));
        stateList.Add(new stateBreak(this));
        stateList.Add(new stateLaser(this));

        stateList.Add(new stateDead(this));

        stateMachine = new StateMachine<BossMove>();

        ChangeState(BossState.Sleep);
    }



    // Update is called once per frame
    protected override void Update()
    {
        if(AuroraLast())
        {
            Barrier.GetComponent<ParticleSystem>().Stop();
            if(!Barrier.GetComponent<ParticleSystem>().isPlaying)
            {
                Barrier.SetActive(false);
            }
        }
        stateMachine.Update();
    }

    //休眠
    private class stateSleep : State<BossMove>
    {
        public stateSleep(BossMove owner) : base(owner) { }

        public override void Enter()
        {

        }

        public override void Execute()
        {
            if(PhaseSystem.Boss)
            {
                owner.ChangeState(BossState.Awake);
            }
        }

        public override void Exit()
        {

        }
    }

    //開始演出
    private class stateAwake : State<BossMove>
    {
        public stateAwake(BossMove owner) : base(owner) { }

        private bool bStart;
        public override void Enter()
        {
            owner.anime.CrossFade("Levitate");
            bStart = false;
            PhaseSystem.BossProduct = true;
        }

        public override void Execute()
        {
            if(!owner.anime.isPlaying && !bStart)
            {
                bStart = true;
                owner.anime.CrossFade("Levitate_sky");
            }

            if(bStart)
            {
                owner.transform.Translate(new Vector3(0,2,0) * Time.deltaTime);

                if(owner.transform.position.y > 10.0f)
                {
                    owner.ChangeState(BossState.Wait);
                }
            }
        }

        public override void Exit()
        {
            PhaseSystem.BossProduct = false;
            owner.Barrier.SetActive(true);
            GameObject.FindGameObjectWithTag("BossStage").transform.GetChild(0).GetChild(0).GetComponent<Collider>().isTrigger = false;
        }
    }

    //何もしていないとき
    private class stateWait : State<BossMove>
    {
        public stateWait(BossMove owner) : base(owner) { }
        private Vector3 dist;
        private Vector3 BPos;
        private Vector3 PPos;
        public override void Enter()
        {
            owner.anime.CrossFade("Idle");
           owner.AttackCount = 0;
        }

        public override void Execute()
        {
            BPos = owner.transform.position;
            BPos.y = 0;
            PPos = owner.Player.transform.position;
            PPos.y = 0;
            dist = (PPos - BPos).normalized;
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(PPos - BPos), 0.5f * Time.deltaTime);

            owner.AttackCount += 1.0f * Time.deltaTime;
            if(owner.AttackCount >= 3.0f)
            {
                owner.ChangeState(BossState.Charge);
            }
        }

        public override void Exit()
        {

        }
    }

    private class stateCharge : State<BossMove>
    {
        public stateCharge(BossMove owner) : base(owner) { }

        private int ChargeCount;
        private Vector3 dist;
        private Vector3 BPos;
        private Vector3 PPos;

        public override void Enter()
        {
            ChargeCount = 0;
        }

        public override void Execute()
        {
            BPos = owner.transform.position;
            BPos.y = 0;
            PPos = owner.Player.transform.position;
            PPos.y = 0;
            dist = (PPos - BPos).normalized;
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(PPos - BPos), 0.3f * Time.deltaTime);

            if (!owner.chargeEff.GetComponent<ParticleSystem>().isPlaying)
            {
                owner.chargeEff.GetComponent<ParticleSystem>().Play();
                ChargeCount++;
            }
            if(ChargeCount > 3)
            {
                owner.ChangeState(BossState.Shock);
            }
            

        }

        public override void Exit()
        {
            owner.chargeEff.GetComponent<ParticleSystem>().Clear();
        }
    }

    //対地衝撃波
    private class stateShock : State<BossMove>
    {
        public stateShock(BossMove owner) : base(owner) { }

        public override void Enter()
        {
            owner.shockWaveEff.transform.position = new Vector3(0, 1, 0);
            owner.shockWaveEff.GetComponent<ParticleSystem>().Play();
        }

        public override void Execute()
        {
            if (!owner.shockWaveEff.GetComponent<ParticleSystem>().isPlaying)
            {
                owner.ChangeState(BossState.Wait);
            }

        }

        public override void Exit()
        {

        }
    }

    //連続火柱
    private class stateBlast : State<BossMove>
    {
        public stateBlast(BossMove owner) : base(owner) { }

        public override void Enter()
        {

        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }

    //ダメージ
    private class stateBreak : State<BossMove>
    {
        public stateBreak(BossMove owner) : base(owner) { }

        public override void Enter()
        {

        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }

    //レーザー発射
    private class stateLaser : State<BossMove>
    {
        public stateLaser(BossMove owner) : base(owner) { }

        public override void Enter()
        {

        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }

    //死亡演出
    private class stateDead : State<BossMove>
    {
        public stateDead(BossMove owner) : base(owner) { }

        public override void Enter()
        {
            owner.anime.CrossFade("Dead");
        }

        public override void Execute()
        {
            if(!owner.anime.isPlaying)
            {
                owner.scene.SceneChange();
            }
        }

        public override void Exit()
        {

        }
    }


    //ステート作成用ベース（ある程度作ったら消す）
    private class bases : State<BossMove>
    {
        public bases(BossMove owner) : base(owner) { }

        public override void Enter()
        {

        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }


   // void OnCollisionEnter(Collision collision)
   void OnTriggerEnter(Collider other)
    {
        Debug.Log("hoge");
        if(other.gameObject.tag == "Attack" && AuroraLast())
        {
            ChangeState(BossState.Dead);
        }
    }

    public bool AuroraLast()
    {
        return parts.LastAurora();
    }
}
