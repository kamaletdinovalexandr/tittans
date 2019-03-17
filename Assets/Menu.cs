using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	[SerializeField] Button NewGameButton;
	[SerializeField] Button ExitGameButton;

	private void Awake() {
		NewGameButton.onClick.AddListener(OnNewGameClick);
		ExitGameButton.onClick.AddListener(OnExitClick);
	}

	private void OnNewGameClick() {
		SceneManager.LoadScene(1);
	}

	private void OnExitClick() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
	}

	private void OnDestroy() {
		NewGameButton.onClick.RemoveListener(OnNewGameClick);
		ExitGameButton.onClick.RemoveListener(OnExitClick);
	}

}
