using System;
using System.Text;
using Project.Scripts.Core.ValueObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class UiController : MonoBehaviour
    {
        public Text dayText;
        public Text cityText;
        private int _profit;
        
        private void UpdateDayCount(int day)
        {
            dayText.text = $"Day {day}";
        }

        private void UpdateCityInfo(CityInfo cityInfo, int profit)
        {
            _profit = profit;
            UpdateCityInfo(cityInfo);
        }

        public void UpdateCityInfo(CityInfo cityInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Cash: ${Convert.ToInt32(cityInfo.cash)} (+${_profit})");
            sb.AppendLine($"Food: {Convert.ToInt32(cityInfo.food)}");
            sb.AppendLine($"Population: {Convert.ToInt32(cityInfo.population.current)}/{Convert.ToInt32(cityInfo.population.ceiling)}");
            sb.AppendLine($"Jobs: {Convert.ToInt32(cityInfo.jobs.current)}/{Convert.ToInt32(cityInfo.jobs.ceiling)}");

            cityText.text = sb.ToString();
        }
        
        public void UpdateUi(CityInfo cityInfo, int profit)
        {
            UpdateDayCount(cityInfo.day);
            UpdateCityInfo(cityInfo, profit);
        }
    }
}
