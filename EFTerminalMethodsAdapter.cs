namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Entity Framework terminal methods adapter.
	/// </summary>
	/// <remarks>
	/// Provider-specific asynchronous overrides will be added here as the terminal surface is implemented for Entity Framework.
	/// The inherited defaults remain correct synchronous fallbacks.
	/// </remarks>
	public class EFTerminalMethodsAdapter : DefaultTerminalMethodsAdapter
	{
	}
}
