using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
   public float changeTime = 3.0f;
   float timer;

    Rigidbody2D rigidbody2d;
    int direction = 1;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }
    // Update is called every frame
   void Update()
   {
     


       timer-= Time.deltaTime;


      if (timer < 0)
      {
        direction = -direction;
        timer = changeTime;
      }

     
   }


    // Update is called once per frame
    void FixedUpdate()
    {
 

         Vector2 position = rigidbody2d.position;

      if (vertical)
       {
           position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Look X", 0);
            animator.SetFloat("Look Y", direction);
       }
       else
       {
           position.x = position.x + speed * direction * Time.deltaTime;
           animator.SetFloat("Look X", direction);
            animator.SetFloat("Look Y", 0);
       }


       rigidbody2d.MovePosition(position);
    }
}
