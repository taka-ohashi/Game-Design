using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RobberSelectUI : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        unhighlight();
    }
 
    void Update()
    {
        
    }

/*    public void OnPointerEnter(PointerEventData eventData)
    {
        //highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
//        unhighlight();
    }
*/
    public void highlight()
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        button.colors = colors;
    }

    public void unhighlight()
    {
        ColorBlock colors = button.colors;
        colors.normalColor = new Color32(170, 170, 170, 255);
        colors.highlightedColor = Color.white;
        button.colors = colors;
    }

}
