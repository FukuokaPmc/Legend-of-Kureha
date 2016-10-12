using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Wait,
    Move,
    Jump,
    Attack,
    Dash,
}

public class PlayerMove : StateSystem<PlayerMove, PlayerState> {
    public float Speed;
    public float JumpSpeed;
    public float Gravity;
    private CharacterController cont;
    private Animator anim;
    private Vector3 MoveDir;
    public GameObject AttackBox;

    private Transform Procamera; //演出用カメラ
    private Transform target; //ダッシュ用のターゲット

	// Use this for initialization
	void Start () {
        Speed = 5.0f;
        JumpSpeed = 7.0f;
        Gravity = 10.0f;
        cont = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        MoveDir = Vector3.zero;
        Procamera = this.transform.GetChild(0);
        // Procamera.gameObject.SetActive(false);
        target = GameObject.Find("WeakEnemy").transform;
        Initialize();
    }

    public void Initialize()
    {
        stateList.Add(new stateWait(this));
        stateList.Add(new stateMove(this));
        stateList.Add(new stateJump(this));
        stateList.Add(new stateAttack(this));
        stateList.Add(new stateDash(this));

        stateMachine = new StateMachine<PlayerMove>();

        ChangeState(PlayerState.Wait);
    }

    // Update is called once per frame
    /*void Update () {
        stateMachine.Update();
    }*/

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

            if (Input.GetKey(KeyCode.LeftShift))
                owner.ChangeState(PlayerState.Jump);

            if(Input.GetMouseButton(0))
                owner.ChangeState(PlayerState.Attack);

            if(Input.GetKeyDown(KeyCode.Space))
                owner.ChangeState(PlayerState.Dash);
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

            if (Input.GetKey(KeyCode.LeftShift))
                owner.ChangeState(PlayerState.Jump);

            if (end)
                owner.ChangeState(PlayerState.Wait);

            if (Input.GetMouseButton(0))
                owner.ChangeState(PlayerState.Attack);

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

        public override void Enter()
        {
            HeightMove.x = 0f;
            HeightMove.y = owner.JumpSpeed;
            HeightMove.z = 0f;

            owner.anim.SetTrigger("Jump");
            //end = false;
        }

        public override void Execute()
        {
            owner.Move();

            HeightMove.y -= owner.Gravity * Time.deltaTime;

            owner.cont.Move(HeightMove * Time.deltaTime);

            if (owner.cont.isGrounded && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
                owner.ChangeState(PlayerState.Move);
            else if (owner.cont.isGrounded)
                owner.ChangeState(PlayerState.Wait);

            if (Input.GetMouseButton(0))
                owner.ChangeState(PlayerState.Attack);
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

    private class stateAttack : State<PlayerMove>
    {
        public stateAttack(PlayerMove owner) : base(owner) { }

        private GameObject attackbox;

        public override void Enter()
        {
            owner.anim.SetTrigger("Attack");
            attackbox = Instantiate(owner.AttackBox) as GameObject;
            attackbox.transform.SetParent(owner.transform);
            attackbox.transform.localPosition = new Vector3(0, 1, 1.2f);
        }

        public override void Execute()
        {
            if (owner.anim.GetBool("AttackEnd"))
                owner.ChangeState(PlayerState.Wait);
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

            MoveSize = (owner.target.position - StartPoint).normalized * MoveSpeed;

        }

        public override void Execute()
        {
            owner.transform.LookAt(owner.target);
            //owner.transform.position = Vector3.Lerp(StartPoint,owner.target.position,Point);
            //Point += MoveSpeed;
            
            owner.cont.Move(MoveSize * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
                owner.ChangeState(PlayerState.Wait);
        }

        public override void Exit()
        {
            owner.transform.localEulerAngles = Vector3.zero;
            owner.anim.SetBool("Dash", false);
            owner.Procamera.GetComponent<Production>().EndProduction();
        }

        

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Enemy")
            ChangeState(PlayerState.Wait);
    }
}
