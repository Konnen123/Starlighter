
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    [SerializeField] private GameObject hips;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] private float walkSpeed;
    public float walkPointRange;
    private bool isChasing;

    //Attacking
    public float timeBetweenAttacks;
    [SerializeField] private float chaseSpeed;
    [SerializeField] float attackDamage;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip,deathClip;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Space] [SerializeField]
    private AnimationClip idleAnimation,walkAnimation,runAnimation,attackAnimation;

    [SerializeField] private bool hasKey;
    [SerializeField] private GameObject key;

    private Animator npcAnimator;

    private float timer=3f,currentTimer;

    private bool isDead;
    private Collider[] colliders;
    private CharacterJoint[] characterJoints;
    private Rigidbody[] rigidbodies;

    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
        npcAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        
        colliders = hips.GetComponentsInChildren<Collider>();
        characterJoints = hips.GetComponentsInChildren<CharacterJoint>();
        rigidbodies = hips.GetComponentsInChildren<Rigidbody>();
        enableAnimator();
        
        npcAnimator.Play(walkAnimation.name);
    }

    private void Update()
    {
        if ( isDead)
            return;
        

      if(alreadyAttacked)
          return;
        
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

       
        
        if (!isChasing)
            if (!playerInSightRange && !playerInAttackRange) Patroling();
        
            if ((playerInSightRange && !playerInAttackRange)) ChasePlayer();
            
            if (playerInAttackRange && playerInSightRange) AttackPlayer();

        
    }

    private void Patroling()
    {
        currentTimer += Time.deltaTime;
        npcAnimator.Play(walkAnimation.name);
        agent.speed = walkSpeed;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f || currentTimer >= timer)
        {
            currentTimer = 0;
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
       
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        npcAnimator.Play(runAnimation.name);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);
        
        if (!alreadyAttacked)
        {
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            
            npcAnimator.Play(attackAnimation.name);
            //Attack code
      
            //End of attack 
        }
        

    }
    private void ResetAttack()
    {
    
        alreadyAttacked = false;
    
    }

    public void DealDamage()
    {
        if (!playerInAttackRange || HealthManager.Instance.GetCurrentHealth()<=0)
            return;
        
        _audioSource.clip = _clip;
        _audioSource.Play();
        HealthManager.Instance.TakeDamage(attackDamage);    
        Debug.Log("hit");
       
    }

    public void TakeDamage(float damage)
    {
        isChasing = true;
        health -= damage;
        ChasePlayer();

        if (health <= 0)
        {
            _audioSource.clip = deathClip;
            _audioSource.Play();
            enableRagdoll();
            Invoke(nameof(DestroyEnemy), 3f);
        }
    }

    void enableAnimator()
    {
        npcAnimator.enabled = true;
        foreach (var joint in characterJoints)
        {
            joint.enableCollision = false;
        }

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        
    }

    void enableRagdoll()
    {
     
        npcAnimator.enabled = false;
        agent.enabled = false;
        isDead = true;
        
        Rigidbody currentRigidbody = GetComponent<Rigidbody>();
        
        currentRigidbody.velocity=Vector3.zero;
        currentRigidbody.detectCollisions = false;
        currentRigidbody.useGravity = false;

        GetComponent<Collider>().enabled = false;
        foreach (var joint in characterJoints)
        {
            joint.enableCollision = true;
        }

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.velocity=Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

    }
    private void DestroyEnemy()
    {
        if (hasKey)
            Instantiate(key, new Vector3(transform.position.x,transform.position.y+1,transform.position.z), Quaternion.identity);
            
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
