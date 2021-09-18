using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Core.Enums;
using Project.Scripts.Core.ValueObjects;

namespace Project.Scripts.Core.ExtensionMethods
{
    public static class ListExtensions
    {
        public static Building SinglePrefabForType(this IList<BuildingPrefabReference> list, BuildingId buildingId)
        {
            return list.Single(s => s.id == buildingId).prefab;
        }
    }
}