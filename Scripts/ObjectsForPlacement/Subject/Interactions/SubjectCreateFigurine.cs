using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SubjectCreateFigurine", menuName = "Data/Subject/Interactions/CreateFigurine")]
public class SubjectCreateFigurine : SubjectInteractions
{
    [SerializeField] private FigurineData figurineData = null;
    [SerializeField] private Figurine prefab = null;
    public override bool CheckingUse((Cell cell, Subject subject) selected, GameManager gameManager) => selected.cell.Selected == null;
    public override void Use((Cell cell, Subject subject) selected, GameManager gameManager)
    {
        if (CheckingUse(selected, gameManager))
        {
            Cell cell = selected.cell;
            Figurine figurine = Figurine.Create(prefab, cell.GetComponent<RectTransform>(), figurineData) as Figurine;
            cell.Selected = figurine;
            figurine.GetComponent<ControlDrag>().Interaction = false;
            for (int index = 0; index < figurine.Data.Interactions.Length; index++)
            {
                FigurineInteractions interaction = figurine.Data.Interactions[index];
                if (interaction.ActivateOnDrop == true)
                {
                    interaction.Use(selected.cell, gameManager);
                }
            }
            gameManager.InteractionsFigure(selected.cell);
        }
    }
}
