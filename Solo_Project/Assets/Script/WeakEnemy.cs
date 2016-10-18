using UnityEngine;
using System.Collections;

public class WeakEnemy : EnemyMove
{

	// Use this for initialization
	public override void Start () {
        HormingTime = 0.5f;
        ChargeTime = 1.0f;
        ChargeDamage = 20.0f;
    }

    // Update is called once per frame
    public override void Update () {
        if (bAwake || AwakeEnemy())
        {
            EnemyCharge(0.3f);
        }
	}
}
