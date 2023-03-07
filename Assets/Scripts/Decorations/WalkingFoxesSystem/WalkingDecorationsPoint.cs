using System;
using UnityEngine;

public class WalkingDecorationsPoint : MonoBehaviour
{
    private WalkingDecorationsMainService walkingDecorationsMainService;
    [SerializeField] private BuildingItem connectedBuildingItem;
    [SerializeField] private Transform walkingPoint;
    private bool isWalkingPointActive = false;

    public Transform WalkingPoint => walkingPoint;
    
    private void Start()
    {
        walkingDecorationsMainService = WalkingDecorationsMainService.instance;

        connectedBuildingItem.OnBuildingUpgrade += CheckBuildingUpgrade;
    }

    private void CheckBuildingUpgrade(int level)
    {
        if(isWalkingPointActive)
            return;

        if (level < 1)
            return;
        
        walkingDecorationsMainService.AddWalkingPoint(this);
        isWalkingPointActive = true;
    }
}
