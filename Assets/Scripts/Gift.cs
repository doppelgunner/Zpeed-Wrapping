using UnityEngine;
using System.Collections;

public class Gift : MonoBehaviour {

	[SerializeField]
	private float moveDistanceDelta = 0.5f;

	private GameObject target;

	private Gift child;
	private Gift.Type giftType;
	private SpriteRenderer sr;

	private Customer customer;

	public Customer Customer {
		get {
			Gift currentGift = this;
			while (currentGift.child != null) {
				currentGift = currentGift.child;
			}

			return currentGift.customer;
		}
	}

	public Gift.Type GiftType {
		get {return giftType;}
		set {giftType = value;}
	}

	public Gift Child {
		get {return child;}
	}

	public enum Type {
		GUN, DOLL,
		
		GUN_PLAIN_WRAPPER, GUN_POLKA_WRAPPER,
		GUN_STRIPES_WRAPPER, GUN_RIBBON_WRAPPER,

		DOLL_PLAIN_WRAPPER, DOLL_POLKA_WRAPPER,
		DOLL_STRIPES_WRAPPER, DOLL_RIBBON_WRAPPER,
	}

	public static Type[] gifts = {
		Type.GUN, Type.DOLL
	};

	public static Type[] gunWrappers = {
		Type.GUN_PLAIN_WRAPPER, Type.GUN_POLKA_WRAPPER,
		Type.GUN_STRIPES_WRAPPER, Type.GUN_RIBBON_WRAPPER
	};

	public static Type[] dollWrappers = {
		Type.DOLL_PLAIN_WRAPPER, Type.DOLL_POLKA_WRAPPER,
		Type.DOLL_STRIPES_WRAPPER, Type.DOLL_RIBBON_WRAPPER
	};

	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		customer = null;
	}
	
	// Update is called once per frame
	void Update () {
		
		MoveToTarget();
	}

	//To delete?
	public void SetTarget(GameObject go) {
		target = go;

	}

	void MoveToTarget() {
		if (target == null) return;
		transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveDistanceDelta);
	}
	
	public void SetChild(Gift child) {
		this.child = child;

		SpriteRenderer childSR = child.GetComponent<SpriteRenderer>();
		if (giftType != Type.GUN_RIBBON_WRAPPER && giftType != Type.DOLL_RIBBON_WRAPPER) {
			DisableChildSpriteRenderer(this);
		}

		child.transform.parent = this.transform;
				
		if (sr != null) {
			sr.sortingOrder = childSR.sortingOrder+ 1;
		}
	}

	public bool IsGiftType(Gift.Type cType) {
		if (this.giftType == cType) return true;

		Gift giftChild = child;
		while (giftChild != null) {
			if (giftChild.GiftType == cType) return true;
			giftChild = giftChild.child;
		}
		return false;
	}

	public Gift.Type GetBaseType() {

		Gift currentGift = this;
		while (currentGift.child != null) {
			currentGift = currentGift.child;
		}
		return currentGift.GiftType;
	}

	void DisableAllSpriteRenderer() {
		Gift giftToDisable = this;
		while (giftToDisable != null) {
			giftToDisable.GetComponent<SpriteRenderer>().enabled = false;
			giftToDisable = giftToDisable.child;
		}
	}

	void DisableChildSpriteRenderer(Gift parent) {
		Gift child = parent.child;
		if (child == null) return;

		SpriteRenderer childSR = child.GetComponent<SpriteRenderer>();
		if (childSR.enabled) {
			childSR.enabled = false;
			DisableChildSpriteRenderer(child);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject == target) {
			target = null;
		}
	}

	void OnTriggerStay2D(Collider2D other) {

		if (other.tag == "People") {
			GameObject otherGameObject = other.gameObject;
			Customer otherCustomerScript = other.GetComponent<Customer>();
			if (!otherCustomerScript.Asking) {
				return; // do nothing if not asking
			} else if (customer == null || (customer != null && customer.gameObject != otherGameObject && IsCloser(otherGameObject))) {
				customer = otherCustomerScript;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (customer != null && other.tag == "People") {
			if (customer.gameObject == other.gameObject) {
				customer = null;
			}
		}
	}

	bool IsCloser(GameObject g2) {
		GameObject g1 = customer.gameObject;
		float g1Distance = Vector2.Distance(gameObject.transform.position, g1.transform.position);
		float g2Distance = Vector2.Distance(gameObject.transform.position, g2.transform.position);
		if (g2Distance < g1Distance) return true;

		return false;
	}
	

}
