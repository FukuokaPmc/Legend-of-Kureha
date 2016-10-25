using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Wait,
    Move,
    Jump,
    Attack,
    Dash,
    Refrect,
    Step,
}

public class PlayerMove : StateSystem<PlayerMove, PlayerState> {
    public float Speed;
    public float JumpSpeed;
    public float Gravity;
    private float HPMax; //最大HP
    private float HPNow; //今のHP
    private HPGauge gauge;
    private CharacterController cont;
    private Animator anim;
    private Vector3 MoveDir;
    private Vector3 PrevPos;
    public GameObject AttackBox;

    private Transform Procamera; //演出用カメラ
    private Transform target; //ダッシュ用のターゲット
    private GameObject Sight;
    private bool bTarget;
    private Vector3 RefrectSize; //跳ねっ返りのサイズ

    private float InvaridTime;
    private bool InvaridFlag;
    private float InvaridCount;

    private GameObject Trail;

    private KeyCode Dash;
    private KeyCode Attack;
    private KeyCode Lockon;
    private KeyCode Jump;
    // Use this for initialization
    void Start () {
        Speed = 15.0f;
        JumpSpeed = 10.0f;
        Gravity = 10.0f;
        HPMax = 100.0f;
        HPNow = HPMax;
        gauge = GameObject.FindGameObjectWithTag("HPGauge").GetComponent<HPGauge>();

        cont = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        MoveDir = Vector3.zero;
        Procamera = this.transform.GetChild(0);
        // Procamera.gameObject.SetActive(false);
        target = null;
        bTarget = false;
        Sight = GameObject.FindGameObjectWithTag("Sight");
        Sight.SetActive(false);
        InvaridTime = 1.0f;
        InvaridFlag = false;
        InvaridCount = 0.0f;

        Trail = transform.GetChild(3).gameObject;
        Trail.SetActive(false);

        Dash = KeyCode.K;
        Attack = KeyCode.J;
        Jump = KeyCode.M;
        Lockon = KeyCode.L;

        Initialize();
    }

    public void Initialize()
    {
        stateList.Add(new stateWait(this));
        stateList.Add(new stateMove(this));
        stateList.Add(new stateJump(this));
        stateList.Add(new stateAttack(this));
        stateList.Add(new stateDash(this));
        stateList.Add(new stateRefrect(this));
        stateList.Add(new stateStep(this));

        stateMachine = new StateMachine<PlayerMove>();

        ChangeState(PlayerState.Wait);
    }

    // Update is called once per frame
    protected override void Update () {
        if(InvaridFlag)
        {
            InvaridCount += 1.0f * Time.deltaTime;

            if(InvaridCount >= 1.0f)
            {
                InvaridFlag = false;
                InvaridCount = 0;
                cont.enabled = true;
            }
        }
        if (Input.GetKeyDown(Lockon) && !PhaseSystem.BossProduct)
            LockOn();
        if(!PhaseSystem.BossProduct)
            stateMachine.Update();
        else if(!IsCurrentState(PlayerState.Wait))
            ChangeState(PlayerState.Wait);
    }

    //何もしていないとき
    private class stateWait : State<PlayerMove>
    {
        public stateWait(PlayerMove owner) : base(owner) { }

        public override void Enter()
        {
            
        }

        public override void Execute()
        {
            owner.cont.Move(new Vector3(0,-owner.Gravity * Time.deltaTime,0));
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    owner.ChangeState(PlayerState.Move);

                if (Input.GetKey(owner.Jump))
                    owner.ChangeState(PlayerState.Jump);

                if (Input.GetKey(owner.Attack))
                    owner.ChangeState(PlayerState.Attack);

                if (Input.GetKeyDown(owner.Dash))
                {
                    if (owner.bTarget)
                    {
                        owner.ChangeState(PlayerState.Dash);
                    }
                    else
                    {
                        owner.ChangeState(PlayerState.Step);
                    }
                }
        }

        public override void Exit()
        {
            
        }
    }

