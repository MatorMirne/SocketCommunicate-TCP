using Protocol;

namespace TCPServer;

public partial class Program
{
	static async Task<(bool IsSuccess, MessageResponse Response)> ProcessAsync(MessageRequest request)
	{
		Console.WriteLine($"수신 : {request.Message}");
		var response = new MessageResponse();

		try
		{
			response.Message = "response message";
			response.Result = Result.Success;
			return (true, response);
		}
		catch
		{
			return (false, response);
		}
	}
}