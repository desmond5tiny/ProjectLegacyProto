using System.Collections.Generic;

public interface IStructure
{
    float GetMaxHealth();
    void AddToGrid();
    void RemoveFromGrid();
    Dictionary<ItemData, int> GetBuildDict();
}