    //移動中
    private class stateMove : State<PlayerMove>
    {
        public stateMove(PlayerMove owner) : base(owner) { }

        private bool end;

        public override void Enter()
        {
            owner.anim.SetBool("Run",true);
            end = false;
        }

        public override void Execute()
        {
            end = owner.Move();

            if (Input.GetKey(owner.Jump))
                owner.ChangeState(PlayerState.Jump);

            if (end)
                owner.ChangeState(PlayerState.Wait);

            if (Input.GetKey(owner.Attack))
                owner.ChangeState(PlayerState.Attack);

            if (Input.GetKeyDown(owner.Dash))
            {
                if (owner.bTarget)
                {
                    owner.ChangeState(PlayerState.Dash);
                }
                else
                {
                    owner.ChangeState(PlayerState.Step);
                }
            }

        }

        public override void Exit()
        {
            owner.anim.SetBool("Run", false);
            //Destroy(owner);
        }
    }

    //ジャンプボタンを押した時
    private class stateJump : State<PlayerMove>
    {
        public stateJump(PlayerMove owner) : base(owner) { }

        //private bool end;
        private Vector3 HeightMove;
        private float Count;
        private float JumpWait;
        public override void Enter()
        {
            HeightMove.x = 0f;
            HeightMove.y = owner.JumpSpeed;
            HeightMove.z = 0f;

            owner.anim.SetTrigger("Jump");
            //end = false;
            Count = 0;
            JumpWait = 0.1f;
        }

        public override void Execute()
        {
            

            

            if (Count >= JumpWait)
            {
                owner.Move();
                HeightMove.y -= owner.Gravity * Time.deltaTime;
                owner.cont.Move(HeightMove * Time.deltaTime);

                if (owner.cont.isGrounded && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
                    owner.ChangeState(PlayerState.Move);
                else if (owner.cont.isGrounded)
                    owner.ChangeState(PlayerState.Wait);
            }
            else
            {
                Count += 1.0f * Time.deltaTime;
            }

            

            if (Input.GetKey(owner.Attack))
                owner.ChangeState(PlayerState.Attack);

            if (Input.GetKeyDown(owner.Dash))
            {
                if (owner.bTarget)
                {
                    owner.ChangeState(PlayerState.Dash);
                }
                else
                {
                    owner.ChangeState(PlayerState.Step);
                }
            }
        }

        public override void Exit()
        {

        }
    }

