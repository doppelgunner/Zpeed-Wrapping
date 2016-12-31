using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	public static T Instance {
		get {
				T foundObject = FindObjectOfType<T>();

				if (instance == null) {
					instance = foundObject;
				} else if (instance != foundObject) {
					Destroy(foundObject);
				}
				return instance;
			}
	}
}
