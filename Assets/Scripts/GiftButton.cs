using UnityEngine;
using System.Collections;
using System;

public class GiftButton : MonoBehaviour {

	[SerializeField]
	private float gunTimer;
	[SerializeField]
	private float dollTimer;

	[SerializeField]
	private float plainTimer;
	[SerializeField]
	private float polkaTimer;
	[SerializeField]
	private float stripesTimer;
	[SerializeField]
	private float ribbonTimer;

	public void ChooseGift(string type) {
		Gift gift = GameManager.Instance.Gift;

		if (gift != null) Debug.Log(gift.GiftType);

		Action method = null;
		float timer = 0f;

		switch (type) {
			case "gun":
				method = new Action(() => GameManager.Instance.PickedGift(Gift.Type.GUN));
				timer = gunTimer;
			break;
			case "doll":
				method = new Action(() => GameManager.Instance.PickedGift(Gift.Type.DOLL));
				timer = dollTimer;
			break;

			case "plain":
				if (gift.IsGiftType(Gift.Type.GUN)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.GUN_PLAIN_WRAPPER));
				}
				else if (gift.IsGiftType(Gift.Type.DOLL)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.DOLL_PLAIN_WRAPPER));
				}

				timer = plainTimer;
			break;
			case "polka":
				if (gift.IsGiftType(Gift.Type.GUN)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.GUN_POLKA_WRAPPER));
				}
				else if (gift.IsGiftType(Gift.Type.DOLL)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.DOLL_POLKA_WRAPPER));
				}

				timer = polkaTimer;
			break;
			case "stripes":
				if (gift.IsGiftType(Gift.Type.GUN)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.GUN_STRIPES_WRAPPER));
				}
				else if (gift.IsGiftType(Gift.Type.DOLL)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.DOLL_STRIPES_WRAPPER));
				}

				timer = stripesTimer;
			break;
			case "ribbon":
				if (gift.IsGiftType(Gift.Type.GUN)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.GUN_RIBBON_WRAPPER));
				}
				else if (gift.IsGiftType(Gift.Type.DOLL)) {
					method = new Action(() => GameManager.Instance.PickedWrapper(Gift.Type.DOLL_RIBBON_WRAPPER));
				}

				timer = ribbonTimer;
			break;

			case "trash":
				GameManager.Instance.ThrowGiftToTrash();
				Loader.Instance.CancelTimer();
				
			break;
		}

		if (method != null) {
			Loader.Instance.SetTimer(timer, method);
		}
	}

}
