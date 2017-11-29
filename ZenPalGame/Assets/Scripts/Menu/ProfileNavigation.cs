using UnityEngine;
using System.Collections;

public class ProfileNavigation : MonoBehaviour {

    private GameObject[] profiles;
    private float offset = -480f;
    private bool sliding = false;
    private int selected = -1;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (gameObject.transform.childCount == 0 && profiles == null)
        {
            return;
        }
        else if (gameObject.transform.childCount == 0 && profiles != null)
        {
            profiles = null;
            return;
        }
        else if (gameObject.transform.childCount != 0 && profiles == null)
        {
            profiles = new GameObject[transform.childCount];
            for(int i = 0; i < profiles.Length; i++)
            {
                profiles[i] = transform.GetChild(i).gameObject;
            }
        }

        if (Input.GetAxisRaw("Left Stick Horizontal") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            selected = FindSelected();
            if (selected != -1)
            {
                sliding = true;
            }
        }
        if(sliding)
        {
            LerpToBttn(offset * selected);
            if(rectTransform.anchoredPosition.x - (offset*selected) < - 0.01f)
            {
                sliding = false;
            }
        }
    }

    int FindSelected()
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            if (profiles[i] == null)
            {
                break;
            }
            else
            {
                if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == profiles[i])
                {
                    return i;
                }
            }
        }
            return -1;
    }

    void LerpToBttn(float position)
    {
        float newX = Mathf.Lerp(rectTransform.anchoredPosition.x, position, Time.deltaTime * 20f);
        Vector2 newPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
        rectTransform.anchoredPosition = newPosition;
    }
}
