using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField] private Power _power;
	private GameObject draggingObject = null;

	public void OnBeginDrag(PointerEventData eventData) {
		draggingObject = Instantiate(this.gameObject, transform.position, Quaternion.identity);
	}

	public void OnDrag(PointerEventData eventData) {
		 
		draggingObject.transform.position = GetWorldMousePosition();
	}

	public void OnEndDrag(PointerEventData eventData) {
		Destroy(draggingObject);
		draggingObject = null;
		GameController.Instance.PlayerSpawn(_power, GetWorldMousePosition());
	}

	private Vector2 GetWorldMousePosition() {
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
