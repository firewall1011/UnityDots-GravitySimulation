using System;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Mass : IComponentData
{
    public float Value;
}
