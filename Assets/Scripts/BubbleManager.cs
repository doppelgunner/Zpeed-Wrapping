using UnityEngine;
using System.Collections.Generic;

public class BubbleManager : Singleton<BubbleManager> {

	[SerializeField]
	private Vector2 bubbleOffset;

	[SerializeField]
	private GameObject bCellDoll;
	[SerializeField]
	private GameObject bCellGun;
	[SerializeField]
	private GameObject bCellWrapperPlain;
	[SerializeField]
	private GameObject bCellWrapperPolka;
	[SerializeField]
	private GameObject bCellWrapperStripes;
	[SerializeField]
	private GameObject bCellWrapperRibbon;
	
	[SerializeField]
	private GameObject bubbleArrow;

	[SerializeField]
	private int maxLimit = 5;
	[SerializeField]
	private int minLimit = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject CreateBubbleAtParent(GameObject at) {
		GameObject newBubble = new GameObject("Bubble"); //parent

		Vector2 newPosition = new Vector2(at.transform.position.x + bubbleOffset.x, at.transform.position.y + bubbleOffset.y);

		GameObject newBubbleArrow = Instantiate(bubbleArrow, newPosition, Quaternion.identity) as GameObject;
		Bubble bubbeScript = newBubble.AddComponent<Bubble>();

		newBubble.transform.position = newPosition;
		newBubble.transform.parent = at.transform;
		newBubbleArrow.transform.parent = newBubble.transform;
		bubbeScript.Arrow = newBubbleArrow;
		RandomizeBubble(newBubble);

		return newBubble;
	}

	void PopBubbles(List<GameObject> bubbles) {
		for (int i = 0; i < bubbles.Count; i++) {
			Destroy(bubbles[i]);
		}
		bubbles.Clear();
	}

	void AddBubbleCell(GameObject parent, GameObject bubbleToCreate, Bubble.Type type) {
		Bubble bubbleScript = parent.GetComponent<Bubble>();
		GameObject arrow = bubbleScript.Arrow;
		List<GameObject> bubbleCells = bubbleScript.BubbleCells;

		Vector2 arrowPosition = arrow.transform.position;
		float newY = (bubbleCells.Count <= 0) ? arrowPosition.y : (bubbleCells[bubbleCells.Count - 1].transform.position.y + 0.75f);
		Vector2 bubbleCellPosition = new Vector2(arrowPosition.x, newY);
		GameObject newBubbleCell = Instantiate(bubbleToCreate, bubbleCellPosition, Quaternion.identity) as GameObject;

		BubbleCell bubbleCellScript = newBubbleCell.AddComponent<BubbleCell>();
		bubbleCellScript.Type = type;

		newBubbleCell.transform.parent = parent.transform;	
		bubbleCells.Add(newBubbleCell);
	}


	void RandomizeBubble(GameObject parent) {
		
		int requiredCell = Random.Range(0,Bubble.gifts.Length);
		Bubble.Type requiredType = Bubble.gifts[requiredCell];
		AddBubbleCell(parent, requiredType);

		//choose a wrapper
		int nWrappers = Random.Range(minLimit,maxLimit);
		for (int i = 0; i < nWrappers; i++) {
			int wrapper = Random.Range(0,Bubble.wrappers.Length);
			Bubble.Type wrapperType = Bubble.wrappers[wrapper];
			AddBubbleCell(parent, wrapperType);
		}

		//choose a topping
		int nToppings = Random.Range(0,2);
		for (int i = 0; i < nToppings; i++) {
			int topping = Random.Range(0,Bubble.toppings.Length);
			Bubble.Type toppingType = Bubble.toppings[topping];
			AddBubbleCell(parent, toppingType);
		}
	}

	void AddBubbleCell(GameObject parent, Bubble.Type type) {	

		GameObject bubble = null;

		switch (type) {
			//gifts
			case Bubble.Type.TOY_GUN:
				bubble = bCellGun;
			break; 
			
			case Bubble.Type.TOY_DOLL:
				bubble = bCellDoll;
			break; 
			
			//wrappers
			case Bubble.Type.WRAPPER_PLAIN:
				bubble = bCellWrapperPlain;
			break; 

			case Bubble.Type.WRAPPER_POLKA:
				bubble = bCellWrapperPolka;
			break;

			case Bubble.Type.WRAPPER_RIBBON:
				bubble = bCellWrapperRibbon;
			break; 

			case Bubble.Type.WRAPPER_STRIPES:
				bubble = bCellWrapperStripes;
			break; 

			default:
			break;
		}

		AddBubbleCell(parent, bubble, type);
	}
}
