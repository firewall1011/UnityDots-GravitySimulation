using System;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ResultingForce : IComponentData
{
    public float3 Value;
}
