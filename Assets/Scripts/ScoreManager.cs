using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text statusText;

	[SerializeField]
	private float countingDelay;
	[SerializeField]
	private int countBy;

	private int scoreCounter;
	private int score;
	private string status;
	// Use this for initialization
	void Start () {

		scoreCounter = 0;
		score = PlayerPrefs.GetInt("score",0);
		status = PlayerPrefs.GetString("passed", "failed");
		StartCoroutine(CalculateScore());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator CalculateScore() {
		while (scoreCounter <= score) {
			scoreText.text = "Score: " + scoreCounter;
			yield return new WaitForSeconds(countingDelay);
			
			scoreCounter += countBy;
		}

		scoreCounter = 0;
		scoreCounter = score;
		statusText.text = status;
	}
}
