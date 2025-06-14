using System.Collections.Generic;

[System.Serializable]
public class PlacedObjectData
{
    public string prefabName;
    public float posX, posY;
    public string extraData; // machineID、animalType、habitatType等
}

[System.Serializable]
public class PlacedObjectDataList
{
    public List<PlacedObjectData> objects = new List<PlacedObjectData>();
}
