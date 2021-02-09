using Romanization.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static partial class Greek
	{
		public static partial class Ancient
		{
			/// <summary>
			/// Attic numerals were used in Ancient Greece roughly from 7th century BCE until they were replaced by
			/// standard Greek numerals in 3rd century BCE.<br />
			/// For more information, visit:<br />
			/// <a href='https://en.wikipedia.org/wiki/Attic_numerals'>https://en.wikipedia.org/wiki/Attic_numerals</a>
			/// for general information<br />
			/// and
			/// <a href='https://www.unicode.org/charts/PDF/U10140.pdf'>https://www.unicode.org/charts/PDF/U10140.pdf</a>
			/// for the full Unicode codepage for many of the (likely-unrenderable) Attic characters
			/// </summary>
			public sealed class AtticNumerals : INumeralParsingSystem<Units>
			{
				// System-Specific Constants
				private readonly Dictionary<string, decimal> ValueTable = new Dictionary<string, decimal>();
				private readonly string[] DrachmaSymbols = { "𐅻", "𐅼", "𐅂", "𐅝", "𐅞", "𐅼", "𐅽", "𐅾", "𐅿", "𐆀" };
				private readonly string[] PlethraSymbols = { "𐅘" };
				private readonly string[] TalentsSymbols = { "𐅺", "𐅈", "𐅉", "𐅊", "𐅋", "𐅌", "𐅍", "𐅎" };
				private readonly string[] StatersSymbols = { "𐅏", "𐅐", "𐅑", "𐅒", "𐅓", "𐅔", "𐅕", "𐅖" };
				private readonly string[] MnasSymbols    = { "𐅳", "𐅗", "𐅴" };
				private readonly string[] YearSymbols    = { "𐅹", "𐆌" };
				private readonly string[] WeightSymbols  = { "𐆎" };
				private readonly string[] TimeSymbols    = { "𐆍" };

				private readonly Regex NumeralDetectionRegex =
					new Regex(
						"(?:[𐆊𐅼𐅀𐆋𐅽𐅁𐅵𐅶𐅾𐅷𐅿𐅸𐆀Ι𐅂𐅘𐅙𐅚𐅛𐅜𐅝𐅞Π𐅈𐅏𐅟𐅳Δ𐅉𐅐𐅗𐅠𐅡𐅢𐅣𐅤𐅥𐅄𐅊𐅑𐅦𐅧𐅨𐅩𐅴Η𐅋𐅒𐅪𐅫𐅅𐅌𐅓𐅬𐅭𐅮𐅯𐅰Χ𐅍𐅔𐅱𐅆𐅎𐅲Μ𐅕𐅇𐅖]\u0305)+",
						RegexOptions.Compiled | RegexOptions.IgnoreCase);

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.
				/// </summary>
				public AtticNumerals()
				{
					#region Romanization Chart

					// Sourced from https://en.wikipedia.org/wiki/Attic_numerals and https://www.unicode.org/charts/PDF/U10140.pdf
					// Comments are if there's a specific unit associated with the symbol, and names in brackets are
					// the region(?) in which the symbol was used

					ValueTable["𐆊"]  =             0;
					ValueTable["𐅼"]  = (decimal) 1/6; // Drachma/Obol
					ValueTable["𐅀"]  = (decimal) 1/4;
					ValueTable["𐆋"]  = (decimal) 1/4;
					ValueTable["𐅽"]  = (decimal) 2/6; // Drachma/Obol
					ValueTable["𐅁"]  = (decimal) 1/2;
					ValueTable["𐅵"]  = (decimal) 1/2;
					ValueTable["𐅶"]  = (decimal) 1/2;
					ValueTable["𐅾"]  = (decimal) 3/6; // Drachma/Obol
					ValueTable["𐅷"] = (decimal) 2/3;
					ValueTable["𐅿"]  = (decimal) 4/6; // Drachma/Obol
					ValueTable["𐅸"]  = (decimal) 3/4;
					ValueTable["𐆀"]  = (decimal) 5/6; // Drachma/Obol
					ValueTable["Ι"]  =             1;
					ValueTable["𐅂"]  =             1; // Drachma
					ValueTable["𐅘"]  =             1; // Plethron
					ValueTable["𐅙"]  =             1; // (Thespian)
					ValueTable["𐅚"]  =             1; // (Hermionian)
					ValueTable["𐅛"]  =             2; // (Epidaurean)
					ValueTable["𐅜"]  =             2; // (Thespian)
					ValueTable["𐅝"]  =             2; // Drachma (Cyrenaic)
					ValueTable["𐅞"]  =             2; // Drachma (Epidaurean)
					ValueTable["Π"]  =             5;
					ValueTable["𐅈"]  =             5; // Talents
					ValueTable["𐅏"]  =             5; // Staters
					ValueTable["𐅟"]  =             5; // (Troezenian)
					ValueTable["𐅳"]  =             5; // Mnas (Delphic)
					ValueTable["Δ"]  =            10;
					ValueTable["𐅉"]  =            10; // Talents
					ValueTable["𐅐"]  =            10; // Staters
					ValueTable["𐅗"]  =            10; // Mnas
					ValueTable["𐅠"]  =            10; // (Troezenian)
					ValueTable["𐅡"]  =            10; // (Troezenian)
					ValueTable["𐅢"]  =            10; // (Hermionian)
					ValueTable["𐅣"]  =            10; // (Messenian)
					ValueTable["𐅤"]  =            10; // (Thespian)
					ValueTable["𐅥"]  =            30; // (Thespian)
					ValueTable["𐅄"]  =            50;
					ValueTable["𐅊"]  =            50; // Talents
					ValueTable["𐅑"]  =            50; // Staters
					ValueTable["𐅦"]  =            50; // (Troezenian)
					ValueTable["𐅧"]  =            50; // (Troezenian)
					ValueTable["𐅨"]  =            50; // (Hermionian)
					ValueTable["𐅩"] =            50; // (Thespian)
					ValueTable["𐅴"]  =            50; // Mnas (Stratian)
					ValueTable["Η"]  =           100;
					ValueTable["𐅋"]  =           100; // Talents
					ValueTable["𐅒"]  =           100; // Staters
					ValueTable["𐅪"] =           100; // (Thespian)
					ValueTable["𐅫"] =           300; // (Thespian)
					ValueTable["𐅅"]  =           500;
					ValueTable["𐅌"]  =           500; // Talents
					ValueTable["𐅓"]  =           500; // Staters
					ValueTable["𐅬"] =           500; // (Epidaurean)
					ValueTable["𐅭"] =           500; // (Troezenian)
					ValueTable["𐅮"] =           500; // (Thespian)
					ValueTable["𐅯"]  =           500; // (Carystian)
					ValueTable["𐅰"]  =           500; // (Naxian)
					ValueTable["Χ"]  =          1000;
					ValueTable["𐅍"]  =          1000; // Talents
					ValueTable["𐅔"]  =          1000; // Staters
					ValueTable["𐅱"]  =          1000; // (Thespian)
					ValueTable["𐅆"]  =          5000;
					ValueTable["𐅎"]  =          5000; // Talents
					ValueTable["𐅲"]  =          5000; // (Thespian)
					ValueTable["Μ"]  =         10000;
					ValueTable["𐅕"]  =         10000; // Staters
					ValueTable["𐅇"]  =         50000;
					ValueTable["𐅖"]  =         50000; // Staters

					#endregion
				}

				/// <summary>
				/// Parses the numeric value of an Attic numeral, and returns the associated unit if possible.
				/// </summary>
				/// <param name="text">The numeral text to parse.</param>
				/// <returns>A numeric value representing the value of <paramref name="text"/>, with a unit if one
				/// could be found.</returns>
				[Pure]
				public NumeralValue<Units> Process(string text)
				{
					text = text
						.LanguageWidePreparation()
						.Replace("\u0305", "");

					Units? unit = text.DetermineResultFromString(
						(Units.Drachma, DrachmaSymbols),
						(Units.Plethra, PlethraSymbols),
						(Units.Talents, TalentsSymbols),
						(Units.Staters, StatersSymbols),
						(Units.Mnas,    MnasSymbols),
						(Units.Years,   YearSymbols),
						(Units.Weight,  WeightSymbols),
						(Units.Time,    TimeSymbols));

					string[] surrogatePairs = text.SplitIntoSurrogatePairs();
					decimal totalValue = 0;
					foreach (string surrogatePair in surrogatePairs)
						if (ValueTable.TryGetValue(surrogatePair, out decimal value))
							totalValue += value;

					return new NumeralValue<Units>(totalValue, unit);
				}

				NumeralValue INumeralParsingSystem.Process(string text)
					=> Process(text).ToUnitlessNumeralValue();

				/// <summary>
				/// Processes all Attic numerals in the text.
				/// </summary>
				/// <param name="text">The text to search for numerals.</param>
				/// <param name="numeralProcessor">The function to use to transform the value from
				/// <see cref="Process(string)"/> into a string to put in the text.</param>
				/// <returns>A copy of <paramref name="text"/>, but with all detected Attic numerals processed using
				/// <paramref name="numeralProcessor"/>.</returns>
				/// <remarks>Attic numeral support is somewhat contrived, as there's no real way to distinguish them from
				/// other Greek text and so in-text detection works based on presence of Unicode overbar characters.
				/// This isn't realistically something that would be seen in practice, and the special Attic characters
				/// often don't even work with the overbar combining character. Perhaps a better solution is possible,
				/// but overbars seem to stand the highest chance of having actually been in use, so that's what it
				/// looks for.<br />
				/// If this particular function is something you need, open an issue and provide an example of what
				/// you need to romanize.<br />
				/// For general parsing of Attic numerals, check out <see cref="AtticNumerals.Process"/>.</remarks>
				public string ProcessNumeralsInText(string text, Func<NumeralValue<Units>, string> numeralProcessor)
				{
					text = text.LanguageWidePreparation();

					StringBuilder result = new StringBuilder(text.Length);
					bool foundMatch = false;
					int startIndex = 0;
					Match match = NumeralDetectionRegex.Match(text);
					while (match.Success)
					{
						foundMatch = true;
						result.Append(text, startIndex, match.Index - startIndex);

						// Handle replacement
						result.Append(numeralProcessor(Process(match.Value)));

						startIndex = match.Index + match.Length;

						match = match.NextMatch();
					}

					// Append any remaining parts of the original text
					if (startIndex < text.Length)
						result.Append(text, startIndex, text.Length - startIndex);

					return foundMatch ? result.ToString() : text;
				}

				string INumeralParsingSystem.ProcessNumeralsInText(string text, Func<NumeralValue, string> numeralProcessor)
					=> ProcessNumeralsInText(text, value => numeralProcessor(value.ToUnitlessNumeralValue()));
			}
		}
	}
}
