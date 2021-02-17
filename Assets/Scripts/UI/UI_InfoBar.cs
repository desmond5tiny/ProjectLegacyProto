using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InfoBar : MonoBehaviour
{
    public TextMeshProUGUI woodAmount;
    public ItemData woodLogs;
    public TextMeshProUGUI stoneAmount;
    public ItemData stoneBlocks;
    public TextMeshProUGUI populationAmount;

    private void OnEnable()
    {
        Stockpile.StockpileChanged += UpdateInfo;
        UnitManager.PopulationIncreased += UpdatePopulation;
        UnitManager.PopulationDecreased += UpdatePopulation;
    }
    private void OnDisable()
    {
        Stockpile.StockpileChanged -= UpdateInfo;
        UnitManager.PopulationIncreased -= UpdatePopulation;
        UnitManager.PopulationDecreased -= UpdatePopulation;
    }

    public void UpdateInfo()
    {
        woodAmount.text = CityManager.Instance.GetStockpileItem(woodLogs).ToString();
        stoneAmount.text = CityManager.Instance.GetStockpileItem(stoneBlocks).ToString();
    }

    public void UpdatePopulation(PlayerUnit _unit)
    {
        populationAmount.text = UnitManager.Instance.unitDict.Count.ToString();
    }

}
