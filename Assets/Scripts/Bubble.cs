using UnityEngine;
using System.Collections.Generic;

public class Bubble : MonoBehaviour {

	private List<GameObject> bubbleCells;
	private GameObject arrow;

	public List<GameObject> BubbleCells {
		get {return bubbleCells; }
	}

	public GameObject Arrow {
		get { return arrow; }
		set {arrow = value;}
	}

	public static Type[] gifts = {
		Type.TOY_GUN, Type.TOY_DOLL
	};

	public static Type[] wrappers = {
		Type.WRAPPER_PLAIN,
		Type.WRAPPER_POLKA,
		Type.WRAPPER_STRIPES,
	};

	public static Type[] toppings = {
		Type.WRAPPER_RIBBON
	};

	void Awake() {
		bubbleCells = new List<GameObject>();
		arrow = null;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public enum Type {
		TOY_GUN, TOY_DOLL, //required

		WRAPPER_PLAIN,
		WRAPPER_POLKA,
		WRAPPER_STRIPES,
		WRAPPER_RIBBON,
	}

}
