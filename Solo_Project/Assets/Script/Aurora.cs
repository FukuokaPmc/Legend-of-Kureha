using UnityEngine;
using System.Collections;

public class Aurora : MonoBehaviour {
    private ParticleSystem particle;
    private bool BreakFlag;
    private int BreakCount;
	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();
        BreakFlag = false;
        BreakCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(BreakFlag)
        {
            BreakCount++;
        }
        if(!particle.isPlaying && BreakCount >= 20)
        {
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().SightOff();
        }
	
	}

    void OnTriggerEnter(Collider col)
    {
        ParticleSystem.Burst[] burst = new ParticleSystem.Burst[1];
        if (col.gameObject.tag == "Attack")
        {
            particle.Clear();
            burst[0].time = 0.00f;
            burst[0].minCount = 300;
            burst[0].maxCount = 300;
            Debug.Log("破壊");
            particle.loop = false;
            particle.startLifetime = 1.0f;
            particle.startSpeed = 10.0f;
            particle.emission.SetBursts(burst);
            particle.Play();
            BreakFlag = true;
        }

        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<PlayerMove>().IsCurrentState(PlayerState.Dash))
            {
                col.gameObject.GetComponent<PlayerMove>().ChangeState(PlayerState.Refrect);
            }
        }
    }
}
