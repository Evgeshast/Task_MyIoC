

namespace IoCSample
{
	public interface ICustomerDAL
	{
	}

	[Export(typeof(ICustomerDAL))]
	public class CustomerDAL : ICustomerDAL
	{ }
}