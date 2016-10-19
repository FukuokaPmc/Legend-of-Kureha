using UnityEngine;
using System.Collections;

public class StrongEnemy : EnemyMove
{
    public GameObject ShotModel;
    // Use this for initialization
    public override void Start () {
        Shot = Instantiate(ShotModel);
        Shot.transform.SetParent(this.transform);
        Shot.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 3.0f);
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
