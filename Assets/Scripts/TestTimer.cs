using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestTimer : MonoBehaviour {

	[SerializeField]
	private Image timerFill;
	[SerializeField]
	private float timeLimit;

	private float elapsedTime;
	private bool loading;
	// Use this for initialization
	void Start () {
		elapsedTime = 0;
		loading = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (loading) {
			elapsedTime += Time.deltaTime;
			float fillAmount = elapsedTime / timeLimit;
			timerFill.fillAmount = fillAmount;

			if (elapsedTime >= timeLimit) {
				loading = false;
				elapsedTime = 0;
			}
		}
	}

	public void Flip() {
		Debug.Log("Fill amount: " + timerFill.fillAmount);
		if (timerFill.fillAmount >= 1) {
			timerFill.fillAmount = 0;
		}
		loading = !loading;
	}
}
