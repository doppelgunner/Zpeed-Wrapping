using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class CardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField]
	private Animator cardCoverAnimator;

	public void OnPointerEnter(PointerEventData eventData) {
		cardCoverAnimator.Play("CardCoverRemove");
	}

	public void OnPointerExit(PointerEventData eventData) {
		cardCoverAnimator.Play("CardCoverPlace");
	}
}
