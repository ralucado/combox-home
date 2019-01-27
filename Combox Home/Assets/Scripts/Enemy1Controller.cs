using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour {
	public float _movSpeed;
	private Vector2 _movVel;
	private float _timer;
	private bool _dead;
	private float _spriteTime;
	// Use this for initialization
	void Start () {
		_movSpeed = 20.0f;
		_dead = false;
		_timer = 0f;
		_spriteTime = 0.03f;

	}


	void Update(){
		_timer = _timer+Time.deltaTime;
		if(_dead && _timer >= _spriteTime){
			_timer = 0.0f;
			float factor = 2f;
			transform.localScale = new Vector3 (transform.localScale.x/factor,transform.localScale.y/factor,transform.localScale.z/factor);
			if (transform.localScale.x < 0.1f)
				gameObject.SetActive (false);
		}
		else{
			if (transform.position.x <= -18.5) {
				_movSpeed = Mathf.Abs (_movSpeed);
			} else if (transform.position.x >= 18.5) {
				_movSpeed = -Mathf.Abs (_movSpeed);
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3(_movSpeed * Time.fixedDeltaTime, 0.0f, 0.0f));
	}

	public void kill(){
		_dead = true;
		_movSpeed = 0.0f;
	}

}
