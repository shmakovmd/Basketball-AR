using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class Connection : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private Transform gameArea;
    [SerializeField] private TMP_InputField inputField;

    public void ConnectAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ConnectAsServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void ConnectAsClient()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(inputField.text, 7777);
        NetworkManager.Singleton.StartClient();
        ball.transform.parent = gameArea;
    }
}