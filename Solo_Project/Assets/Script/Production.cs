using UnityEngine;
using System.Collections;

public class Production : MonoBehaviour {
    public Vector3[] CameraPos;
	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(false);
        this.transform.localPosition = CameraPos[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartProduction(int nNum)
    {
        this.gameObject.SetActive(true);
        this.transform.localPosition = CameraPos[nNum];
    }

    public void EndProduction()
    {
        this.gameObject.SetActive(false);
    }
}
