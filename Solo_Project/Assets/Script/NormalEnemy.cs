using UnityEngine;
using System.Collections;

public class NormalEnemy : EnemyMove
{
    // Use this for initialization
    public override void Start () {
        HormingTime = 0.3f;
        ChargeTime = 2.0f;
        AwakeDistance = 45.0f;
        ChargeDamage = 30.0f;
    }

    // Update is called once per frame
    public override void Update () {
        if (bAwake || AwakeEnemy())
        {
            EnemyCharge(0.5f);
            EnemyStun();
        }
	}
}
