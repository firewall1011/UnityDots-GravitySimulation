using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public class SpawnPrefabsSystem : SystemBase
{
    public int NumberOfPlanets;
    public Vector3 BorderCenter;
    public float BorderLength;
    public float2 SizeRange;

    EntityManager entityManager;
    float spawnTime;

    protected override void OnCreate()
    {
        BorderCenter = math.float3(0);
        BorderLength = 1000f;
        SizeRange = new float2(1, 1000);
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        spawnTime -= Time.DeltaTime;

        if(spawnTime <= 0)
        {
            float randomMass = UnityEngine.Random.Range(SizeRange.x, SizeRange.y);
            float3 randomPos = (UnityEngine.Random.insideUnitSphere * BorderLength) + BorderCenter;
            Entities.WithStructuralChanges().ForEach((in PrefabData prefab) =>
            {
                Entity planet = entityManager.Instantiate(prefab.prefabEntity);
                entityManager.SetComponentData(planet, new Mass
                {
                    Value = randomMass
                });
                entityManager.SetComponentData(planet, new Translation
                {
                    Value = randomPos
                });
            }).Run();
            spawnTime = UnityEngine.Random.Range(.1f, .3f);
        }
    }
}
