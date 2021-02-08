namespace Romanization
{
	/// <summary>
	/// The type of output numeral parsed numbers should be put into.<br />
	/// For instance, Greek numerals are traditionally romanized as Roman numerals except for when in
	/// official/government documents.
	/// </summary>
	public enum OutputNumeralType
	{
		/// <summary>
		/// The numeral system of much of the world.<br />
		/// Example: <c>267.5</c>
		/// </summary>
		Arabic,
		/// <summary>
		/// The numeral system of Rome, still often used today for names etc.<br />
		/// Example: <c>CCLXVIIS</c>
		/// </summary>
		Roman
	}
}
