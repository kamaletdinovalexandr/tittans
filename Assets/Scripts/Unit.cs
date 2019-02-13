using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public float Speed;
	public float AlternateSpeed { get; set; }
	public int Lives;
	public TeamColor Team;
	public Power UnitPower;
	public Vector2 TargetPosition;
	private Transform _nearEnemyTransform;
	private bool _runAway;

	void Update () {
		if (Speed < 0.01f)
			return;
		
		 Vector2 targetPosition = TargetPosition;

		 if (_nearEnemyTransform != null)
			 targetPosition = _nearEnemyTransform.position;
		 
		if (_runAway && _nearEnemyTransform != null) {
			targetPosition = transform.position * -1;
		}

		MoveTo(targetPosition);
	}

	private void MoveTo(Vector2 position) {
		var speed = AlternateSpeed > 0 ? AlternateSpeed : Speed;
		transform.position = Vector2.MoveTowards(transform.position, position,speed * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		var otherUnit = other.gameObject.GetComponent<Unit>();
		if (otherUnit == null || Team == otherUnit.Team)
			return;
			
		if (IsPowerfullThen(otherUnit.UnitPower)) {
			Debug.Log("Unit with power " + UnitPower + " is damaged unit " + otherUnit.UnitPower);
			otherUnit.TakeDamage();
		}
		
		if (UnitPower == Power.mine)
			TakeDamage();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		var unit = other.gameObject.GetComponent<Unit>();
		if (unit == null || Team == unit.Team)
			return;

		if (UnitPower == Power.mine && unit.UnitPower != Power.mine) {
			Speed = 5f;
			_nearEnemyTransform = unit.transform;
			_runAway = false;
			return;
		}

		if (UnitPower == Power.tower && unit.UnitPower != Power.tower) {
			unit.AlternateSpeed = 0.2f;
			return;
		}
			
		if (UnitPower == Power.rock && unit.UnitPower == Power.scissors 
		    || UnitPower == Power.paper && unit.UnitPower == Power.rock) {
			_nearEnemyTransform = unit.transform;
			_runAway = false;
			return;
		}
		
		_nearEnemyTransform = unit.transform;
		_runAway = true;
	}

	private void OnTriggerExit2D(Collider2D other) {
		var unit = other.gameObject.GetComponent<Unit>();
		if (unit == null || Team == unit.Team)
			return;
		
		_nearEnemyTransform = null;
		_runAway = false;
		AlternateSpeed = 0f;
		if (UnitPower == Power.mine)
			Speed = 0f;
	}

	private bool IsPowerfullThen(Power otherPower) {
		return UnitPower == Power.mine && otherPower != Power.mine 
			|| UnitPower == Power.titan && otherPower != Power.titan
			|| UnitPower == Power.rock && otherPower == Power.scissors
		    || UnitPower == Power.scissors && otherPower == Power.paper
		    || UnitPower == Power.paper && otherPower == Power.rock
			|| otherPower == Power.tower;
	}

	public void TakeDamage() {
		Lives--;
		if (Lives <= 0)
			Debug.Log("No lives left with power: " + UnitPower);
			Destroy(gameObject);
	}
}
