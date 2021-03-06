﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour {
	public Canvas _comboCanvas;

	public void move(int dir){
		if ((dir < 0 && transform.localPosition.x < 0f) || (dir > 0 && transform.localPosition.x > 0f) ){
			gameObject.transform.localPosition= new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		}
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
			other.gameObject.GetComponent<Enemy1Controller> ().kill ();
			_comboCanvas.GetComponentInChildren<ComboController> ().addKill ();
			Debug.Log ("add a kill");
		}
	}

}
