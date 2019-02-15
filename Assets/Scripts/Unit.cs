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
	public Transform NearEnemyTransform;
	public bool RunAway;
	private ICollideBehaviour _collideBehaviour;

	private void Awake() {
		switch (UnitPower) {
			case Power.mine:
				_collideBehaviour = new MineBehaviour(this);
				break;
			case Power.titan:
				_collideBehaviour = new TitanBehaviour(this);
				break;
			case Power.tower:
				_collideBehaviour = new TowerBehaviour(this);
				break;
			case Power.rock:
				_collideBehaviour = new RockBehaviour(this);
				break;
			case Power.paper:
				_collideBehaviour = new PaperBehaviour(this);
				break;
			case Power.scissors:
				_collideBehaviour = new ScissorsBehaviour(this);
				break;
			
		}
	}

	void Update () {
		if (Speed < 0.01f)
			return;
		
		 Vector2 targetPosition = TargetPosition;

		 if (NearEnemyTransform != null)
			 targetPosition = NearEnemyTransform.position;
		 
		if (RunAway && NearEnemyTransform != null) {
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

		_collideBehaviour.DoCollide(unit);
	}

	private void OnTriggerExit2D(Collider2D other) {
		var unit = other.gameObject.GetComponent<Unit>();
		if (unit == null || Team == unit.Team)
			return;
		
		NearEnemyTransform = null;
		RunAway = false;
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
