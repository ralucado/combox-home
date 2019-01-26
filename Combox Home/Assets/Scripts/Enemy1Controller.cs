using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour {
	public float _movSpeed;
	private Vector2 _movVel;

	// Use this for initialization
	void Start () {
		_movSpeed = 20.0f;
	}


	void Update(){
		if (transform.position.x <= -18.5) {
			_movSpeed = Mathf.Abs (_movSpeed);
		} else if (transform.position.x >= 18.5) {
			_movSpeed = -Mathf.Abs (_movSpeed);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3(_movSpeed * Time.fixedDeltaTime, 0.0f, 0.0f));
	}


}
