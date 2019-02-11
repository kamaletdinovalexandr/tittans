using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public float Speed;
	public Team UnitTeam;
	public Power UnitPower;
	public Vector2 TargetPosition;

	void Update () {
		if (TargetPosition != null)

			transform.position = Vector2.MoveTowards(transform.position, 
			                                         TargetPosition, 
			                                         Speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		var unit = collision.gameObject.GetComponent<Unit>();
		if (unit == null)
			return;
		if (unit.UnitTeam != UnitTeam && CanDestroy(unit.UnitPower)) {
			Debug.Log("Unit with power " + UnitPower + " is destroyed unit with power " + unit.UnitPower);
			Destroy(unit.gameObject);
		}
	}

	private bool CanDestroy(Power power) {
		if (UnitPower == Power.scissors && power == Power.paper
		    || UnitPower == Power.paper && power == Power.rock
		    || UnitPower == Power.rock && power == Power.scissors)
			return true;
		
		return false;
	}
}
