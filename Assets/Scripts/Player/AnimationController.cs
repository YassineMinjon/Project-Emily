using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnimationController : MonoBehaviour
{
    public GameObject body;
    public GameObject footstepPrefab;
    private Animator anim;

    [Tooltip("0 = open links, 1 = open rechts, 2 = hand op zij, 3 = vuist voor lantern")]
    public Sprite[] Hands;
    private Sprite[] currentHands = new Sprite[2]; // 0 = left, 1 = right
    [Tooltip("0 = plat, 1 = licht omhoog, 2 = stijl omhoog, 3 = recht omhoog")]
    public Sprite[] Feet;
    private Sprite[] currentFeet = new Sprite[2]; // 0 = left, 1 = right
    private float direction;

    [HideInInspector]
    public Vector2 playerInput;
    public bool isJumping;

    public SpriteRenderer hand_left;
    public SpriteRenderer hand_right;
    public SpriteRenderer foot_left;
    public SpriteRenderer foot_right;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Application.isEditor)
        {
            anim = GetComponentInChildren<Animator>();
        }
        anim.SetBool("IsWalking", (playerInput.x != 0) ? true : false);
        if(direction != playerInput.x && playerInput.x != 0)
        {
            direction = playerInput.x;
            StartCoroutine(TurnAround());
        }
        anim.SetBool("IsJumping", isJumping);
    }

    IEnumerator TurnAround()
    {
        body.transform.localScale = new Vector3(-direction * 0.8f, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(0.1f);
        body.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }

    #region AnimationEvent methods
    public void SetLeftHandSprite(int spriteIndex)
    {
        if (currentHands[0] == null || currentHands[0] != Hands[spriteIndex])
        {
            hand_left.sprite = Hands[spriteIndex];
            currentHands[0] = Hands[spriteIndex];
        }
    }

    public void SetRightHandSprite(int spriteIndex)
    {
        if (currentHands[1] == null || currentHands[1] != Hands[spriteIndex])
        {
            hand_right.sprite = Hands[spriteIndex];
            currentHands[1] = Hands[spriteIndex];
        }
    }

    public void SetLeftFootSprite(int spriteIndex)
    {
        if (currentFeet[0] == null || currentFeet[0] != Feet[spriteIndex])
        {
            foot_left.sprite = Feet[spriteIndex];
            currentFeet[0] = Feet[spriteIndex];
        }
    }

    public void SetRightFootSprite(int spriteIndex)
    {
        if (currentFeet[1] == null || currentFeet[1] != Feet[spriteIndex])
        {
            foot_right.sprite = Feet[spriteIndex];
            currentFeet[1] = Feet[spriteIndex];
        }
    }

    public void PlayFootstepSound()
    {
        Instantiate(footstepPrefab, Vector3.Lerp(foot_right.transform.position, foot_left.transform.position, 0.5f), Quaternion.identity);
    }
    #endregion
}
