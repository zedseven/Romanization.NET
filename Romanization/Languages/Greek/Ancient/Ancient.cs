// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static partial class Greek
	{
		/// <summary>
		/// The class for romanizing Ancient Greek text.
		/// </summary>
		public static partial class Ancient
		{
			public enum Units
			{
				/// <summary>
				/// A general concept of time, but without a specific unit (days, months, years).
				/// </summary>
				Time,
				/// <summary>
				/// A general concept of weight/value, but without a specific unit. Most weight units from this period
				/// had inherent monetary meaning as whatever their weight was in some valuable material.
				/// </summary>
				Weight,
				/// <summary>
				/// The unit of years.
				/// </summary>
				Years,
				/// <summary>
				/// The ancient Greek currency, also used as a small unit of weight known as a dram.<br />
				/// See <a href='https://en.wikipedia.org/wiki/Greek_drachma'>https://en.wikipedia.org/wiki/Greek_drachma</a> for more info.
				/// </summary>
				Drachma,
				/// <summary>
				/// An old measurement of weight equal to about 26 kilograms, and simultaneously a unit of value of
				/// that weight in silver.<br />
				/// See <a href='https://en.wikipedia.org/wiki/Attic_talent'>https://en.wikipedia.org/wiki/Attic_talent</a> for more info.
				/// </summary>
				Talents,
				/// <summary>
				/// An acnient Greek coin used certain parts of Greece.<br />
				/// See <a href='https://en.wikipedia.org/wiki/Stater'>https://en.wikipedia.org/wiki/Stater</a> for more info.
				/// </summary>
				Staters,
				/// <summary>
				/// An ancient Greek unit of measurement equal to roughly 30 meters. It was also used as a measure of
				/// area, with the value of approximately 30 meters squared. It was additionally used as the Greek
				/// equivalent of an acre.<br />
				/// See <a href='https://en.wikipedia.org/wiki/Plethron'>https://en.wikipedia.org/wiki/Plethron</a> for more info.
				/// </summary>
				Plethra,
				/// <summary>
				/// The plural of mina, which is a unit of weight and currency value.<br />
				/// See <a href='https://en.wiktionary.org/wiki/mina#English'>https://en.wiktionary.org/wiki/mina#English</a> for more info.
				/// </summary>
				Mnas,
			}
		}
	}
}
