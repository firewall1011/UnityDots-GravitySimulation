using System;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Radius : IComponentData
{
    public float Value;
}
