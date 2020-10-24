using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour
{
    public TextMeshProUGUI woodAmount;
    public ItemData woodLogs;
    public TextMeshProUGUI stoneAmount;
    public ItemData stoneBlocks;
    public TextMeshProUGUI populationAmount;

    private void OnEnable()
    {
        Stockpile.StockpileChanged += UpdateInfo;
        UnitManager.PopulationChanged += UpdatePopulation;
    }
    private void OnDisable()
    {
        Stockpile.StockpileChanged -= UpdateInfo;
        UnitManager.PopulationChanged -= UpdatePopulation;
    }

    public void UpdateInfo()
    {
        woodAmount.text = CityManager.Instance.GetStockpileItem(woodLogs).ToString();
        stoneAmount.text = CityManager.Instance.GetStockpileItem(stoneBlocks).ToString();
    }

    public void UpdatePopulation(int _amount)
    {
        populationAmount.text = _amount.ToString();
    }

}
