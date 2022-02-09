using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    //Lobby
    [SerializeField] GameObject lobbyGUI;
    [SerializeField] GameObject lobbyCamera;
    public InputField ipInput;
    public InputField usernameInput;

    //Game GUI
    [SerializeField] GameObject gameGUI;
    [SerializeField] GameObject HealthBarPrefab;



    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectToServer()
    {
        SetupGame();
        NetworkManager.Singleton.Connect(ipInput.text);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerID.name);
        message.AddString(usernameInput.text);
        NetworkManager.Singleton.Client.Send(message);
    }

    private void SetupGame()
    {
        //Disabling Lobby
        lobbyGUI.SetActive(false);
        lobbyCamera.SetActive(false);

        //Entering Game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetupGameGUI();
    }

    void SetupGameGUI()
    {
        gameGUI.SetActive(true);
    }
    public void BackToMain()
    {
        //Leaving Game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RemoveGameGUI();

        //Enabling Lobby
        lobbyGUI.SetActive(true);
        lobbyCamera.SetActive(true);
    }

    void RemoveGameGUI()
    {
        gameGUI.SetActive(false);
    }
}
