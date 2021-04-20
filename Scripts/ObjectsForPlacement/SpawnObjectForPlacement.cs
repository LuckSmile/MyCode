using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpawnObjectForPlacement : MonoBehaviour
{
    [SerializeField] private Figurine prefabFigurine = null;
    [SerializeField] private Subject prefabSubject = null;
    [Space]
    [SerializeField] private RectTransform content = null;
    [Space]
    [SerializeField] private ObjectForPlacementData[] objectsForPlacementData = null;
    [Space]
    [SerializeField] private Player player = null;
    [SerializeField] private CurrencyData step = null;
    [SerializeField] private GameManager gameManager = null;
    [Space]
    [SerializeField] private bool isCellOneFigurine = true;
    public bool IsObserveStep { get; set; } = true;
    public string Indefecator => indefecator;
    private string indefecator = "";
    private List<ObjectForPlacementData> objectsRepeat = null;

    public event System.Action<Figurine> CreateFigurine;
    public event System.Action<Subject> CreateSubject;
    private void Awake()
    {
        indefecator = gameObject.GetInstanceID().ToString();
        objectsRepeat = new List<ObjectForPlacementData>();
    }
    private void Update()
    {
        if (objectsRepeat.Count > 0 && content.childCount == 0)
        {
            Create(objectsRepeat[0]);
            objectsRepeat.RemoveAt(0);
        }
        if (content.childCount == 0 && (IsObserveStep == false || player.Currencies[step].Amount > 0))
        {
            while(true)
            {
                for(int index = 0; index < objectsForPlacementData.Length; index++)
                {
                    int indexRandomNumber = Random.Range(0, objectsForPlacementData.Length);
                    ObjectForPlacementData temp = objectsForPlacementData[index];
                    objectsForPlacementData[index] = objectsForPlacementData[indexRandomNumber];
                    objectsForPlacementData[indexRandomNumber] = temp;
                }

                ObjectForPlacementData objectForPlacementData = objectsForPlacementData[Random.Range(0, objectsForPlacementData.Length)];
                int randomNumber = Random.Range(0, 100);
                if (randomNumber <= objectForPlacementData.Chance)
                {
                    if (objectForPlacementData is SubjectData && (isCellOneFigurine == false || gameManager.Cells.Where(x => x.Value.Selected != null).Count() > 0))
                    {
                        Create(objectForPlacementData);
                        if (IsObserveStep == true)
                            player.Currencies[step].Amount--;
                        break;
                    }
                    else if(objectForPlacementData is FigurineData)
                    {
                        Create(objectForPlacementData);
                        if(IsObserveStep == true)
                            player.Currencies[step].Amount--;
                        break;
                    }
                    else if (objectForPlacementData is  Accessories)
                    {
                        Accessories accessories = objectForPlacementData as Accessories;
                        if(gameManager.Cells.Where(x => x.Value.Selected != null).Select(x => x.Value.Selected.Data).Contains(accessories.Figurine))
                        {
                            Create(accessories.Subject);
                            if (IsObserveStep == true)
                                player.Currencies[step].Amount--;
                            break;
                        }
                        else
                        {
                            Create(accessories.Figurine);
                            if (IsObserveStep == true)
                                player.Currencies[step].Amount--;
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <param name="parent">Default use own parent</param>
    public void Create(ObjectForPlacementData data, RectTransform parent = null)
    {
        if (parent == null)
            parent = content;

        if (data is FigurineData)
        {
            Figurine figurine = Figurine.Create(prefabFigurine, parent, data as FigurineData) as Figurine;
            figurine.name = indefecator;
            CreateFigurine?.Invoke(figurine);
        }
        else if (data is SubjectData)
        {
            Subject subject = Subject.Create(prefabSubject, parent, data as SubjectData) as Subject;
            subject.name = indefecator;
            CreateSubject?.Invoke(subject);
        }
    }
    /// <summary>
    /// Добавить в очередь
    /// </summary>
    /// <param name="objectForPlacement"></param>
    public void AddQueue(ObjectForPlacementData objectForPlacement)
    {
        Clear();
        Create(objectForPlacement);
    }
    public void Clear()
    {
        if (content.childCount > 0)
        {
            ClearContent<FigurineData>();
            ClearContent<SubjectData>();
        }
    }
    private void ClearContent<TData>() where TData : ObjectForPlacementData
    {
        ObjectForPlacement<TData> obj = content.GetComponentInChildren<ObjectForPlacement<TData>>();
        if(obj != null)
        {
            ObjectForPlacementData dataRepeat = obj.Data;
            this.objectsRepeat.Add(dataRepeat);
            Destroy(obj.gameObject);
        }
    }
}