    //地上・空中時の移動処理
    private bool Move()
    {
        float InpX = 0f;
        float InpY = 0f;
        bool bX = false;
        bool bY = false;
        if (!PhaseSystem.Boss)
        {
            if (Input.GetKey(KeyCode.A))
            {
                InpX = -1f;
                bX = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                InpX = 1f;
                bX = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                InpY = 1f;
                bY = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                InpY = -1f;
                bY = true;
            }
            if (bX && bY)
            {
                InpX /= 2;
                InpY /= 2;
            }

            if (bX && bY)
            {
                if (InpX >= 0)
                {
                    if (InpY <= 0)
                        this.transform.eulerAngles = new Vector3(0, 135, 0);
                    else
                        this.transform.eulerAngles = new Vector3(0, 45, 0);
                }
                else
                {
                    if (InpY <= 0)
                        this.transform.eulerAngles = new Vector3(0, -135, 0);
                    else
                        this.transform.eulerAngles = new Vector3(0, -45, 0);
                }
            }
            else if (bX)
            {
                if (InpX >= 0)
                    this.transform.eulerAngles = new Vector3(0, 90, 0);
                else
                    this.transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else if (bY)
            {
                if (InpY <= 0)
                    this.transform.eulerAngles = new Vector3(0, 180, 0);
                else
                    this.transform.eulerAngles = new Vector3(0, 0, 0);
            }




            MoveDir = new Vector3(Mathf.Sin(this.transform.eulerAngles.y * (Mathf.PI / 180.0f)), 0.0f, Mathf.Cos(this.transform.eulerAngles.y * (Mathf.PI / 180.0f)));
            MoveDir.y -= Gravity * Time.deltaTime;
            MoveDir *= Speed;

            if (bX || bY)
            {
                cont.Move(MoveDir * Time.deltaTime);
            }

            if ((!bX) && (!bY))
                return true;
            return false;
        }
        else if(!PhaseSystem.BossProduct)
        {
            Vector3 PlayerPos;
            if (Input.GetKey(KeyCode.A))
            {
                InpX = -1f;
               // transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
                bX = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                InpX = 1f;
               // transform.RotateAround(Vector3.zero, Vector3.up, -20 * Time.deltaTime);
                bX = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                // transform.LookAt(Camera.main.transform.forward);
                InpY = 1.0f;
                bY = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
               // transform.LookAt(-Camera.main.transform.forward);
                InpY = -1f;
                bY = true;
            }

            MoveDir = InpX * Camera.main.transform.TransformDirection(Vector3.right) + InpY * Camera.main.transform.TransformDirection(Vector3.forward);
            MoveDir.y = 0f;
            PlayerPos = MoveDir;
            MoveDir.y -= Gravity * Time.deltaTime;
            MoveDir *= Speed;

            

            if (bX || bY)
            {
                cont.Move(MoveDir * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(PlayerPos);
            }

            PrevPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);

            if ((!bX) && (!bY))
                return true;
            return false;
        }
        else
        {
            return false;
        }
    }

    private class stateAttack : State<PlayerMove>
    {
        public stateAttack(PlayerMove owner) : base(owner) { }

        private GameObject attackbox;
        private Vector3 MoveSize;
        public override void Enter()
        {
            owner.anim.SetTrigger("Attack");
            attackbox = Instantiate(owner.AttackBox) as GameObject;
            attackbox.transform.SetParent(owner.transform);
            attackbox.transform.localPosition = new Vector3(0, 1, 1.2f);
        }

        public override void Execute()
        {
            if (owner.bTarget)
            {
                owner.transform.LookAt(owner.target);
                if(owner.cont.isGrounded)
                    owner.transform.eulerAngles = new Vector3(0.0f,owner.transform.eulerAngles.y, owner.transform.eulerAngles.z);
            }
            if (owner.anim.GetBool("AttackEnd"))
                owner.ChangeState(PlayerState.Wait);

            MoveSize = new Vector3(Mathf.Sin(owner.transform.eulerAngles.y * (Mathf.PI / 180.0f)), 0.0f, Mathf.Cos(owner.transform.eulerAngles.y * (Mathf.PI / 180.0f)));
            owner.cont.Move(MoveSize * 0.02f);
        }

        public override void Exit()
        {
            owner.anim.SetBool("AttackEnd", false);
            Destroy(attackbox);
        }
    }

    private class stateDash : State<PlayerMove>
    {
        public stateDash(PlayerMove owner) : base(owner) { }

        private Vector3 StartPoint;
        private float Point;
        private float MoveSpeed;

        private Vector3 MoveSize;

        public override void Enter()
        {
            Vector3 box;
            StartPoint = owner.transform.position;
            StartPoint.y += 1f; 
            //DashMove = Vector3.zero;
            // DashMove.z = 100.0f;

            Point = 0.0f;
            MoveSpeed = 40f;


            owner.anim.SetBool("Dash", true);
            owner.Procamera.GetComponent<Production>().StartProduction(0);
            owner.Sight.GetComponent<Target>().CameraChange();
            if (owner.bTarget)
                MoveSize = (owner.target.position - StartPoint).normalized;
            else
                MoveSize = new Vector3(Mathf.Sin(owner.transform.eulerAngles.y * (Mathf.PI / 180.0f)), 0.0f, Mathf.Cos(owner.transform.eulerAngles.y * (Mathf.PI / 180.0f)));

            MoveSize *= MoveSpeed;

            

        }

        public override void Execute()
        {
            if(owner.bTarget)
                owner.transform.LookAt(owner.target);
            //owner.transform.position = Vector3.Lerp(StartPoint,owner.target.position,Point);
            //Point += MoveSpeed;
            
            owner.cont.Move(MoveSize * Time.deltaTime);

            if (Input.GetKeyDown(owner.Dash) || owner.cont.enabled != true)
                owner.ChangeState(PlayerState.Wait);
        }

        public override void Exit()
        {
            owner.transform.localEulerAngles = Vector3.zero;
            owner.anim.SetBool("Dash", false);
            owner.Procamera.GetComponent<Production>().EndProduction();
            owner.Sight.GetComponent<Target>().CameraChange();
            MoveSize.x *= -1;
           // MoveSize.y *= -1;
            MoveSize.z *= -1;
            owner.RefrectSize = MoveSize;

            
        }



        

    }

    private class stateRefrect : State<PlayerMove>
    {
        public stateRefrect(PlayerMove owner) : base(owner) { }
        private int Count;
        public override void Enter()
        {
            owner.RefrectSize *= 0.3f;
            Count = 0;
            Time.timeScale = 0.3f;
        }

        public override void Execute()
        {
            Count++;
            owner.cont.Move(owner.RefrectSize * Time.deltaTime);

            if(Count >= 30)
                owner.ChangeState(PlayerState.Wait);
        }

        public override void Exit()
        {
            Time.timeScale = 1.0f;
        }

    }

    private class stateStep : State<PlayerMove>
    {
        public stateStep(PlayerMove owner) : base(owner) { }
        private float Count;
        private float StepVelo;
        public override void Enter()
        {
            Count = 0;
            StepVelo = 10.0f;
            owner.Trail.SetActive(true);
            owner.Trail.GetComponent<Trail>().TimerStop();
        }

        public override void Execute()
        {
            Count += 1.0f * Time.deltaTime;
            owner.cont.Move(owner.MoveDir * StepVelo * Time.deltaTime);

            if (Count >= 0.1f)
                owner.ChangeState(PlayerState.Wait);
        }

        public override void Exit()
        {
            owner.Trail.GetComponent<Trail>().EraseTimer();
        }
    }

    //ロックオン
    void LockOn()
    {
        float Distance;
        float Near = 0;

        if (!bTarget)
        {
            GameObject[] enemy;
            enemy = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemy.Length != 0)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))  //とりあえず敵のみ取得
                {
                    Distance = Vector3.Distance(obj.transform.position, this.transform.position);

                    if ((Near == 0 || Near > Distance) && (this.transform.position.z <= obj.transform.position.z))
                    {
                        Near = Distance;
                        target = obj.transform;
                    }
                }

                bTarget = true;
                Sight.SetActive(true);
                Sight.GetComponent<Target>().TargetLockOn(target);
            }

            if(PhaseSystem.Boss)
            {
                target = GameObject.FindGameObjectWithTag("Boss").transform;

                bTarget = true;
                Sight.SetActive(true);
                Sight.GetComponent<Target>().TargetLockOn(target);
            }
        }
        else
        {
            bTarget = false;
            Sight.SetActive(false);
        }
            
    }

    public void SightOff()
    {
        bTarget = false;
        Sight.SetActive(false);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Enemy")
        {
            if(IsCurrentState(PlayerState.Dash))
            {
                ChangeState(PlayerState.Refrect);
                hit.gameObject.GetComponent<EnemyMove>().Stun();
            }    
        }
        if (hit.gameObject.tag == "Boss" || hit.gameObject.tag == "BossParts")
        {
            if (IsCurrentState(PlayerState.Dash))
            {
                ChangeState(PlayerState.Refrect);
            }
        }

    }

    public void HPMinus(float fDamage)
    {
        HPNow -= fDamage;
        gauge.HPChange(HPNow / HPMax);
        Invarid();
    }

    private void  Invarid()
    {
        InvaridFlag = true;
        cont.enabled = false;
    }

}
