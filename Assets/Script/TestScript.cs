using System.Linq;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private TextTyper[] testTextTyper = new TextTyper[1];
    [SerializeField] private TextTyper orign;
    [SerializeField] private Transform parents;
    [SerializeField] private DTScrollView _scrollView;
    [SerializeField] private GameObject _scrollViewItem;
    [TextArea(3, 100)]
    [SerializeField] private string test;
    // Update is called once per frame
    void Start()
    {
        GameDataManager.Instance.LoadScenarioData(0,1);
        // for (int i = 0; i < 1; ++i)
        // {
        //     testTextTyper[i] = orign;
        //     testTextTyper[i].TypeText(test);
        // }
        //
        // _scrollView.InitScrollView(OnUpdateScrollView, _scrollViewItem);
        // _scrollView.MakeList(10);
    }

    GameObject OnUpdateScrollView(int index)
    {
        var item = _scrollView.GetItem(index);
        var typer = item.transform.Find("Text").GetComponent<TextTyper>();
        
        typer.TypeText(test);
        return item;
    }
}
