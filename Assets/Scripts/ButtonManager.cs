using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	public void GoToScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}
