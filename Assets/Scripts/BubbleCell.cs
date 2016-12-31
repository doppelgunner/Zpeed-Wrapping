using UnityEngine;
using System.Collections;

public class BubbleCell : MonoBehaviour {

	private Bubble.Type type;

	public Bubble.Type Type {
		get { return type;}
		set {type = value;}
	}
}
