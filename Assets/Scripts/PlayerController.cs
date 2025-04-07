using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    private Rigidbody2D rigidbody2d;
    Vector2 move;
    
    
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    // Variables related to animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1,0);
    public InputAction talkAction;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        talkAction.Enable();

     
    }

    // Update is called once per frame
    void Update()
    {   
         move = MoveAction.ReadValue<Vector2>();
         

         if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

          if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(talkAction.triggered)
        {
            Debug.Log("X was pressed");
            FindFriend();
        }

        
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * 3.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 5f, LayerMask.GetMask("NPC"));
        Debug.DrawRay(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, Color.red, 5f);
        if(hit.collider != null)
        {
            Debug.Log("Raycast touched " + hit.collider.gameObject.name);
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

            if(character != null)
            {
                UIHandler.instance.DisplayDialogue(character.dialogue);
            }

        }
    }
}

