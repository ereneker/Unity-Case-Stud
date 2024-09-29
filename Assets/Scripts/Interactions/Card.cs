using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int cardValue;
    public bool isFlipped = false;

    private Animator animator;
    private GameManager gameManager;
    private Image cardFrontImage;
    private Image cardBackImage;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFlipped)
        {
            FlipBack();
            return;
        }

        Debug.Log("Flipped");
        FlipCard();
        gameManager.CardFlipped(this);
    }

    public void FlipCard()
    {
        isFlipped = true;
        animator.SetBool("isFlipped", true);

        AudioManager.Instance.PlaySound("CardFlip");
    }

    public void FlipBack()
    {
        isFlipped = false;
        animator.SetBool("isFlipped", false);

        AudioManager.Instance.PlaySound("CardFlip");
    }

    public void ShowFront()
    {
        cardBackImage.enabled = false;
        cardFrontImage.enabled = true;
    }

    public void ShowBack()
    {
        cardBackImage.enabled = true;
        cardFrontImage.enabled = false;
    }
}