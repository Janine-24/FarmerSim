using UnityEngine;

public class Merchantanimator : MonoBehaviour
{
    public float produceInterval = 10f;
    public GameObject productPrefab;
    private float timer;
    public bool isFed = false;

    void Start()
    {
        enabled = false;
    }

    void Update()
    {
        if (!isFed) return;

        timer += Time.deltaTime;
        if (timer >= produceInterval)
        {
            ProduceProduct();
            isFed = false;
            timer = 0f;
        }
    }

    public void Feed()
    {
        isFed = true;
    }

    void ProduceProduct()
    {
        Instantiate(productPrefab, transform.position, Quaternion.identity);
        WarehouseManager.Instance.AddItem(productPrefab.name, 1);
    }
}