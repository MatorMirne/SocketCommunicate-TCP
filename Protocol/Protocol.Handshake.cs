namespace Protocol;

public class HandshakeRequest : ProtocolRequest
{
	public int ClientId { get; set; }	// 0~999
	public HandshakeRequest() : base(ProtocolId.Handshake) { }
}

public class HandshakeResponse : ProtocolResponse
{
	public int ServerId { get; set; }	// 1000~
	public HandshakeResponse() : base(ProtocolId.Handshake) { }
}