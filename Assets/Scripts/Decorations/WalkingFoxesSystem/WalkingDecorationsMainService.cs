using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WalkingDecorationsMainService : MonoBehaviour
{
    public static WalkingDecorationsMainService instance;
    
    private List<WalkingDecorationsPoint> walkingPoints = new List<WalkingDecorationsPoint>();

    private int maxWalkedDecorations;
    [SerializeField] private int walkedDecorationsPerBuilding = 3;
    [SerializeField] private float walkedDecorationsSpawnCooldownMultiplier;
    [SerializeField] private float walkedDecorationSpeed = 3;
    
    [Space]
    
    [SerializeField] private GameObject walkingDecorationPrefab;
    [SerializeField] private List<NavMeshAgent> walkingDecorationsAgents = new List<NavMeshAgent>();
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxWalkedDecorations = walkingPoints.Count * walkedDecorationsPerBuilding;
        
        UpdateMaxWalkedDecorations();

        StartCoroutine(WalkingDecorationsSpawnUpdater());
        StartCoroutine(StoppedWalkingDecorationsDestroyUpdater());
    }

    public void AddWalkingPoint(WalkingDecorationsPoint walkingDecorationsPoint)
    {
        walkingPoints.Add(walkingDecorationsPoint);
        
        UpdateMaxWalkedDecorations();
    }

    private void UpdateMaxWalkedDecorations()
    {
        maxWalkedDecorations = (walkingPoints.Count - 1) * walkedDecorationsPerBuilding;
    }

    private IEnumerator WalkingDecorationsSpawnUpdater()
    {
        var spawnCooldown = 1f;
        var idleWait = new WaitForSeconds(3);
        
        while (true)
        {
            var isWalkingPointTooFew = walkingPoints.Count < 2;
            if (isWalkingPointTooFew)
            {
                yield return idleWait;
                
                continue;
            }
            
            var isWalkedDecorationsTooMany = walkingDecorationsAgents.Count >= maxWalkedDecorations;
            if (isWalkedDecorationsTooMany)
            {
                yield return idleWait;
                
                continue;
            }

            spawnCooldown = Random.Range(0.5f, 2f) * walkedDecorationsSpawnCooldownMultiplier;

            yield return new WaitForSeconds(spawnCooldown);

            SpawnWalkedDecoration();
        }

        void SpawnWalkedDecoration()
        {
            if (walkingPoints.Count < 2)
                throw new Exception("ЗАСПАВНИТЬ ХОДЯЧУЮ ДЕКОРАЦИЮ НЕВОЗМОЖНО, ТОЧЕК МЕЖДУ КОТОРЫМИ ДЕКОРАЦИЯ МОЖЕТ ХОДИТЬ МЕНЬШЕ 2!");
            
            var startPointT = GetRandomDecorationPoint().WalkingPoint;
            var endPointT = GetRandomDecorationPoint().WalkingPoint;
            
            while (endPointT == startPointT)
                endPointT = GetRandomDecorationPoint().WalkingPoint;

            var spawnedAgent = Instantiate(walkingDecorationPrefab,startPointT.position,Quaternion.identity)
                .GetComponent<NavMeshAgent>();
            
            spawnedAgent.SetDestination(endPointT.position);
            spawnedAgent.speed = walkedDecorationSpeed;
            
            walkingDecorationsAgents.Add(spawnedAgent);
        }
        
        WalkingDecorationsPoint GetRandomDecorationPoint()
        {
            var randomNum = Random.Range(0,walkingPoints.Count);

            return walkingPoints[randomNum];
        }
        
    }
    
    private IEnumerator StoppedWalkingDecorationsDestroyUpdater()
    {
        var destroyCooldown = new WaitForSeconds(0.5f);
        
        while (true)
        {
            yield return destroyCooldown;
            
            CheckAndDestroyStoppedWalkedDecorations();
        }

        void CheckAndDestroyStoppedWalkedDecorations()
        {
            var toDestroyAgent = walkingDecorationsAgents.Where(agent => !agent.hasPath).ToList();

            for (int i = 0; i < toDestroyAgent.Count; i++)
            {
                var item = toDestroyAgent[i];

                walkingDecorationsAgents.Remove(item);
                
                Destroy(item.gameObject);
            }
        }
    }

}
