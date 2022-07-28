using System.Collections.Generic;
using AronBoguta;
using UnityEngine;

public class FireOutwardsCommand : ICommand
{
    private List<SmallCubeController> _pooledCubes;
    private ParticleSystem _particleSystem;
    private Vector3 _center;
    private float _forceFactor;

    public FireOutwardsCommand(List<SmallCubeController> cubes, Vector3 center, float forceFactor, ParticleSystem particleSystem)
    {
        _particleSystem = particleSystem;
        _pooledCubes = cubes;
        _center = center;
        _forceFactor = forceFactor;
    }

    public void Execute()
    {
        _particleSystem.Play();
        
        foreach (var cube in _pooledCubes)
        {
            if (cube.gameObject.activeSelf)
            {
                cube.ShootOutwards(_center, _forceFactor);
            }
        }
    }
}
