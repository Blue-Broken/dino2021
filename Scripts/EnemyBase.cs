using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyBase : MonoBehaviour
{
    protected Rigidbody enemyBody;
    protected AudioSource enemyAudio;
    protected Animator enemyAnim;

    [Header("Base Variables")]
    public int life;
    public int damange;
    public float activeDistance;
    public Collider headCollider;
    public bool activeAlways = true;
    protected bool actived;
    protected float playerDistance;
    protected Transform playerTransform;
    public bool Died;

    protected virtual void Start()
    {
        enemyBody = GetComponent<Rigidbody>();
        enemyAnim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected virtual void FixedUpdate()
    {
        playerDistance = Vector3.Distance(playerTransform.position, transform.position);

        if(playerDistance < activeDistance)
        {
            actived = true;
        }
        else if(!activeAlways)
        {
            actived = false;
        }
    }
    public virtual void DamangeCicle(int damange, Collider hitArea)
    {
        if (hitArea != headCollider)
        {
            life -= damange;
        }
        else
        {
            life = 0;
        }

        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
