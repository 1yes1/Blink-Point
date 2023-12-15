using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Python;
using System.IO;
using UnityEditor;

public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client; 
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;


    public void Start()
    {
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    [MenuItem("Python/Gaze Estimation")]
    static void RunEnsureNaming()
    {
        string scriptPath = Path.Combine(Application.dataPath, "Scripts\\gaze-estimation-master\\main.py");
        PythonRunner.RunFile(scriptPath);
    }


    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if (printToConsole) { print(data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

}
