using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationItem : MonoBehaviour
{
	public string id;
	private TextMeshProUGUI tmp;

	private void Awake()
	{
		tmp = GetComponent<TextMeshProUGUI>();
		LocalizationManager.Instance.OnLocalizationChange += SetLocalizedText;
	}

	private void SetLocalizedText()
	{
		tmp.text = LocalizationManager.Instance.currentLanguage.GetLocalizedText(id);
	}

	private void OnApplicationQuit()
	{
		LocalizationManager.Instance.OnLocalizationChange -= SetLocalizedText;
	}
}