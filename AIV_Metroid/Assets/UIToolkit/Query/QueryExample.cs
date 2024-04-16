using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

public class QueryExample : MonoBehaviour
{

    private void Start() {


        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        //VisualElement e = root.Q("Elem1");

        //List<VisualElement> e = root.Q("ElemContainer").Query<VisualElement>().ToList();

        //root.Query(className: "basicSquareElem").ForEach(elem => Debug.Log(elem.name));

        //SetMarkers(e);

        //SetMarker(root.Q("Label1"));
        //SetMarker(root.Query("Label1").First());
        //SetMarker(root.Q("Label3"));
        //SetMarker(root.Q("Elem1").Q<Label>());
        //SetMarker(root.Q("Elem3").Q<Label>());
        List<Label> list = root.Query<Label>().ToList();
        list.RemoveAt(1);
        SetMarkers(list);

    }


    private void SetMarkers<T> (List<T> list) where T: VisualElement {
        foreach (var elem in list) {
            SetMarker(elem);
        }
    }

    private void SetMarker<T> (T e) where T: VisualElement {
        e.AddToClassList("marker");
    }

}
