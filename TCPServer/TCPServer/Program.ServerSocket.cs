using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TCPServer;

public partial class Program
{
	static async Task RunAsync(Remote remote)
	{
		while (true)
		{
			var receive = await ReceiveAsync(remote);
			if (receive.IsSuccess == false) 
				break;
			// Send(remote, receive.Response);
		}

		Close(remote);
	}

	static async Task<(bool IsSuccess, ProtocolResponse Response)> ReceiveAsync(Remote remote)
	{
		int length = await remote.socket.ReceiveAsync(remote.receiveBuffer, SocketFlags.None);
		string rcv = System.Text.Encoding.UTF8.GetString(remote.receiveBuffer, 0, length);

		if (rcv == "" || rcv.Length == 0)
			return (false, null);
		
		if (remote.socket.Connected == false)
			return (false, null);
		
		// Console.WriteLine($"수신 : {rcv}");
		
		var obj = JsonConvert.DeserializeObject<JObject>(rcv);

		if (obj.TryGetValue("ProtocolId", out var value))
		{
			(bool IsSuccess, ProtocolResponse Response) Result = (false, null);
			switch (value?.Value<int>())
			{
				case (int)ProtocolId.Handshake:
					Result = await ProcessAsync(JsonConvert.DeserializeObject<HandshakeRequest>(rcv));
					break;
				case (int)ProtocolId.Mesasge:
					Result = await ProcessAsync(JsonConvert.DeserializeObject<MessageRequest>(rcv));
					break;
			}
			
			if(Result.Response != null) return Result;
		}

		return (false, null);
	}

	static void Send(Remote remote, ProtocolResponse response)
	{
		string json = JsonSerializer.Serialize(response);
		remote.sendBuffer = System.Text.Encoding.UTF8.GetBytes(json);
		remote.socket.Send(remote.sendBuffer, 0, json.Length, SocketFlags.None);
	}

	static void Close(Remote remote)
	{
		remote.socket.Shutdown(SocketShutdown.Both);
		remote.socket.Close();
		remote.socket = null;

		// [순서 중요] 메모리를 해제한 후 socket을 제거해야 합니다.
		RemotePool.RemoveConnection(remote);
	}
}