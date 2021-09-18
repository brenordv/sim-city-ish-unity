using System;
using UnityEngine;

namespace Project.Scripts.Core.ExtensionMethods
{
    public static class Vector3Extensions
    {
        public static (int, int) ToGridPosition(this Vector3 v3)
        {
            var x = Convert.ToInt32(v3.x);
            var z = Convert.ToInt32(v3.z);
            return (x, z);
        }
    }
}