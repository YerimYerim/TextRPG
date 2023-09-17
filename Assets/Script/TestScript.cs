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
    [SerializeField] private GameObject[] _scrollViewItem;
    [TextArea(3, 100)]
    [SerializeField] private string test;
    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < 1; ++i)
        {
            testTextTyper[i] = orign;
            testTextTyper[i].TypeText(test);
        }

        _scrollView.InitScrollView(_scrollViewItem, OnUpdateScrollView);
        _scrollView.MakeList(10);
    }

    void OnUpdateScrollView(int index)
    {
        
    }
}
