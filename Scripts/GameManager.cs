﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : SaveComponentsCell
{
    public static Player Player { get; private set; }
    [SerializeField] private Player player = null;
    public SpawnObjectForPlacement SpawnObjectForPlacement => spawnObjectForPlacement;
    [SerializeField] private SpawnObjectForPlacement spawnObjectForPlacement;
    [SerializeField] private SpawnObjectForPlacement spawnAuxiliaryFigurine;
    public IReadOnlyDictionary<Vector2, Cell> Cells => cells;
    private readonly Dictionary<Vector2, Cell> cells = new Dictionary<Vector2, Cell>();

    private void Awake()
    {
        GameManager.Player = player;
    }
    private Dictionary<Figurine, Cell> GetFigurines()
    {
        Dictionary<Figurine, Cell> figurines = new Dictionary<Figurine, Cell>();
        foreach (KeyValuePair<Vector2, Cell> location in this.cells)
        {
            if (location.Value.Selected != null)
            {
                figurines.Add(location.Value.Selected, location.Value);
            }
        }
        return figurines;
    }
    private void Swap<T>(int selected, int replacement, ref List<T> list)
    {
        T temp = list[selected];
        list[selected] = list[replacement];
        list[replacement] = temp;
    }
    public void InteractionsFigure(Cell selected)
    {
        Dictionary<Figurine, Cell> figurineAndLocation = GetFigurines();

        if (figurineAndLocation.Count > 0)
        {
            List<Figurine> figurines = figurineAndLocation.Keys.ToList();
            for(int index = 0; index < figurines.Count; index++)
            {
                int numberRandom = Random.Range(0, figurines.Count);
                Swap(index, numberRandom, ref figurines);
            }

            if(selected.Selected != null)
            {
                int indexSelected = figurines.IndexOf(selected.Selected);
                Swap(0, indexSelected, ref figurines);
            }

            List<Figurine> blackList = new List<Figurine>();
            Dictionary<FigurineInteractions, List<Figurine>> repeatedly = new Dictionary<FigurineInteractions, List<Figurine>>();
            for(int index = 0; index < figurines.Count; index++)
            {
                Figurine figurine = figurines[index];
                if (figurine != null && blackList.Contains(figurine) == false && figurine.Interaction == true)
                {
                    blackList.Add(figurine);
                    FigurineData data = figurine.Data;
                    for (int indexInteraction = 0; indexInteraction < data.Interactions.Length; indexInteraction++)
                    {
                        FigurineInteractions figurineInteraction = data.Interactions[indexInteraction];
                        
                        if(figurine == selected.Selected && figurineInteraction.ActivateOnDrop != true)
                        {
                            figurine.InteractionsColmlited.Add(figurineInteraction);
                            continue;
                        }
                        if (figurineInteraction.Use(figurineAndLocation[figurine], this) == false)
                        {
                            if (repeatedly.ContainsKey(figurineInteraction) == false)
                            {
                                repeatedly.Add(figurineInteraction, new List<Figurine>());
                            }
                            repeatedly[figurineInteraction].Add(figurine);
                        }
                        else
                        {
                            figurine.InteractionsColmlited.Add(figurineInteraction);
                        }
                    }
                }
            }

            int checkIndex = 0;
            List<FigurineInteractions> figurineInteractions = repeatedly.Keys.ToList();
            while (repeatedly.Count > 0)
            {
                FigurineInteractions figurineInteraction = figurineInteractions[checkIndex];
                List<Figurine> figurinesRepeatedly = repeatedly[figurineInteraction];

                figurineAndLocation = GetFigurines();

                for (int index = 0; index < figurinesRepeatedly.Count; index++)
                {
                    Figurine figurine = figurinesRepeatedly[index];
                    if(figurine != null)
                    {
                        if (figurineAndLocation.ContainsKey(figurine) == true && figurineInteraction.Use(figurineAndLocation[figurine], this) == true)
                        {
                            repeatedly[figurineInteraction].Remove(figurine);
                            figurine.InteractionsColmlited.Add(figurineInteraction);
                            if (repeatedly[figurineInteraction].Count == 0)
                                repeatedly.Remove(figurineInteraction);
                        }
                        else
                        {
                            repeatedly[figurineInteraction].Remove(figurine);
                            if (repeatedly[figurineInteraction].Count == 0)
                                repeatedly.Remove(figurineInteraction);
                        }
                    }
                }

                checkIndex++;
                if (checkIndex >= repeatedly.Count)
                {
                    checkIndex = 0;
                }
            }


            figurines.ForEach(x =>
            {
                if (x != null) x.InteractionsColmlited.Clear();
            });
        }
    }

    private void CancelSubject(Subject subject)
    {
        subject.transform.SetParent(subject.transform.parent.parent);
        Destroy(subject.gameObject);
        if (subject.name.Contains(spawnObjectForPlacement.Indefecator) == true)
        {
            spawnObjectForPlacement.AddQueue(subject.Data);
        }
        else
        {
            spawnAuxiliaryFigurine.AddQueue(subject.Data);
        }
    }

    private void InteractionsSubject(Cell cell, Subject subject)
    {
        SubjectInteractions[] interactions = subject.Data.Interactions;
        bool errorChecking = false;
        int complited = 0;
        for (int index = 0; index < interactions.Length; index++)
        {
            SubjectInteractions subjectInteraction = interactions[index];
            if(subjectInteraction.CheckingUse((cell, subject), this) == false)
            {
                errorChecking = true;
                if(subject.Data.TypeCompleted == SubjectData.TypesCompleted.All)
                {
                    break;
                }
            }
            else
            {
                complited++;
            }
        }

        switch(subject.Data.TypeCompleted)
        {
            case SubjectData.TypesCompleted.All:
                if (errorChecking == false)
                {
                    subject.transform.SetParent(subject.transform.parent.parent);
                    for (int index = 0; index < interactions.Length; index++)
                    {
                        SubjectInteractions subjectInteraction = interactions[index];
                        subjectInteraction.Use((cell, subject), this);
                    }
                    Destroy(subject.gameObject);
                }
                else
                {
                    CancelSubject(subject);
                }
                break;
            case SubjectData.TypesCompleted.NecessarilyCompleteds:
                if (complited > 0)
                {
                    subject.transform.SetParent(subject.transform.parent.parent);
                    for (int index = 0; index < interactions.Length; index++)
                    {
                        SubjectInteractions subjectInteraction = interactions[index];
                        subjectInteraction.Use((cell, subject), this);
                    }
                    Destroy(subject.gameObject);
                }
                else
                {
                    CancelSubject(subject);
                }
                break;
            case SubjectData.TypesCompleted.OneCompleted:
                if (complited > 0)
                {
                    subject.transform.SetParent(subject.transform.parent.parent);
                    for (int index = 0; index < interactions.Length; index++)
                    {
                        SubjectInteractions subjectInteraction = interactions[index];
                        if (subjectInteraction.CheckingUse((cell, subject), this) == true)
                        {
                            subjectInteraction.Use((cell, subject), this);
                            break;
                        }
                    }
                    Destroy(subject.gameObject);
                }
                else
                {
                    CancelSubject(subject);
                }
                break;
        }
    }
    public override void OnSave(Cell component)
    {
        component.OnDropFigureActivete += () => InteractionsFigure(component);
        component.OnDropSubjectActivete += InteractionsSubject;
        cells.Add(component.Index, component);
    }

}
