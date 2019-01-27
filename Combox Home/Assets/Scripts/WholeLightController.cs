using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WholeLightController : MonoBehaviour {
	public Canvas _comboCanvas;

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
