using UnityEngine;

namespace GameEntitties {
	public class Base : MonoBehaviour {

		public TeamColor CurrentTeam;
		public int BaseLives;

		private void OnCollisionEnter2D(Collision2D other) {
			var otherUnit = other.gameObject.GetComponent<Unit>();
			if (otherUnit == null || otherUnit.Team == CurrentTeam)
				return;

			BaseLives -= 1;
			Debug.Log(CurrentTeam.ToString() + " base is destroyed unit " + otherUnit.Team);
			Destroy(otherUnit.gameObject);
		}
	}
}
