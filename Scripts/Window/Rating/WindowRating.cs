using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowRating : Window
{
    [SerializeField] private UIRating prefab = null;
    [SerializeField] private RectTransform content = null;
    private List<UIRating> uIRatings = null;
    private void Awake()
    {
        AllCreate(10);
    }
    public override void Open()
    {
        this.gameObject.SetActive(true);
    }
    private void AllCreate(int count)
    {
        uIRatings = new List<UIRating>();
        for (int index = 0; index < count; index++)
        {
            UIRating uIRating = Instantiate(prefab, content);
            uIRatings.Add(uIRating);
        }
    }
}
