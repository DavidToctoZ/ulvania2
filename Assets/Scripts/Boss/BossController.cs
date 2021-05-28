using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;
    private Animator animator;

    public GameObject minion;

    public float rate = 1f;
    SpriteRenderer bossF;

    int health = 7;
    [SerializeField]
    private int maxHealth = 7;

    public Slider healthSlider;

    void Start()
    {
        animator = GetComponent<Animator>();
        positions.Add(new Vector3(46.25f, 9.4f)); // No
        positions.Add(new Vector3(51.4f, 12.2f)); //No
        positions.Add(new Vector3(31.22f, 12.2f));  //Si 2 
        positions.Add(new Vector3(33.13f, 2.33f)); //Si  3
        positions.Add(new Vector3(40.75f, 7.7f)); //Center attack 4

        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0f;
        healthSlider.value = maxHealth;

        bossF = GetComponent<SpriteRenderer>();
    }

    public void Hurt(int damage)
    {
        health -= damage;
        healthSlider.value -= damage;
        if (health <= 0)
        {
            Die();
            
        }
    }

    void ataqueHoz()
    {
        isAtaqueHoz = true;
        animator.SetTrigger("attack");
    }

    void ataqueMinion()
    {
        animator.SetTrigger("summon");
    }

    private void Die()
    {
        StartCoroutine("destroyBoss");
    }

    IEnumerator destroyBoss()
    {
        animator.SetTrigger("destroy");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    List<Vector3> positions = new List<Vector3>();


    int r;
    int cont = 0;
    int contM = 0;
    // Update is called once per frame
    int typeAttack;
    
    void Update()
    {
        d = getR(2);
        if (Math.Floor(Time.time) % 13 == 0 && Math.Floor(Time.time) > 0){
            
           
            contM++;
            if (contM == 1)
            {
                ataqueMinion();
                //Transform clone;
                Vector3 t = transform.position;
                t.x = t.x + 5;
                Instantiate(minion, t, Quaternion.identity);
                
            }
        }
        else if (Math.Floor(Time.time) % 5 == 0)
        {
            
            ataqueHoz();
            StartCoroutine("attackHoz");
            cont++;
            if(cont == 1)
            {
                
                int tmp = getR(5);
                if(tmp == 2 || tmp == 3)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                transform.position = positions[tmp];
                
            }
        }
        else
        {
            cont = 0;
            contM = 0;
        }

    }
    int d;
    public int getR(int x)
    {
        return Random.Range(0, x);
    }
    IEnumerator attackHoz()
    {
        
        yield return new WaitForSeconds(2.5f);
        isAtaqueHoz = false;
    }

    public void moveTo()
    {
        transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        ataqueHoz();
    }
    public bool getAtaqueHozBool()
    {
        return isAtaqueHoz;
    }

    public bool isAtaqueHoz = false;
    
   
}
