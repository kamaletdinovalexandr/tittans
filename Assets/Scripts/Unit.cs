using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public float Speed { get; set; }
	public Team UnitTeam { get; set; }
	public Power UnitPower { get; set; }
	public Vector2 TargetPosition { get; set; }

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
		if (unit.UnitTeam != UnitTeam && IsPowerfull(unit.UnitPower)) {
			Debug.Log("Object with power " + UnitPower + " destroyed by " + unit.UnitPower);
			Destroy(gameObject);
		}
	}

	private bool IsPowerfull(Power power) {
		if (UnitPower == Power.paper && power == Power.scissors
		    || UnitPower == Power.rock && power == Power.paper
		    || UnitPower == Power.scissors && power == Power.rock)
			return true;
		
		return false;
	}
}
