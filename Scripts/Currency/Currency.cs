using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Currency
{ 
    public CurrencyData Data => data;
    public uint Amount
    {
        get => amount;
        set
        {
            amount = value;
            if (counter != null)
            {
                counter.Amount = amount;
            }
        }
    }
    public string AlternativeAmount => alternativeAmount;

    [SerializeField] private CurrencyData data = null;
    [SerializeField] private uint amount = 0;
    [SerializeField] private UICounter counter = null;
    [SerializeField] private string alternativeAmount = "";


    public Currency(CurrencyData data)
    {
        this.data = data;
    }
    public Currency(CurrencyData data, uint amount) : this(data)
    {
        this.amount = amount;
    }
}
