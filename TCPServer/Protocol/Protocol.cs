namespace Protocol;

public enum ProtocolId : int
{
	Mesasge = 100,
	
	None = 999,
	Error =1000,
}

public abstract class ProtocolRequest
{
	public ProtocolId ProtocolId { get; set; }

	protected ProtocolRequest(ProtocolId protocolId)
	{
		this.ProtocolId = protocolId;
	}
}

public abstract class ProtocolResponse
{
	public ProtocolId ProtocolId { get; set; }
	public Result Result { get; set; }

	protected ProtocolResponse(ProtocolId protocolId)
	{
		this.ProtocolId = protocolId;
	}
}



public enum Result
{
	Success = 1,
	Fail = 2,
}