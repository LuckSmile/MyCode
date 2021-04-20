using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIItemPrice : MonoBehaviour
{
    [SerializeField] private DataItemPrice data = null;
    [Space]
    [SerializeField] private Image body = null;
    [Space]
    [SerializeField] private Image iconPrice = null;
    [SerializeField] private TextMeshProUGUI textPrice = null;
    public event System.Action EventOnClickAgree;
    public static UIItemPrice Create(UIItemPrice prefab, RectTransform parent, DataItemPrice data)
    {
        UIItemPrice answer = Instantiate(prefab, parent);
        answer.data = data;
        
        answer.body.sprite = data.Body;
        if (data.Price.AlternativeAmount != "")
        {
            answer.iconPrice.gameObject.SetActive(false);
            answer.textPrice.text = data.Price.AlternativeAmount;
        }
        else
        {
            answer.textPrice.text = data.Price.Amount.ToString();
            answer.iconPrice.sprite = data.Price.Data.Icon;
        }
        
        return answer;
    }
    public void Agree()
    {
        EventOnClickAgree?.Invoke();
    }
}
