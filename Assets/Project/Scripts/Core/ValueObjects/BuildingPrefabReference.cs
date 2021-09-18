using System;
using Project.Scripts.Core.Enums;

namespace Project.Scripts.Core.ValueObjects
{
    [Serializable]
    public class BuildingPrefabReference
    {
        public BuildingId id;
        public Building prefab;
    }
}