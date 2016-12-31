using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Loader : Singleton<Loader> {

	[SerializeField]
	private GameObject timerObject;
	[SerializeField]
	private Image timerFiller;

	private float elapsedTime;
	private bool loading;
	private float timeLimit;

	private Action method;

	public bool Loading {
		get {return loading;}
	}

	// Use this for initialization
	void Start () {
		elapsedTime = 0;
		timeLimit = 0;
		loading = false;
		method = null;

		timerObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (loading) {
			elapsedTime += Time.deltaTime;
			
			float fillAmount = elapsedTime / timeLimit;
			timerFiller.fillAmount = fillAmount;

			if (elapsedTime >= timeLimit) {
				loading = false;

				HideTimerObject();

				method();
				method = null;
			}
		}
	}

	public void CancelTimer() {
		this.timeLimit = 0;
		this.elapsedTime = 0;
		this.loading = false;
		this.method = null;

		HideTimerObject();
	}

	public void SetTimer(float timeLimit, Action method) {
		this.timeLimit = timeLimit;
		this.elapsedTime = 0;
		this.loading = true;
		this.method = method;

		timerObject.SetActive(true);
	}

	void HideTimerObject() {
		timerObject.SetActive(false);
		timerFiller.fillAmount = 0;
	}
}
