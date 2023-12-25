using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod()]
    static void Initialize()
    {
        if (Instance != null) return;
        Instance = new GameObject("LoginManager").AddComponent<LoginManager>();
        DontDestroyOnLoad(Instance);

        Instance.Login();
    }

    Socket Socket;


    void Login()
    {
        ConnectToTcpServer();
    }

    void ConnectToTcpServer()
    {
        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress serverIPAdress = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverEndPoint = new(serverIPAdress, 8888);

        //서버로 연결 요청
        try
        {
            Debug.Log("Connecting to Server");
            Socket.Connect(serverEndPoint);

            var packet = new SimplePacket()
            {
                mouseX = 10,
                mouseY = 20
            };
            Socket.Send(SimplePacket.ToByteArray(packet));
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed:" + e.Message);
        }
    }


    [Serializable]  //하나로 직렬화 묶겠다. 뜻? 바이트화 하겠다?
    public class SimplePacket       //모노비헤이비어는 싱글톤으로 만들거라서 여기서는 삭제
    {

        public float mouseX = 0.0f;
        public float mouseY = 0.0f;


        public static byte[] ToByteArray(SimplePacket packet)
        {
            //스트림생성 한다.  물흘려보내기
            MemoryStream stream = new MemoryStream();

            //스트림으로 건너온 패킷을 포맷으로 바이너리 묶어준다.
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, packet.mouseX);       //스트림에 담는다. 시리얼라이즈는 담는다는 뜻임.
            formatter.Serialize(stream, packet.mouseY);

            return stream.ToArray();
        }
    }


}