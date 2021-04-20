using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowShop : Window
{
    [SerializeField] private UIItemPrice prefab = null;
    [SerializeField] private RectTransform parent = null;
    [SerializeField] private SpawnObjectForPlacement spawnObjectForPlacement = null;
    [SerializeField] private Player player = null;
    [Space]
    [SerializeField] private DataItemPrice[] items = null;
    private void Awake()
    {
        for(int index = 0; index < items.Length; index++)
        {
            DataItemPrice dataItem = items[index];
            UIItemPrice uIItem = UIItemPrice.Create(prefab, parent, dataItem);
            uIItem.EventOnClickAgree += () =>
            {
                if((int)player.Currencies[dataItem.Price.Data].Amount - dataItem.Price.Amount >= 0)
                {
                    player.Currencies[dataItem.Price.Data].Amount -= dataItem.Price.Amount;
                    spawnObjectForPlacement.AddQueue(dataItem.Item);
                    this.Close();
                }
            };
            uIItem.transform.localScale = new Vector3(1, 1, 1);
        }
        this.Close();
    }
    public override void Open()
    {
        base.Open();
    }
}
