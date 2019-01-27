using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ComboController : MonoBehaviour {

	public Image _comboImage;
	public Text _comboText;
	private float _timeSinceKill;
	public float _comboMaxTime;
	private int _killCount;
	// Use this for initialization
	void Start () {
		_timeSinceKill = 0f;
		_killCount = 0;
		_comboMaxTime = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		_timeSinceKill = _timeSinceKill + Time.deltaTime;
		if (_timeSinceKill >= _comboMaxTime) {
			_timeSinceKill = _comboMaxTime;
			_killCount = 0;
		}
		float timeRemaining = _comboMaxTime - _timeSinceKill;
		float percentage = timeRemaining / _comboMaxTime;
		_comboImage.fillAmount = percentage;
		_comboText.text = _killCount.ToString ();
	}

	public void addKill(){
		_timeSinceKill = 0f;
		++_killCount;
	}

	public void loseCombo(){
		_timeSinceKill = _comboMaxTime;
		_killCount = 0;
	}
}
