
namespace Infrastructure
{
	public interface IRedisConnectionFactory
	{
		ConnectionMultiplexer Connection();
	}
}
