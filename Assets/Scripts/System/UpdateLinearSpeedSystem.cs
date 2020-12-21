using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[UpdateAfter(typeof(CalculateGravitationSystem))]
public class UpdateLinearSpeedSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity velocity, in ResultingForce force, in Mass mass) =>
        {
            velocity.Linear += force.Value / (mass.Value);
        }).ScheduleParallel();
    }
}