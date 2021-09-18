using System;
using System.Collections.Generic;
using Project.Scripts.Core.Enums;
using Project.Scripts.Core.ExtensionMethods;
using Project.Scripts.Core.ValueObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts
{
    public class BuildingHandler : MonoBehaviour
    {
        public List<BuildingPrefabReference> buildingPrefabs;
        private City _city;
        private UiController _uiController;
        private Board _board;
        private Building _selectedBuilding;
        private Camera _camera;

        private void Awake()
        {
            _city = FindObjectOfType<City>();
            _uiController = FindObjectOfType<UiController>();
            _board = FindObjectOfType<Board>();
            _camera = Camera.main;
        }

        private void Update()
        {
            var addInteraction = ShouldInteractToAdd();
            var sellInteraction = ShouldInteractToSell();
            
            if (addInteraction && !sellInteraction)
                InteractWithBoard(true);
            else if (!addInteraction && sellInteraction)
                InteractWithBoard(false);
        }

        public void EnableBuilder(string id)
        {
            var buildingId = (BuildingId)Enum.Parse(typeof(BuildingId), id);
            _selectedBuilding = buildingPrefabs.SinglePrefabForType(buildingId);
            Debug.Log($"Selected building: {buildingId}");
        }

        private bool ShouldInteractToAdd()
        {
            return _selectedBuilding != null &&
                   (
                       (Input.GetButton("Fire1") && Input.GetButton("Jump")) ||
                       Input.GetButtonDown("Fire1")
                   );
        }

        private bool ShouldInteractToSell()
        {
            return Input.GetButtonDown("Fire2");
        }
        
        private void InteractWithBoard(bool add)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hitSomething = Physics.Raycast(ray, out var hit);
            if (!hitSomething || HasClickedOnUi()) return;

            var gridPosition = _board.CalculateGridPosition(hit.point);
            
            if (add)
            {
                if (_board.IsGridPositionOccupied(gridPosition)) return;
                
                var hadFunds = _city.cityInfo.SpendCash(_selectedBuilding.cost);
                if (!hadFunds) return;

                _city.UpdateBuildingCount(_selectedBuilding.id);
                _uiController.UpdateCityInfo(_city.cityInfo);
                _board.AddBuilding(_selectedBuilding, gridPosition);
                return;
            }

            var (shouldRefund, refundValue) = _board.RemoveBuilding(gridPosition);
            if (!shouldRefund) return;
            _city.cityInfo.cash += refundValue;
            _uiController.UpdateCityInfo(_city.cityInfo);

        }

        private static bool HasClickedOnUi()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}