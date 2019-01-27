using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public Canvas _comboCanvas;
	private bool _invincible;
	private float _invincibleTimer;
	public float _maxInvincibleTime;
	private float _maxAnimTime;
	private float _currAnimTime;
	private bool _shielded;
	public GameObject _sideLight;
	public GameObject _wholeLight;
	private BoxCollider2D _lastFloorCollider;
	public LayerMask mask;
	public float _maxBufferTime;
	private bool _lastState;
	private float _shieldMaxTime;
	private float _shieldCurrentTime;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody2D> ();
		_movSpeed = 20.0f;
		_jumpSpeed = 180.0f;
		_timerJump = 0.0f;
		_timerDunk = 0.0f;
		_startJump = false;
		_startDunk = false;
		_bufferedJump = false;
		_bufferedDunk = false;
		_maxBufferTime = 0.2f;
		_invincible = false;
		_invincibleTimer = 0f;
		_maxInvincibleTime = 1f;
		_maxAnimTime = 0.02f;
		_currAnimTime = 0f;
		_shielded = false;
		_lastState = true;
		_shieldMaxTime = _shieldCurrentTime = 5f;

	}

	void Update(){
		_timerJump = _timerJump+Time.deltaTime;
		_timerDunk = _timerDunk+Time.deltaTime;
		if (_shielded) {
			_shieldCurrentTime = _shieldCurrentTime - Time.deltaTime;
			if (_shieldCurrentTime <= 0f) {

			}
		}
		_invincibleTimer = _invincibleTimer + Time.deltaTime;
		if (_invincible) {
			_currAnimTime = _currAnimTime + Time.deltaTime;
			if (_invincibleTimer >= _maxInvincibleTime) {
				_invincible = false;
				_currAnimTime = 0f;
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
				gameObject.GetComponentInChildren<MeshRenderer> ().enabled = true;
			} else if (_currAnimTime >= _maxAnimTime){
				_currAnimTime = 0f;
				gameObject.GetComponent<MeshRenderer> ().enabled = !_lastState;
				gameObject.GetComponentInChildren<MeshRenderer> ().enabled =  !_lastState;
				_lastState = !_lastState;
			}
		}
		float mv_horizontal = Input.GetAxis ("Horizontal");
		if (!_shielded){
			if (mv_horizontal < 0f) {
				gameObject.GetComponentInChildren<LightController> ().move (-1);
			} else if (mv_horizontal > 0f) {
				gameObject.GetComponentInChildren<LightController> ().move (1);
			}
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
			} else {
				_startDunk = false;
			}
		}
		if (Input.GetKeyDown (KeyCode.Space) && _shieldCurrentTime > 0f) {
			_shielded = true;
			_sideLight.gameObject.SetActive (false);
			_wholeLight.gameObject.SetActive (true);

		}
		if (_shieldCurrentTime <= 0f ||  Input.GetKeyUp (KeyCode.Space)) {
			_shielded = false;
			_shieldCurrentTime = Mathf.Max(0f, _shieldCurrentTime);
			_sideLight.gameObject.SetActive (true);
			_wholeLight.gameObject.SetActive (false);
		}
		if (Input.GetKeyDown (KeyCode.Tab)) { 
			_shieldCurrentTime = _shieldMaxTime; 		//hacks
		}
		if (_startJump) {
			_jmpVel = new Vector2 (0.0f, _jumpSpeed);
		} else if (_startDunk) {
			_jmpVel = new Vector2 (0.0f, -(_movSpeed)); 
		} else if (!isGrounded ()) {
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy") && !other.gameObject.GetComponent<Enemy1Controller>().isDead()) {
			other.gameObject.GetComponent<Enemy1Controller> ().kill ();
			if (!_invincible && !_shielded) {
				_comboCanvas.GetComponentInChildren<ComboController> ().loseCombo ();
				_invincible = true;
				_invincibleTimer = 0f;
			}
		}
	}

}
