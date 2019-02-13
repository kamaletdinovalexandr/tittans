using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public float Speed;
	public TeamColor Team;
	public Power UnitPower;
	public Vector2 TargetPosition;
	private Transform _nearEnemyTransform;
	private bool _runAway;

	void Update () {
		if (!_runAway)
			MoveTo(_nearEnemyTransform != null ? (Vector2)_nearEnemyTransform.position : TargetPosition);
		else {
			MoveTo(TargetPosition * -1);
		}
	}

	private void MoveTo(Vector2 position) {
		transform.position = Vector2.MoveTowards(transform.position, position,Speed * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		var otherUnit = other.gameObject.GetComponent<Unit>();
		if (otherUnit == null)
			return;
		
		if (otherUnit.Team != Team && IsPowerfullThen(otherUnit.UnitPower)) {
			Debug.Log("Unit with power " + UnitPower + " is destroyed unit " + otherUnit.UnitPower);
			Destroy(otherUnit.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		var unit = other.gameObject.GetComponent<Unit>();
		if (unit == null)
			return;

		if (Team == unit.Team)
			return;

		if (UnitPower == Power.rock && unit.UnitPower == Power.scissors) {
			_nearEnemyTransform = unit.transform;
			return;
		}

		if (UnitPower != Power.paper || unit.UnitPower != Power.rock) 
			return;
		
		_nearEnemyTransform = unit.transform;
		_runAway = true;
	}

	private void OnTriggerExit2D(Collider2D other) {
		_nearEnemyTransform = null;
		_runAway = false;
	}

	private bool IsPowerfullThen(Power otherPower) {
		return UnitPower == Power.titan && otherPower != Power.titan
			   || UnitPower == Power.rock && otherPower == Power.scissors
		       || UnitPower == Power.scissors && otherPower == Power.paper
		       || UnitPower == Power.paper && otherPower == Power.rock;
	}
}
