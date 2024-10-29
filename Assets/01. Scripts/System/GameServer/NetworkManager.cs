using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class NetworkManager : MonoSingleton<NetworkManager>
{
    ServerSession _session = new ServerSession();

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
        Init();
	}

	public void Init()
    {
        //로컬 컴퓨터의 호스트 이름을 가져온다.
        string host = Dns.GetHostName();
        //포컬 컴퓨터의 호스트 이름을 활용해서 IP주소리스트를 받아온다. => GetHostAddresses함수를 쓰면 되는거 아닌지 확인필요.
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //유저의 IP정보를 받아온다 (0번배열은 IPv4주소를 가르킨다)
        IPAddress ipAddr = ipHost.AddressList[0]; /*new IPAddress(new byte[] { 43,201,81,66 });*/
		//호스트의 서비스에 연결할 애플리케이션에 필요한 로컬또는 원격 포트정보를 포함하는 endpoint생성
		IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => { return _session; }, 1);

    }

	public void Update()
	{
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }
    }

	public void Send(IMessage packet)
	{
		//ID 헤더 만들기   : Enum에서 동일한 이름 찾아서 변환
		string msgName = packet.Descriptor.Name.Replace("_", string.Empty);

		MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);
		//Size 헤더 만들기 : 패킷에서 사이즈 체크
		ushort size = (ushort)packet.CalculateSize();

		//패킷 객체를 바이트배열로 변환
		byte[] sendBuffer = new byte[size + 4]; // 헤더사이즈 4 추가한 임시 패킷 버퍼 생성
		Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));  // ID,Size,Payload 전체 크기 sendbuffer 0바이트 위치에 할당
		Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));  // 2바이트 위치에, ID값 할당
		Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);  // 4바이트 위치에 페이로드 할당

		Send(new ArraySegment<byte>(sendBuffer));
	}

	private void Send(ArraySegment<byte> arraySegment)
	{
		_session.Send(arraySegment);
	}

}
