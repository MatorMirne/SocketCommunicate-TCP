namespace Protocol;

public abstract class Protocol
{
	public ProtocolType protocolType { get; set; }
}

public enum ProtocolType : int
{
	Mesasge = 100,
	
	None = 999,
	Error =1000,
}