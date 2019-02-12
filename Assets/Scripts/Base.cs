using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

	public TeamColor CurrentTeam;
	public int BaseLives;

	private void OnTriggerEnter2D(Collider2D other) {
		var otherUnit = other.GetComponent<Unit>();
		if (otherUnit == null || otherUnit.Team == CurrentTeam)
			return;
		
		BaseLives -= 1;
		Destroy(otherUnit.gameObject);
	}
}
