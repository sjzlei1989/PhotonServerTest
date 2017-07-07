using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerNameInputField : MonoBehaviour
{
    private string defaultName = "";
    private string playerPrefName = "PlayerName";
    public InputField input;
	void Start ()
	{
        if(null == input) {
            input = gameObject.GetComponent<InputField>();
        }
        if(PlayerPrefs.HasKey(playerPrefName)) {
            defaultName = PlayerPrefs.GetString(playerPrefName);
            input.text = defaultName;
        }
        else {
            defaultName = input.text;
        }

        PhotonNetwork.playerName = defaultName;
	}

    public void OnTextChanged(string _value)
    {
        if(!string.IsNullOrEmpty(_value)) {
            PhotonNetwork.playerName = _value;
            PlayerPrefs.SetString(playerPrefName, _value);
        }
    }
}
