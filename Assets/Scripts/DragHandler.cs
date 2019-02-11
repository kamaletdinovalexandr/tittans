using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField] private Power _power;
	private Transform draggingObject = null;

	public void OnBeginDrag(PointerEventData eventData) {
		draggingObject = GameController.Instance.PlayerSpawn(_power, GetWorldMousePosition());
//		Debug.Break();
	}

	public void OnDrag(PointerEventData eventData) {
		 if (draggingObject != null)
			draggingObject.position = GetWorldMousePosition();
	}

	public void OnEndDrag(PointerEventData eventData) {
		draggingObject = null;	
	}

	private Vector2 GetWorldMousePosition() {
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
