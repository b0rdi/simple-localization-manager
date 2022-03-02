using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LocalizationManager : MonoBehaviour
{
	public static LocalizationManager Instance;
	public string defaultLanguage = "en";
	public string defaultLocalizationFolder = "Languages";
	public Language currentLanguage;
	public List<Language> availableLanguages = new List<Language>();

	private void Awake()
	{
		Instance = this;
		LoadDefaultLocalizations();
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(defaultLanguage))
		{
			ChangeLanguage(defaultLanguage);
		}
	}

	private void LoadDefaultLocalizations()
	{
		if (!string.IsNullOrEmpty(defaultLanguage))
		{
			TextAsset[] textAssets = Resources.LoadAll<TextAsset>(defaultLocalizationFolder);
			if (textAssets.Length > 0)
			{
				Language newLanguage;
				foreach (TextAsset langFile in textAssets)
				{
					newLanguage = new Language(langFile.name, langFile);
				}
			}
			else
			{
				Debug.LogErrorFormat("[{0}] Could not find any localization!", "LocalizationManager");
			}
		}
		else
		{
			Debug.LogErrorFormat("[{0}] Wrong default folder!", "LocalizationManager");
		}
	}

	public void ChangeLanguage(string _id)
	{
		if (!currentLanguage.id.Equals(_id))
		{
			foreach (Language language in availableLanguages)
			{
				if (language.id.Equals(_id))
				{
					currentLanguage = language;
					Debug.LogFormat("[{0}] Language changed to {1}", "LocalizationManager", _id);
					ChangeLocalization();
					break;
				}
			}
		}
	}

	public Action OnLocalizationChange;
	private void ChangeLocalization()
	{
		OnLocalizationChange?.Invoke();
	}
}

[Serializable]
public class Language
{
	public string id;
	private TextAsset asset;
	private JObject jsonObject;

	public Language(string _id, TextAsset _asset)
	{
		id = _id;
		asset = _asset;
		jsonObject = (JObject)JsonConvert.DeserializeObject(asset.text);
		LocalizationManager.Instance.availableLanguages.Add(this);
	}

	public string GetLocalizedText(string _textItemId)
	{
		jsonObject.TryGetValue(_textItemId, out JToken langValue);
		if (langValue != null)
		{
			string text = langValue.ToString();
			if (!string.IsNullOrEmpty(text.ToString()))
			{
				return langValue.ToString();
			}
			else
			{
				Debug.LogErrorFormat("[{0}] Cannot get localization for \"{1}\" on language \"{2}\".", "LocalizationManager", _textItemId, id);
				return _textItemId;
			}
		}
		else
		{
			Debug.LogErrorFormat("[{0}] Cannot get localization for \"{1}\" on language \"{2}\".", "LocalizationManager", _textItemId, id);
			return _textItemId;
		}
	}
}