using UnityEngine;
using System.Collections;

public class State<T>
{

    protected T owner;
    public State(T owner)
    {
        this.owner = owner;
    }

    //ステート遷移時に呼ばれる
    public virtual void Enter(){}
    //ステート中毎フレーム呼び出される
    public virtual void Execute() { }
    //ステート遷移する際に一度だけ呼ばれる
    public virtual void Exit() { }
}
