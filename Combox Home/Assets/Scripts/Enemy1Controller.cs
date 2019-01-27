using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour {
	public float _movSpeed;
	private Vector2 _movVel;
	private float _timer;
	private bool _dead;
	private float _spriteTime;
	private float _offset;
	private bool _spawning;
	private float _spawnTimer;
//	private float _spawnMaxTime;
	// Use this for initialization
	void Start () {
		_movSpeed = 10.0f;
		_dead = false;
		_timer = 0f;
		_spriteTime = 0.03f;
		_offset = Random.Range (0f, 1f);
		_spawning = true;
		_spawnTimer = 0.0f;
//		_spawnMaxTime = 0.5f;
		transform.localScale = new Vector3 (0.1f,0.1f,0.05f);

	}


	void Update(){
		_timer = _timer+Time.deltaTime;
		_spawnTimer = _spawnTimer+Time.deltaTime;

		if(_dead && _timer >= _spriteTime){
			_timer = 0.0f;
			float factor = 2f;
			transform.localScale = new Vector3 (transform.localScale.x/factor,transform.localScale.y/factor,transform.localScale.z/factor);
			if (transform.localScale.x < 0.1f)
				gameObject.SetActive (false);
		}
		else if(_spawning && _spawnTimer >= _spriteTime){
			_spawnTimer = 0.0f;
			float factor = 2f;
			transform.localScale = new Vector3 (transform.localScale.x*factor,transform.localScale.y*factor,transform.localScale.z*factor);
			if (transform.localScale.x >= 2f) {
				_spawning = false;
				transform.localScale = new Vector3 (2f,2f,1f);

			}
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
		transform.Translate (new Vector3(_movSpeed * Time.fixedDeltaTime, Mathf.Sin(_offset+Time.time*20f)*0.1f, 0.0f));
	}

	public bool isDead(){
		return _dead || _spawning;
	}

	public void kill(){
		_dead = true;
		_movSpeed = 0.0f;
		gameObject.GetComponent<CircleCollider2D> ().enabled = false;
		gameObject.tag = "Untagged";
	}

}
