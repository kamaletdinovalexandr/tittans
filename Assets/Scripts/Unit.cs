using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public float Speed;
	public TeamColor Team;
	public Power UnitPower;
	public Vector2 TargetPosition;
	private Vector2 _nearEnemyPosition;

	void Update () {
		if (_nearEnemyPosition != Vector2.zero) {
			MoveTo(_nearEnemyPosition);
		}
			
		
		MoveTo(TargetPosition);
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

		if (IsPowerfullThen(unit.UnitPower))
			_nearEnemyPosition = unit.transform.position;

	}

	private void OnTriggerExit2D(Collider2D other) {
		_nearEnemyPosition = Vector2.zero;
	}

	private bool IsPowerfullThen(Power otherPower) {
		return UnitPower == Power.titan && otherPower != Power.titan
			   || UnitPower == Power.rock && otherPower == Power.scissors
		       || UnitPower == Power.scissors && otherPower == Power.paper
		       || UnitPower == Power.paper && otherPower == Power.rock;
	}
}
