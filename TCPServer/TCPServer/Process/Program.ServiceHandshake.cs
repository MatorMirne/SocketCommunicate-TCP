using Protocol;

namespace TCPServer;

public partial class Program
{
	static async Task<(bool IsSuccess, HandshakeResponse Response)> ProcessAsync(HandshakeRequest request)
	{
		Console.WriteLine($"수신 : {request.ClientId}");
		PrintState("all");
		var response = new HandshakeResponse();

		try
		{
			if (request.ClientId > 1000)
				Console.WriteLine($"이미 서버에 할당된 아이디입니다. : {request.ClientId}");

			response.ServerId = Remote.idCount++;
			Console.WriteLine($"clientID:{request.ClientId} -> serverId:{response.ServerId}");

			response.Result = Result.Success;
			return (true, response);
		}
		catch
		{
			return (false, response);
		}
	}
}