using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EnemyBox
{
    public int Block;    //何ブロック目に配置されているか
    public int EnemyNumber; //敵番号（ボスは別の仕組みで生成する？）
    public Vector2 Epos; //XY座標格納
};

public class EnemyGanerator : MonoBehaviour {
    
    private float BlockDist; //次のステージブロックまでの距離
    private GameObject ENEMY;
    public GameObject[] EnemyModel;
    private string FileName;
    private string Stage;
    private TextAsset CSV;
    private List<string[]> EnemyStatus = new List<string[]>();
    private EnemyBox[] EnemyPos;
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

       // EnemyPos = new EnemyBox[EnemyStatus]; ファイルの長さを取得し、それに合わせて長さを変更
    }
	
	// Update is called once per frame
	void Update () {
        for(int n = 0; n < EnemyPos.Length; n++)
        {
            ENEMY = Instantiate(EnemyModel[EnemyPos[n].EnemyNumber]);
        }
	    
	}
}
