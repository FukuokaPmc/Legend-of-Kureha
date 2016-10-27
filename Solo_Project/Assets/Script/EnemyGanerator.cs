using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public struct EnemyBox
{
    public int Block;    //何ブロック目に配置されているか
    public int EnemyNumber; //敵番号（ボスは別の仕組みで生成する？）
    public Vector2 Epos; //XY座標格納
};

public class EnemyGanerator : MonoBehaviour {
    
    private float BlockDist; //次のステージブロックまでの距離
    private GameObject[] ENEMY;
    public GameObject[] EnemyModel;
    private string FileName;
    private string Stage;
    private TextAsset CSV;
    private List<string[]> EnemyStatus = new List<string[]>();
    private EnemyBox[] EnemyPos;
    private Transform Player;
    // Use this for initialization
    void Start () {
        BlockDist = 50.0f;
        FileName = "Stage";
        Stage = "1";
        CSV = Resources.Load("Stage/" + FileName + Stage) as TextAsset;
        StringReader reader = new StringReader(CSV.text);

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            EnemyStatus.Add(line.Split(','));
        }

        EnemyPos = new EnemyBox[EnemyStatus.Count]; //ファイルの長さを取得し、それに合わせて長さを変更
        ENEMY = new GameObject[EnemyStatus.Count];

       // int ListCount = 0;
        for(int n = 0; n < EnemyPos.Length; n++)
        {
            EnemyPos[n].Block = int.Parse(EnemyStatus[n][0]);
            //EnemyPos[n].Block = 0;
            //ListCount++;
            EnemyPos[n].EnemyNumber = int.Parse(EnemyStatus[n][1]);
           // ListCount++;
            EnemyPos[n].Epos.x = float.Parse(EnemyStatus[n][2]);
            //ListCount++;
            EnemyPos[n].Epos.y = float.Parse(EnemyStatus[n][3]);
           // ListCount++;
        }

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int n = 0; n < EnemyPos.Length; n++)
        {
            ENEMY[n] = Instantiate(EnemyModel[EnemyPos[n].EnemyNumber]);
            ENEMY[n].GetComponent<EnemyMove>().SetTarget(Player.transform);
            ENEMY[n].transform.position = new Vector3(EnemyPos[n].Epos.x, EnemyPos[n].Epos.y, EnemyPos[n].Block * BlockDist);

        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int n = 0; n < ENEMY.Length; n++)
        {
            if(ENEMY[n] != null && Player.position.z >= ENEMY[n].transform.position.z + 10.0f)
            {
                Destroy(ENEMY[n].gameObject);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().SightOff();
            } 
        }

    }
}
