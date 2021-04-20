using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataItemPrice", menuName = "Data/Window/Data/ItemPrice")]
public class DataItemPrice : ScriptableObject
{
    public ObjectForPlacementData Item => item;
    [SerializeField] private ObjectForPlacementData item = null;
    public Sprite Body => body;
    [SerializeField] private Sprite body = null;
    public Currency Price => price;
    [SerializeField] private Currency price = null;
}
