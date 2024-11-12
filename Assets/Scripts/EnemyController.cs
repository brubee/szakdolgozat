using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public GameObject playerStatus, gameManager;
    private Animator animator;

    //For the movement
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet = false;

    public float sightRange, glowRange;
    public bool playerInSightRange, playerInGlowRange;
    public bool ableToCatch;

    //Glowing mode necessities
    public GameObject glowEffect;
    private bool hasSpawned = false;
    private GameObject spawnedPrefab;

    //Checking to see if stuck necessities
    private float lastCheckTime = 0;
    private Vector3 lastCheckPosition;
    private float checkTime = 3.0f;

    //Sounds
    public AudioClip stepsAudioClip;
    public AudioClip[] mimicAudioClips;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerStatus = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        lastCheckPosition = transform.position;

        //If stuck or unreachable Walkpoint
        InvokeRepeating("SearchWalkPoint", 10, 30);
    }

    private void Update()
    {
        //Check for sight and glow range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInGlowRange = Physics.CheckSphere(transform.position, glowRange, whatIsPlayer);

        if (!playerInSightRange && !playerInGlowRange) Roaming();
        if (playerInSightRange && !playerInGlowRange) Chasing();
        if (playerInSightRange && playerInGlowRange) Glowing();

        CheckIfStuck();
    }

    private void CheckIfStuck()
    {
        if ((Time.time - lastCheckTime) > checkTime)
        {
            if ((transform.position - lastCheckPosition).magnitude < 1f)
            {
                animator.SetBool("probablyStuck", true);
            }
            else
            {
                animator.SetBool("probablyStuck", false);
                SoundManager.instance.playSoundClip(stepsAudioClip, transform, 0.2f);
            }
            lastCheckPosition = transform.position;
            lastCheckTime = Time.time;
        }
    }

    private void Roaming()
    {
        animator.SetBool("playerIsNearby", false);
        ableToCatch = false;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
            walkPointSet = false;

        if (hasSpawned)
        {
            Destroy(spawnedPrefab);
            hasSpawned = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-25f, 26f);
        float randomX = Random.Range(-32f, -3f);

        walkPoint = new Vector3((transform.position.x + randomX)/2, transform.position.y, (transform.position.z - randomZ)/1.5f);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void Chasing()
    {
        animator.SetBool("playerIsNearby", true);
        SoundManager.instance.playRandomSoundClip(mimicAudioClips, transform, 0.8f);

        agent.SetDestination(player.position);

        if (hasSpawned)
        {
            Destroy(spawnedPrefab);
            hasSpawned = false;
        }
    }

    private void Glowing()
    {
        SoundManager.instance.playRandomSoundClip(mimicAudioClips, transform, 0.8f);
        ableToCatch = true;
        if (ableToCatch && Vector3.Distance(transform.position, player.transform.position) < 2.5)
        {
            if (playerStatus.GetComponent<PlayerController>().isBlind != true)
            {
                gameManager?.GetComponent<GameManager>().GameOver();
            }
            else
            {
                Roaming();
            }
        }

        if (!hasSpawned)
        {
            spawnedPrefab = Instantiate(glowEffect, transform.position, Quaternion.identity);
            hasSpawned = true;
        }
    }
}
