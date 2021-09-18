using System.Collections.Generic;
using Project.Scripts.Core.Enums;
using Project.Scripts.Core.ValueObjects;
using UnityEngine;

namespace Project.Scripts
{
    public class City : MonoBehaviour
    {
        public CityInfo cityInfo;
        public int initialCash = 30;
        
        private UiController _uiController;
        private readonly Dictionary<BuildingId, int> _buildings = new Dictionary<BuildingId, int>
        {
            { BuildingId.Road, 0 }, { BuildingId.House, 0 }, { BuildingId.Farm, 0 }, { BuildingId.Factory, 0 }
        };

        private void Awake()
        {
            _uiController = GetComponent<UiController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            cityInfo.cash = initialCash;
            _uiController.UpdateUi(cityInfo, 0);
        }

        public void UpdateBuildingCount(BuildingId buildingId, int delta = 1)
        {
            if (!_buildings.ContainsKey(buildingId))
                _buildings.Add(buildingId, 0);
            
            _buildings[buildingId] += delta;
        }

        public void EndTurn()
        {
            cityInfo.day++;
            var profit = CalculateCash();
            CalculateJobs();
            CalculateFood();
            CalculatePopulation();
            _uiController.UpdateUi(cityInfo, profit);
            Debug.Log("Day Ended!");
        }

        private void CalculateJobs()
        {
            cityInfo.jobs.ceiling = _buildings[BuildingId.Factory] * 3;
            cityInfo.jobs.current = Mathf.Min(cityInfo.population.current, cityInfo.jobs.ceiling);
        }

        private int CalculateCash()
        {
            var profit = (int)Mathf.Min(cityInfo.jobs.current, cityInfo.population.current) * 2;
            cityInfo.cash += profit;
            return profit;
        }

        private void CalculateFood()
        {
            cityInfo.food += _buildings[BuildingId.Farm] * 2f;
        }

        private void CalculatePopulation()
        {
            cityInfo.population.ceiling = _buildings[BuildingId.House] * 3f;
            if (GainedPopulation())
            {
                var remainingFood = Mathf.Max(cityInfo.food - cityInfo.population.current * 2f, 0);
                cityInfo.food = remainingFood;
                cityInfo.population.current = Mathf.Min(cityInfo.population.current + (cityInfo.food * .25f),
                    cityInfo.population.ceiling);
            }
            else if (cityInfo.food < cityInfo.population.current)
            {
                cityInfo.population.current -= Mathf.Max(cityInfo.population.current - cityInfo.food * .5f, 0);
            }
        }

        private bool GainedPopulation()
        {
            return cityInfo.food >= cityInfo.population.current &&
                   cityInfo.population.current < cityInfo.population.ceiling;
        }
    }
}