using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	[SerializeField]
	private GameObject desk;

	[SerializeField]
	private List<GameObject> spawnPoints;
	[SerializeField]
	private float timeLimitMinutes;
	[SerializeField]
	private Vector2 customerDelayRange;

	[SerializeField]
	private LayerMask draggableLayerMask;
	[SerializeField]
	private Vector2 dragScaleFactor;

	[SerializeField]
	private LayerMask peopleLayerMask;

	//people
	[SerializeField]
	private GameObject boy;
	[SerializeField]
	private GameObject girl;
	[SerializeField]
	private GameObject lady;
	[SerializeField]
	private GameObject man;

	[SerializeField]
	private float boyTimeLimit;
	[SerializeField]
	private float girlTimeLimit;
	[SerializeField]
	private float ladyTimeLimit;
	[SerializeField]
	private float manTimeLimit;


	//gifts
	[SerializeField]
	private GameObject gun;
	[SerializeField]
	private GameObject doll;

	//gun wrappers
	[SerializeField]
	private GameObject gunPlain;
	[SerializeField]
	private GameObject gunPolka;
	[SerializeField]
	private GameObject gunStripes;
	[SerializeField]
	private GameObject gunRibbon;

	//doll wrappers
	[SerializeField]
	private GameObject dollPlain;
	[SerializeField]
	private GameObject dollPolka;
	[SerializeField]
	private GameObject dollStripes;
	[SerializeField]
	private GameObject dollRibbon;

	//customers for level
	[SerializeField]
	private bool manAvailable;
	[SerializeField]
	private bool ladyAvailable;
	[SerializeField]
	private bool boyAvailable;
	[SerializeField]
	private bool girlAvailable;

	[SerializeField]
	private GameObject exit;

	public GameObject Exit {
		get {return exit;}
	}

	private Gift gift; 
	private bool dragGift;
	private Vector2 giftOriginalScale;
	private int score;
	private int customers;

	private bool started;
	private bool gameOver;
	private float timeLapse;

	[SerializeField]
	private float passingFactor = 0.5f;
	[SerializeField]
	private float startDelay = 3f;

	[SerializeField]
	private Text timerText;
	[SerializeField]
	private Animator timerTextAnimator;

	[SerializeField]
	private int startHour = 8;
	[SerializeField]
	private int startMinutes = 0;
	[SerializeField]
	private float secondsInMinuteGame = 0.5f; // 1 minute in gameworld = ? seconds

	private float secondsCounter;
	private float timeLimitSeconds;
	private float alertLimit;

	public Gift Gift {
		get {return gift;}
	}

	IEnumerator StartGame(float f) {
		yield return new WaitForSeconds(f);
		started = true;
		timerTextAnimator.Play("TimerShrink");
		StartCoroutine(AddCustomers());
	}

	// Use this for initialization
	void Start () {
		dragGift = false;
		score = 0;

		started = false;
		gameOver = false;

		secondsCounter = 0;
		timeLimitSeconds = timeLimitMinutes * 60f;
		alertLimit = (timeLimitSeconds > 30f) ? timeLimitSeconds - 30f : 0;

		if (manAvailable) Customer.available.Add(Customer.Type.MAN);
		if (ladyAvailable) Customer.available.Add(Customer.Type.LADY);
		if (boyAvailable) Customer.available.Add(Customer.Type.BOY);
		if (girlAvailable) Customer.available.Add(Customer.Type.GIRL);

		StartCoroutine(StartGame(startDelay));
	}
	
	// Update is called once per frame
	void Update () {
		
		if (started && !gameOver) {
			timeLapse += Time.deltaTime;

			secondsCounter += Time.deltaTime;
			if (secondsCounter >= secondsInMinuteGame) {
				secondsCounter = 0;
				startMinutes++;
				if (startMinutes >= 60) {
					startHour++;
					startMinutes = 0;
					if (startHour >= 24) {
						startHour = 0;
					}
				}
			}

			string hour =((startHour >= 10) ? "" + startHour : "0" + startHour);
			string minute =((startMinutes >= 10) ? "" + startMinutes : "0" + startMinutes);
			timerText.text = hour + ":" + minute;

			if (timeLapse >= timeLimitSeconds) {
				gameOver = true;
				timerText.text = "CLOSED";
				StartCoroutine(CloseStore());
			}

			if (timeLapse >= alertLimit) {
				timerText.color = new Color(1,0,0);
			}
		}


		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, draggableLayerMask);
			if (hit && !Loader.Instance.Loading) {
				dragGift = true;
				gift.transform.localScale = new Vector2(giftOriginalScale.x * dragScaleFactor.x, giftOriginalScale.y * dragScaleFactor.y);
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			Customer customer = gift.Customer;
			if (customer != null && customer.Asking) {
				customer.ReceiveGift(gift);
				gift = null;
			} else {
				gift.transform.position = desk.transform.position;
				gift.transform.localScale = new Vector2(giftOriginalScale.x, giftOriginalScale.y);
			}

			dragGift = false;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			int randIndex = Random.Range(0,spawnPoints.Count);
			spawnPoints[randIndex].GetComponent<Slot>().Clear();
		}

		if (dragGift) {
			gift.transform.position = mousePos;
		}

	}

	IEnumerator CloseStore() {
		while (gameOver) {
			if (AllSlotsAvailable()) {
				Debug.Log("Closing...");
				if (Passed()) {
					PlayerPrefs.SetString("passed", "Passed");
				} else {
					PlayerPrefs.SetString("passed", "Failed");
				}
				PlayerPrefs.SetInt("score", score);
				SceneManager.LoadScene("Score");
			}
			yield return new WaitForSeconds(0.5f); //check every 0.5 seconds
		}
	}

	bool AllSlotsAvailable() {
		for (int i = 0; i < spawnPoints.Count; i++) {
				Slot slot = spawnPoints[i].GetComponent<Slot>();
				if (slot.IsTaken) return false;
		}
		
		return true;
	}

	public void ThrowGiftToTrash() {
		//TODO calculate costs


		Destroy(gift.gameObject);
		gift = null;
	}

	public void PickedGift(Gift.Type type) {
		Debug.Log("Picked gift: " + type);

		if (gift != null) return;
		GetGift(type);
	}

	public void PickedWrapper(Gift.Type type) {
		Debug.Log("Picked wrapper: " + type);

		if (gift == null) return;
		WrapGift(type);
	}

	void SetGift(Gift gift) {
		this.gift = gift;
		giftOriginalScale = this.gift.transform.localScale;
	}

	void WrapGift(Gift.Type type) {
		//create the gift
		GameObject newWrapper = CreateGift(type, desk);
		Gift newGiftScript = newWrapper.GetComponent<Gift>();
		newGiftScript.SetChild(gift);
		SetGift(newGiftScript);
	}

	void GetGift(Gift.Type type) {
		//create the gift
		GameObject newGift = CreateGift(type, desk);
		Gift newGiftScript = newGift.GetComponent<Gift>();
		SetGift(newGiftScript);
	}

	void GetRandomGift() {
		int index = Random.Range(0, Gift.gifts.Length);

		//create the gift
		GameObject newGift = CreateGift(Gift.gifts[index], desk);
		Gift newGiftScript = newGift.GetComponent<Gift>();
		SetGift(newGiftScript);
	}

	GameObject CreateGift(Gift.Type type, GameObject at) {
		GameObject prefab = null;
		switch (type) {
			//gifts
			case Gift.Type.GUN:
				prefab = gun;
			break;
			case Gift.Type.DOLL:
				prefab = doll;
			break;

			//gun wrappers
			case Gift.Type.GUN_PLAIN_WRAPPER:
				prefab = gunPlain;
			break;
			case Gift.Type.GUN_POLKA_WRAPPER:
				prefab = gunPolka;
			break;
			case Gift.Type.GUN_STRIPES_WRAPPER:
				prefab = gunStripes;
			break;
			case Gift.Type.GUN_RIBBON_WRAPPER:
				prefab = gunRibbon;
			break;

			//doll wrappers
			case Gift.Type.DOLL_PLAIN_WRAPPER:
				prefab = dollPlain;
			break;
			case Gift.Type.DOLL_POLKA_WRAPPER:
				prefab = dollPolka;
			break;
			case Gift.Type.DOLL_STRIPES_WRAPPER:
				prefab = dollStripes;
			break;
			case Gift.Type.DOLL_RIBBON_WRAPPER:
				prefab = dollRibbon;
			break;

			default:
			break;
		}

		GameObject newGift = Instantiate(prefab, at.transform.position, Quaternion.identity) as GameObject;
		Gift newGiftScript = newGift.AddComponent<Gift>();
		newGiftScript.GiftType = type;
		
		return newGift;
	}

	/*
	void PickGift(RaycastHit2D hit) {
		if (!hit) return;

		Collider2D other = hit.collider;
		if (other.tag == "Gift") {
			other.GetComponent<Gift>().SetTarget(desk);
			//Debug.Log("Picked: " + other.gameObject);
		}
	}
	*/

	IEnumerator AddCustomers() {
		while (!gameOver) {
			int availableIndex =  AvailableSlot();
			if (availableIndex >= 0) {
				yield return new WaitForSeconds(Random.Range(customerDelayRange.x, customerDelayRange.y));
				if (!gameOver) {
					AddRandomCustomer(spawnPoints[availableIndex]);
					
					//add counter
					customers++;
				}

			} else yield return null;
		}
	}

	void AddRandomCustomer(GameObject spawnPoint) {
		int customerIndex = Random.Range(0, Customer.available.Count);
		AddCustomer(Customer.available[customerIndex], spawnPoint);
	}

	void AddCustomer(Customer.Type type, GameObject spawnPoint) {
		Slot slotScript = spawnPoint.GetComponent<Slot>();

		float timeLimit = 0;

		GameObject customer = null;
		switch (type) {
			case Customer.Type.BOY:
			customer = Instantiate(boy, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			timeLimit = boyTimeLimit;
			break;

			case Customer.Type.GIRL:
			customer = Instantiate(girl, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			timeLimit = girlTimeLimit;
			break;

			case Customer.Type.LADY:
			customer = Instantiate(lady, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			timeLimit = ladyTimeLimit;
			break;

			case Customer.Type.MAN:
			customer = Instantiate(man, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			timeLimit = manTimeLimit;
			break;
		}

		Customer customerScript = customer.AddComponent<Customer>();
		customerScript.SetSlot(slotScript);
		customerScript.Bubble = BubbleManager.Instance.CreateBubbleAtParent(customer).GetComponent<Bubble>();
		customerScript.StartTimer(timeLimit);
	}

	int AvailableSlot() {
		for (int i = 0; i < spawnPoints.Count; i++) {

			GameObject spawnPoint = spawnPoints[i];
			if(spawnPoint == null) continue;

			Slot slotScripts = spawnPoint.GetComponent<Slot>();
			if (slotScripts == null) continue;

			if (!slotScripts.IsTaken) return i;
		}
		return -1;
	}

	public void AddScore(int n) {
		if (n <= 0) return;
		score += n;

		Debug.Log("Score: : " + score);
	}

	public bool Passed() {
		int maxScore = customers * 100;
		int passingScore = (int)(maxScore * passingFactor);
		
		Debug.Log("Max score: " + maxScore);
		Debug.Log("Passing score: " + passingScore);
		Debug.Log("Score: " + score);

		if (score >= passingScore) {
			return true;
		}

		return false;
	}

}
