using UnityEngine;
using System.Collections;

public enum BossState
{
    Sleep,
    Awake,
    Wait,
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
    // Use this for initialization
    void Start()
    {
        anime = GetComponent<Animation>();
        scene = GameObject.Find("SceneManager").GetComponent<Scene_Manager>();
        //phase = GameObject.Find("PhaseSystem").GetComponent<PhaseSystem>();
        Player = GameObject.FindGameObjectWithTag("Player");

        bAwake = false;

        Initialize();
    }

    public void Initialize()
    {;
        stateList.Add(new stateSleep(this));
        stateList.Add(new stateAwake(this));
        stateList.Add(new stateWait(this));
        stateList.Add(new stateDead(this));

        stateMachine = new StateMachine<BossMove>();

        ChangeState(BossState.Sleep);
    }



    // Update is called once per frame
    protected override void Update()
    {
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
        }

        public override void Execute()
        {
            BPos = owner.transform.position;
            BPos.y = 0;
            PPos = owner.Player.transform.position;
            PPos.y = 0;
            dist = (PPos - BPos).normalized;
            //owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(PPos - BPos), 0.5f * Time.deltaTime);
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
        if(other.gameObject.tag == "Attack")
        {
            ChangeState(BossState.Dead);
        }
    }
}
