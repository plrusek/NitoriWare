﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	[SerializeField]
	private Prefix keyPrefix;
	[SerializeField]
	private string _key;
    [SerializeField]
    private Parameter[] parameters;

	public string key
	{
		get {return _key;}
		set { _key = value; updateText(); }
    }

    private TextLimitSize limitSize;    //Force update when text is changed

    [System.Serializable]
    public struct Parameter
    {
        public string value;
        public bool isKey;
        public string keyDefaultString;
    }

    private Text textComponent;
	private TextMesh textMesh;
    private string initialText, loadedLanguage;

	private enum Prefix
	{
		None,
		CurrentMicrogame
	}

	void Start ()
	{
		textComponent = GetComponent<Text>();
		textMesh = GetComponent<TextMesh>();
        limitSize = GetComponent<TextLimitSize>();
        loadedLanguage = "";
        initialText = getText();
        updateText();
    }

    private void LateUpdate()
    {
        
        if (loadedLanguage != TextHelper.getLoadedLanguage())
        {
            setText(initialText);
            updateText();
        }
    }

    /// <summary>
    /// Sets the key to load from and reloads the text with the new key
    /// </summary>
    /// <param name="key"></param>
    public void setKey(string key)
	{
		this._key = key;
		updateText();
	}

	public void updateText()
	{
        loadedLanguage = TextHelper.getLoadedLanguage();
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(loadedLanguage))
            return;

		string value;
		if (keyPrefix == Prefix.CurrentMicrogame)
			value = TextHelper.getLocalizedMicrogameText(key, getText(), parameters);
		else
			value = TextHelper.getLocalizedText(getPrefixedKey(), getText(), parameters);

		setText(value);

        //if (limitSize != null)
        //{
        //    var component = GetComponent<TextLimitSize>();
        //    component.updateScale();
        //    //if (textComponent != null)
        //    //    ((CanvasTextLimitSize)limitSize).updateScale();
        //    //else if (textMesh != null)
        //    //    ((TextMeshLimitSize)limitSize).updateScale();
        //}
    }

	private void setText(string text)
	{
		if (textComponent != null)
			textComponent.text = text;
		else if (textMesh != null)
			textMesh.text = text;
	}

	private string getText()
	{
		if (textComponent != null)
			return textComponent.text;
		if (textMesh != null)
			return textMesh.text;
		return "";
	}

	string getPrefixedKey()
	{
		switch(keyPrefix)
		{
			//Handled seperately
			//case (Prefix.CurrentMicrogame):
			//	return "microgame." + gameObject.scene.name.Substring(0, gameObject.scene.name.Length - 1) + ".";
			default:
				return key;
		}
	}
}
