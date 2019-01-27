using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyController : MonoBehaviour {
	public GameObject _enemy;
	private float _currentTime;
	private float _enemySpawnTime;
	private float _posX = 18.25f;
	private float _posY = -17.46f;
	private float _posZ = 0.5f;
	// Use this for initialization
	void Start () {
		_currentTime = 0f;
		_enemySpawnTime = Random.Range (0.5f, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		_currentTime = _currentTime + Time.deltaTime;
		if (_currentTime >= _enemySpawnTime) {
			_currentTime = 0f;
			spawnEnemy ();
		}

	}

	void spawnEnemy(){
		float onetozero = Random.Range (-1f, 1f);
		float rangtres = Random.Range (0, 3);
		Vector3 pos = new Vector3 (_posX * onetozero, _posY + (10f * rangtres), _posZ);
		GameObject newEnemy = Instantiate(_enemy,pos, Quaternion.identity);
		newEnemy.SetActive (true);
	}
}
