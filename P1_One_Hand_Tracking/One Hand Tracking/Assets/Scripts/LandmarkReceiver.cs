using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class LandmarkReceiver : MonoBehaviour
{
    private UdpClient udpClient;
    private Thread receiveThread;
    [HideInInspector] public string receivedData = ""; // Inspector에서 숨김

    public int port = 5052;

    void Start()
    {
        udpClient = new UdpClient(port);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log($"Listening for UDP data on port {port}...");
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
        try
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                receivedData = Encoding.UTF8.GetString(data);
            }
        }
        catch (SocketException ex)
        {
            Debug.LogError($"Socket error: {ex.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        udpClient.Close();
    }
}
