using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour {

	private Slot slot;
	private Bubble bubble;

	private float elapsedTime;
	private float timeLimit;
	private bool count;

	private SpriteRenderer sr;

	private bool exited;
	private bool asking;

	public bool Asking {
		get {return asking;}
	}

	public Bubble Bubble {
		get {return bubble;}
		set {bubble = value;}
	}

	public enum Type {
		BOY, GIRL,
		LADY, MAN
	}

	public static List<Type> available = new List<Type>();

	public static Type[] all = {
		Type.BOY, Type.GIRL,
		Type.LADY, Type.MAN
	};

	public static Type[] easy  = {
		Type.BOY, Type.GIRL
	};

	public static Type[] hard = {
		Type.LADY, Type.MAN
	};

	public void SetSlot(Slot slot) {
		this.slot = slot;
		slot.Take(this);
	}

	public void LeaveSlot() {
		if (slot == null) return;
		slot.Clear();
		asking = false;
		Destroy(bubble.gameObject);
		sr.sortingOrder = -1;
		StartCoroutine(MoveTo(GameManager.Instance.Exit));
	}

	void Awake() {
		timeLimit = 0;
		elapsedTime = 0;
		count = false;
		sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		exited = false;
		asking = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (count) {
			elapsedTime += Time.deltaTime;

			float ratio = elapsedTime / timeLimit;

			sr.color = new Color(sr.color.r, 1 - ratio, 1 -  ratio);
			
			if (elapsedTime >= timeLimit) {
				LeaveSlot(); //leave after time limit expired
				this.count = false;
			}
		}
	}

	public void StartTimer(float timeLimit) {
		this.timeLimit = timeLimit;
		this.count = true;
	}

	public void ReceiveGift(Gift gift) {
		//TODO receive gift
		//highlight to green to see satisfaction
		this.count = false;
		sr.color = new Color(0,1,0);
		CheckGift(gift);
		Destroy(gift.gameObject);
		LeaveSlot();
	}

	public void Select() {
		transform.localEulerAngles = new Vector3(45,0,0);
	}

	public void Deselect() {
		transform.localEulerAngles = new Vector3(0,0,0);
	}

	IEnumerator MoveTo(GameObject exit) {
		while (!exited) {
			transform.position = Vector2.MoveTowards(transform.position, exit.transform.position, 0.1f);

			yield return null;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Finish") {
			exited = true;
			Destroy(gameObject);
		}
	}

	void CheckGift(Gift gift) {
		int countScore = 0;
		if (CheckOrder(gift)) countScore += 50;
		if (CheckBase(gift)) countScore += 50;

		GameManager.Instance.AddScore(countScore);
	}


	bool CheckOrder(Gift gift) {
		List<GameObject> bubbleCells = bubble.BubbleCells;
		Gift currentGift = gift;
		for (int i = bubbleCells.Count - 1; i >= 0; i--) {
			if (currentGift == null) return false;
			if (!CheckGiftIfSimilar(currentGift.GiftType, bubbleCells[i].GetComponent<BubbleCell>().Type)) return false;
			currentGift = currentGift.Child;
		}

		return true;
	}

	bool CheckBase(Gift gift) {
		List<GameObject> bubbleCells = bubble.BubbleCells;
		Bubble.Type bubbleType = bubbleCells[0].GetComponent<BubbleCell>().Type;
		Gift.Type giftType = gift.GetBaseType();

		return CheckGiftIfSimilar(giftType, bubbleType);
	}

	bool CheckGiftIfSimilar(Gift.Type giftType, Bubble.Type bubbleType) {

		switch (giftType) {
			case Gift.Type.DOLL:
			if (bubbleType == Bubble.Type.TOY_DOLL) return true;
			break;
			case Gift.Type.GUN:
			if (bubbleType == Bubble.Type.TOY_GUN) return true;
			break;

			case Gift.Type.DOLL_PLAIN_WRAPPER:
			case Gift.Type.GUN_PLAIN_WRAPPER:
			if (bubbleType == Bubble.Type.WRAPPER_PLAIN) return true;
			break;

			case Gift.Type.DOLL_POLKA_WRAPPER:
			case Gift.Type.GUN_POLKA_WRAPPER:
			if (bubbleType == Bubble.Type.WRAPPER_POLKA) return true;
			break;

			case Gift.Type.DOLL_RIBBON_WRAPPER:
			case Gift.Type.GUN_RIBBON_WRAPPER:
			if (bubbleType == Bubble.Type.WRAPPER_RIBBON) return true;
			break;

			case Gift.Type.DOLL_STRIPES_WRAPPER:
			case Gift.Type.GUN_STRIPES_WRAPPER:
			if (bubbleType == Bubble.Type.WRAPPER_STRIPES) return true;
			break;
		}

		return false;
	}

}
