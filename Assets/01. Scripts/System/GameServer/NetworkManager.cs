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
        //���� ��ǻ���� ȣ��Ʈ �̸��� �����´�.
        string host = Dns.GetHostName();
        //���� ��ǻ���� ȣ��Ʈ �̸��� Ȱ���ؼ� IP�ּҸ���Ʈ�� �޾ƿ´�. => GetHostAddresses�Լ��� ���� �Ǵ°� �ƴ��� Ȯ���ʿ�.
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //������ IP������ �޾ƿ´� (0���迭�� IPv4�ּҸ� ����Ų��)
        IPAddress ipAddr = ipHost.AddressList[0]; /*new IPAddress(new byte[] { 43,201,81,66 });*/
		//ȣ��Ʈ�� ���񽺿� ������ ���ø����̼ǿ� �ʿ��� ���öǴ� ���� ��Ʈ������ �����ϴ� endpoint����
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
		//ID ��� �����   : Enum���� ������ �̸� ã�Ƽ� ��ȯ
		string msgName = packet.Descriptor.Name.Replace("_", string.Empty);

		MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);
		//Size ��� ����� : ��Ŷ���� ������ üũ
		ushort size = (ushort)packet.CalculateSize();

		//��Ŷ ��ü�� ����Ʈ�迭�� ��ȯ
		byte[] sendBuffer = new byte[size + 4]; // ��������� 4 �߰��� �ӽ� ��Ŷ ���� ����
		Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));  // ID,Size,Payload ��ü ũ�� sendbuffer 0����Ʈ ��ġ�� �Ҵ�
		Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));  // 2����Ʈ ��ġ��, ID�� �Ҵ�
		Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);  // 4����Ʈ ��ġ�� ���̷ε� �Ҵ�

		Send(new ArraySegment<byte>(sendBuffer));
	}

	private void Send(ArraySegment<byte> arraySegment)
	{
		_session.Send(arraySegment);
	}

}
