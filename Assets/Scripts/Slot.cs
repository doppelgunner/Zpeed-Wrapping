using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {

	private Customer customer;

	public bool IsTaken {
		get {return customer != null;}
	}

	public void Take(Customer customer) {
		this.customer = customer;
	}

	public void Clear() {
		this.customer = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
