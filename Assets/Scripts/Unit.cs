﻿using System.Collections;
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
		var otherUnit = collision.gameObject.GetComponent<Unit>();
		if (otherUnit == null)
			return;
		
		if (otherUnit.UnitTeam != UnitTeam && !IsPowerfull(otherUnit.UnitPower)) {
			Debug.Log("Unit with power " + UnitPower + " is destroyed by " + otherUnit.UnitPower);
			Destroy(gameObject);
		}
	}

	private bool IsPowerfull(Power otherPower) {
		return UnitPower == Power.scissors && otherPower == Power.rock
		       || UnitPower == Power.paper && otherPower == Power.scissors
		       || UnitPower == Power.rock && otherPower == Power.paper;
	}
}
