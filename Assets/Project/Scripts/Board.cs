using System;
using Project.Scripts.Core.ExtensionMethods;
using UnityEngine;

namespace Project.Scripts
{
    public class Board : MonoBehaviour
    {
        [Range(0, 1)] public float refundPercentOnSell = .4f;
        public AudioClip buyingSound;
        public AudioClip sellingSound;

        private SoundPlayer _soundPlayer;
        private readonly Building[,] _buildings = new Building[100, 100];

        private void Awake()
        {
            _soundPlayer = FindObjectOfType<SoundPlayer>();
        }

        public void AddBuilding(Building buildingPrefab, Vector3 position)
        {
            var (x, z) = position.ToGridPosition();
            var building = Instantiate(buildingPrefab, position, Quaternion.identity);

            _buildings[x, z] = building;
            _soundPlayer.Play(buyingSound);
            Debug.Log($"Built a '{buildingPrefab.id}' at position {position.x}, {position.z}");
        }

        public bool IsGridPositionOccupied(Vector3 position)
        {
            var (x, z) = position.ToGridPosition();

            if (!IsWithinBoard(x, z)) return true;

            return _buildings[x, z] != null;
        }

        public (bool, int) RemoveBuilding(Vector3 position)
        {
            var (x, z) = position.ToGridPosition();

            if (!IsWithinBoard(x, z) ||
                !IsGridPositionOccupied(position)) return (false, 0);

            var buildingId = _buildings[x, z].id;
            var originalCost = _buildings[x, z].cost;
            var toRefund = Convert.ToInt32(Mathf.Round(originalCost * refundPercentOnSell));
            Destroy(_buildings[x, z].gameObject);
            _buildings[x, z] = null;

            Debug.Log($"Sold a '{buildingId}' at position {position.x}, {position.z}");
            _soundPlayer.Play(sellingSound);
            return (true, toRefund);
        }

        public Vector3 CalculateGridPosition(Vector3 clickedPosition)
        {
            return new Vector3(
                Mathf.Round(clickedPosition.x),
                0.5f,
                Mathf.Round(clickedPosition.z));
        }

        private bool IsWithinBoard(int x, int z)
        {
            return IsWithinBoundaries(x) && IsWithinBoundaries(z);
        }

        private bool IsWithinBoundaries(int value)
        {
            return value >= 0 && value <= 100;
        }
    }
}