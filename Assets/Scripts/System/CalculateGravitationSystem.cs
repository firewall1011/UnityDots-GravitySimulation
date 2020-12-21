using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class CalculateGravitationSystem : SystemBase
{
    EntityQuery planetQuery;

    [BurstCompile]
    private struct ChaserSystemJob : IJobChunk
    {
        // Read-only data stored (potentially) in other chunks
        [ReadOnly]
        public ArchetypeChunkComponentType<Translation> PositionTypeAccessor;

        // Read-only data in the current chunk
        [ReadOnly]
        public ArchetypeChunkComponentType<Mass> MassTypeAccessor;

        // Read-write data in the current chunk
        public ArchetypeChunkComponentType<ResultingForce> ForceTypeAccessor;

        //Non-entity data
        public float G;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var positions = chunk.GetNativeArray<Translation>(PositionTypeAccessor);
            var masses = chunk.GetNativeArray<Mass>(MassTypeAccessor);
            var forces = chunk.GetNativeArray<ResultingForce>(ForceTypeAccessor);

            
            for (int i = 0; i < chunk.Count; i++)
            {
                //float3 result = forces[i].Value;
                float3 result = 0;
                for (int j = 0; j < chunk.Count; j++)
                {
                    if (i == j)
                        continue;
                    float3 direction = math.normalizesafe(positions[j].Value - positions[i].Value);
                    float magnitude = G * masses[i].Value * masses[j].Value / math.distancesq(positions[j].Value, positions[i].Value);
                    result += direction * magnitude;
                }
                forces[i] = new ResultingForce { Value = result};
            }
        }
    }

    protected override void OnCreate()
    {
        planetQuery = GetEntityQuery(typeof(ResultingForce), ComponentType.ReadOnly(typeof(Mass)), ComponentType.ReadOnly(typeof(Translation)));
    }

    protected override void OnUpdate()
    {
        var job = new ChaserSystemJob
        {
            PositionTypeAccessor = GetArchetypeChunkComponentType<Translation>(true),
            MassTypeAccessor = GetArchetypeChunkComponentType<Mass>(true),
            ForceTypeAccessor = GetArchetypeChunkComponentType<ResultingForce>(false),
            G = 1f,
        };

        Dependency = job.Schedule(planetQuery, Dependency);
    }
}