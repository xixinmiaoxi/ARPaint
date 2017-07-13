using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disapper : MonoBehaviour {

    private float PassTime;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PassTime += Time.deltaTime;
        if(PassTime > 0.8f)
        {
            PassTime = 0;//时间归零，以便下次进入时重新计时
            gameObject.SetActive(false);//识别成功信息消失
        }
	}
}
