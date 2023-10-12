namespace Protocol;

public class MessageRequest : ProtocolRequest
{
	public string Message { get; set; }
	public MessageRequest() : base(ProtocolId.Mesasge) { }
}

public class MessageResponse : ProtocolResponse
{
	public string Message { get; set; }
	public MessageResponse() : base(ProtocolId.Mesasge) { }
}