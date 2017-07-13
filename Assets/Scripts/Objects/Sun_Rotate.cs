using UnityEngine;
using System.Collections;

public class Sun_Rotate : MonoBehaviour {

	public float speed=8.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,speed*Time.deltaTime,0,Space.Self );
	}
}
