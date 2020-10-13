using System.Collections.Generic;

public interface IResource
{
    void DropResource(int amount);
    bool GetRescource(float damage, out int dropResource);
    ItemData GetResourceData();
}
