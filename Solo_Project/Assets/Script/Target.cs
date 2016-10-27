using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Target : MonoBehaviour {
    private Camera camera;
    private Camera SubCamera;
    private Transform target;
    private RectTransform NowTrans;
    private bool bMode;
	// Use this for initialization
	void Start () {
        NowTrans = this.GetComponent<RectTransform>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SubCamera = GameObject.FindGameObjectWithTag("SubCamera").GetComponent<Camera>();
        bMode = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos;
        if (!bMode)
            pos = camera.WorldToViewportPoint(target.position);
        else
            pos = SubCamera.WorldToViewportPoint(target.position);

        NowTrans.anchorMax = pos;
        NowTrans.anchorMin = pos;
	}

    public void TargetLockOn(Transform trans)
    {
        target = trans;
        
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void CameraChange()
    {
        bMode = bMode ? false : true;
    }
}
