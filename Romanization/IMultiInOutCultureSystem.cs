namespace Romanization
{
	/// <summary>
	/// An extended version of <see cref="IMultiInCultureSystem"/> that supports providing a culture to
	/// romanize to, as well as from. the reason this is separate from <see cref="IMultiInCultureSystem"/>
	/// is because many systems don't have to do anything culture-specific when romanizing to a culture, but some do.
	/// </summary>
	public interface IMultiInOutCultureSystem : IMultiInCultureSystem, IMultiOutCultureSystem { }
}
