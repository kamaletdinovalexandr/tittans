using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameCore;

namespace PlayerInput {
	public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		[SerializeField] private Power _power;
		private GameObject draggingObject = null;
		private Image draggingImage = null;
		private Color originalColor;

		public void OnBeginDrag(PointerEventData eventData) {
			draggingObject = Instantiate(gameObject, transform.parent, true);
			draggingImage = draggingObject.GetComponent<Image>();
			originalColor = draggingImage.color;
		}

		public void OnDrag(PointerEventData eventData) {
			if (draggingObject == null)
				return;

			draggingObject.transform.position = Input.mousePosition;
			bool isSpawn = GameController.Instance.IsBlueSpawnAvailable(_power, GetWorldMousePosition());
			draggingImage.color = isSpawn ? originalColor : Color.red;
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (GameController.Instance.IsBlueSpawnAvailable(_power, GetWorldMousePosition())) {
				GameController.Instance.PlayerSpawn(_power, GetWorldMousePosition());
			}

			Destroy(draggingObject);
			draggingObject = null;
			draggingImage = null;
		}

		private Vector2 GetWorldMousePosition() {
			return Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
