using UnityEngine;
using System.Collections;

public class Production : MonoBehaviour {
    public Vector3[] CameraPos;
    private int ProNum;
	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(false);
        this.transform.localPosition = CameraPos[0];
        ProNum = 0;

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 cameratarget;
        cameratarget = transform.GetComponentInParent<PlayerMove>().transform.position;
        cameratarget.y += 5.0f;
        cameratarget.z -= 3.0f;
        this.transform.localPosition = CameraPos[0];
        this.transform.LookAt(cameratarget);

    }

    public void StartProduction(int nNum)
    {
        this.gameObject.SetActive(true);
        this.transform.localPosition = CameraPos[nNum];
        ProNum = nNum;
    }

    public void EndProduction()
    {
        this.gameObject.SetActive(false);
    }
}
