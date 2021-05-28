using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ULVania.Enemy;

public class HeroMelee : MonoBehaviour
{
    private Animator animator;
    private GameObject contactPoint;
    public GameManager gm;

    [SerializeField]
    private float attackRange = 2f;

    [SerializeField]
    private int damage = 2;

    public float rate = 1f;
    private float counter = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        contactPoint = transform.Find("ContactPoint").gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > counter)
        {
            // Tenemos iniciar ataque
            Attack();
            counter = Time.time + rate;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            contactPoint.transform.position, attackRange);
        foreach(Collider2D collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                gm.addPower(1);
                collider.GetComponent<EnemyController>().Hurt(damage);
            }else if(collider.tag == "boss")
            {
                gm.addPower(1);
                collider.GetComponent<BossController>().Hurt(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (contactPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(contactPoint.transform.position, attackRange);
    }

    
}
