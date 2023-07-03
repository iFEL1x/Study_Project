using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnListComponent : MonoBehaviour

    {
    [SerializeField] private SpawnData[] _spawnres;

    public void Spawn(string id)
    {
        foreach (var data in _spawnres)
        {
            var spawner = _spawnres.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();
        }
    }

    [Serializable]
    public class SpawnData
    {
        public string Id;
        public SpawnComponent Component;
    }
    }
}