using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Rigidbody2D _rigidbody;
	private float _movSpeed;
	private float _jumpSpeed;
	private Vector2 _movVel;
	public Vector2 _jmpVel;
	private bool _startJump;
	private bool _startDunk;
	private bool _bufferedJump;
	private float _timerJump;
	private float _timerDunk;
	private bool _bufferedDunk;


	private BoxCollider2D _lastFloorCollider;
	public LayerMask mask;
	public float _maxBufferTime;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody2D> ();
		_movSpeed = 30.0f;
		_jumpSpeed = 180.0f;
		_timerJump = 0.0f;
		_timerDunk = 0.0f;
		_startJump = false;
		_startDunk = false;
		_bufferedJump = false;
		_bufferedDunk = false;
		_maxBufferTime = 0.2f;
	}

	void Update(){
		_timerJump = _timerJump+Time.deltaTime;
		_timerDunk = _timerDunk+Time.deltaTime;
		float mv_horizontal = Input.GetAxis ("Horizontal");
		if (mv_horizontal < 0f) {
			gameObject.GetComponentInChildren<LightController> ().move (-1);
		} else if (mv_horizontal > 0f) {
			gameObject.GetComponentInChildren<LightController> ().move (1);
		}
		_movVel = (new Vector2 (-mv_horizontal, 0.0f).normalized * _movSpeed);
		if (isGrounded () && Input.GetKeyDown (KeyCode.UpArrow)) {
			_startJump = true; 
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			_bufferedJump = true;
			_timerJump = 0.0f;
		} else {
			if (isGrounded() && _lastFloorCollider != null && !_lastFloorCollider.enabled) {
				_lastFloorCollider.enabled = true;
			}
			if (isGrounded () && _bufferedJump && _timerJump < _maxBufferTime) {
				_startJump = true;
				_timerJump = 0.0f;
				_bufferedJump = false;
				Debug.Log ("Buffer Jumping");
			} else {
				_startJump = false;
			}
		}

		if (canGoDown () && Input.GetKeyDown (KeyCode.DownArrow)) {
			_startDunk = true;
			_lastFloorCollider.enabled = false;
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			_bufferedDunk = true;
			_timerDunk = 0.0f;
		} else {
			if (canGoDown() && _bufferedDunk && _timerDunk < _maxBufferTime) {
				_startDunk = true;
				_lastFloorCollider.enabled = false;
				_timerDunk = 0.0f;
				_bufferedDunk = false;
				Debug.Log ("Buffer Dunking");
			} else {
				_startDunk = false;
			}
		}

		if (_startJump) {
			_jmpVel = new Vector2 (0.0f, _jumpSpeed);
		} else if (_startDunk) {
			_jmpVel = new Vector2 (0.0f, -(_movSpeed)); 
		} else if (!isGrounded ()) {
			//_jmpVel.y = Mathf.Max (0.0f, _jmpVel.y - (_jumpSpeed / 8.0f));
			_jmpVel.y = _jmpVel.y - (_jumpSpeed / 8.0f);

		}


	}

	void FixedUpdate(){
		_rigidbody.MovePosition(_rigidbody.position + (_movVel + _jmpVel) * Time.fixedDeltaTime );
		//Debug.DrawLine (transform.position, transform.position + (Vector2.down * 0.1f));
	}

	bool canGoDown(){
		RaycastHit2D hit = Physics2D.Raycast (transform.position-new Vector3(0.0f, 2.0f, 0.0f), Vector2.down, 0.1f, mask);
		if (hit && hit.collider.gameObject.CompareTag ("DownGround")){
			_lastFloorCollider = hit.collider.GetComponent<BoxCollider2D>();
			return true;
		}
		return false;
	}

	bool isGrounded(){			
		return Physics2D.Raycast (transform.position-new Vector3(0.0f, 2.0f, 0.0f), Vector2.down, 0.1f, mask);
	}

}
