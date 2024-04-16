using UnityEngine.UIElements;
using UnityEngine;

public class PointBarControl : VisualElement
{

    public new class UxmlFactory : UxmlFactory<PointBarControl, UxmlTraits> {

    }

    public new class UxmlTraits : VisualElement.UxmlTraits {
        UxmlIntAttributeDescription maxPointsAttribute = new UxmlIntAttributeDescription() {
            name = "Max_Points"
        };
        UxmlIntAttributeDescription fullPointsAttribute = new UxmlIntAttributeDescription() {
            name = "Full_Points"
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
            base.Init(ve, bag, cc);

            (ve as PointBarControl).MaxPoints = maxPointsAttribute.GetValueFromBag(bag, cc);
            (ve as PointBarControl).FullPoints = fullPointsAttribute.GetValueFromBag(bag, cc);
        }
    }

    private const string styleResource = "PointBarControlUSS";
    private const string containerClassName = "pointContainer";
    private const string pointTemplateName = "PointTemplate";
    private const string pointFullName = "pointFull";
    private const string pointEmptyName = "pointEmpty";

    private VisualTreeAsset pointTemplate;

    private VisualElement pointContainer;

    private VisualElement[] points;

    private int maxPoints;
    public int MaxPoints {
        get { return maxPoints; }
        set {
            maxPoints = value;
            CreatePoints();
        }
    }
    private int fullPoints;
    public int FullPoints {
        get { return fullPoints; }
        set {
            fullPoints = value;
            UpdateFullPoints();
        }
    }

    public PointBarControl () {

        styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
        pointTemplate = Resources.Load<VisualTreeAsset>(pointTemplateName);

        pointContainer = new VisualElement();
        pointContainer.name = "PointContainer";
        pointContainer.AddToClassList(containerClassName);

        hierarchy.Add(pointContainer);
        CreatePoints();
    }

    private void CreatePoints () {
        pointContainer.Clear();
        points = new VisualElement[maxPoints];
        for (int i = 0; i < maxPoints; i++) {
            var point = pointTemplate.Instantiate();
            point.name = "Point_" + i;
            point.style.flexDirection = FlexDirection.Row;
            pointContainer.Add(point);
            points[i] = point;
        }
    }

    private void UpdateFullPoints () {
        for (int i = 0; i < points.Length; i++) {
            points[i].Q("Point").RemoveFromClassList(pointFullName);
            points[i].Q("Point").RemoveFromClassList(pointEmptyName);
            points[i].Q("Point").AddToClassList(i < FullPoints ? pointFullName : pointEmptyName);
        }
    }

}
