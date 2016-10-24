using UnityEngine;
using System.Collections;

public class WeakEnemy : EnemyMove
{
    public GameObject ShotModel;
    // Use this for initialization
    public override void Start () {
        HormingTime = 0.5f;
        ChargeTime = 1.0f;
        ChargeDamage = 20.0f;
        Shot = Instantiate(ShotModel);
        Shot.transform.SetParent(this.transform);
        Shot.transform.localScale = Vector3.one;
        Shot.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 3.0f);
    }

    // Update is called once per frame
    public override void Update () {
        if (bAwake || AwakeEnemy())
        {
            EnemyShot();
           // EnemyCharge(0.3f);
            EnemyStun();
        }
	}
}
