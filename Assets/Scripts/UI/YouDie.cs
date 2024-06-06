using TMPro;
using UnityEngine;

public class YouDie : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI reasonText;

    public void Show(string reason)
    {
        gameObject.SetActive(true);
        reasonText.text = reason;
    }
}
