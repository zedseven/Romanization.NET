using Romanization.Internal;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;

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
			/// The Beta Code Greek transliteration system.<br />
			/// For more information, visit:
			/// <a href='https://en.wikipedia.org/wiki/Beta_Code'>https://en.wikipedia.org/wiki/Beta_Code</a>
			/// </summary>
			public sealed class BetaCode : IRomanizationSystem
			{
				/// <inheritdoc />
				public SystemType Type => SystemType.Transliteration; // To the extreme

				/// <summary>
				/// Whether or not to support the entire TLG Beta Code replacement set. Note that this contains
				/// hundreds of characters, most of which you will never ever need. If you're not sure, don't use full
				/// replacements.<br />
				/// For the full replacement specification, see:
				/// <a href='http://stephanus.tlg.uci.edu/encoding/BCM.pdf'>http://stephanus.tlg.uci.edu/encoding/BCM.pdf</a>
				/// </summary>
				public readonly bool FullReplacements;

				// System-Specific Constants
				private const string FullSetFileName = "BetaCodeGreek.csv";
				private readonly ReplacementChart ReplacementTable;

				private readonly RepeatedCombiningCharsSub? OverlineSub;
				private readonly RepeatedCombiningCharsSub? UnderlineSub;
				private readonly RepeatedCombiningCharsSub? InvertedBreveSub;
				private readonly RepeatedCombiningCharsSub? BelowBreveSub;
				private readonly RepeatedCombiningCharsSub? BreveSub;
				private readonly RepeatedCombiningCharsSub? DoubleUnderlineSub;
				private readonly EncapsulatingCharsSub?     DoubleSolidusBracketsSub;
				private readonly EncapsulatingCharsSub?     SingleSolidusBracketsSub;

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.
				/// </summary>
				/// <param name="fullReplacements">Whether or not to support the entire TLG Beta Code replacement set.
				/// Note that this contains thousands of characters, most of which you will never ever need. If you're
				/// not sure, don't use full replacements.</param>
				public BetaCode(bool fullReplacements = false)
				{
					FullReplacements = fullReplacements;

					#region Replacement Chart

					// Sourced from https://en.wikipedia.org/wiki/Beta_Code
					// and http://stephanus.tlg.uci.edu/encoding/BCM.pdf

					if (!FullReplacements)
					{
						ReplacementTable = new ReplacementChart(StringComparer.Ordinal)
						{
							// Diacritics
							{"\u0313",  ")"}, // Smooth breathing
							{"\u0314",  "("}, // Rough breathing
							{"\u0301",  "/"}, // Acute accent
							{"\u0342",  "="}, // Circumflex accent
							{"\u0300", "\\"}, // Grave accent
							{"\u0308",  "+"}, // Diaeresis
							{"\u0345",  "|"}, // Iota subscript
							{"\u0304",  "&"}, // Macron
							{"\u0306",  "'"}, // Breve

							// Punctuation
							{"\u0387",   ":"}, // Mid dot
							{"\u037E",   ";"}, // Question mark
							{"\u02D9", "#72"}, // High dot (in ancient Greek this acted as a full stop)
							{"\u205A", "#73"}, // In ancient texts the Greek two-dot punctuation mark (looks like a colon) served as the full stop
							{"’",        "'"}, // Apostrophe
							{"‐",        "-"}, // Hyphen
							{"—",        "_"}, // Dash
							{"ʹ",        "#"}, // Keraia
							{"ʹ",        "#"}, // Distinct from above but visually the same
							{"ʺ",        "#"}, // Double Keraia

							// Main characters (2021)
							{"Α",    "*A"},
							{"α",     "A"},
							{"Β",    "*B"},
							{"β",     "B"},
							{"Γ",    "*G"},
							{"γ",     "G"},
							{"Δ",    "*D"},
							{"δ",     "D"},
							{"Ε",    "*E"},
							{"ε",     "E"},
							{"ϝ",    "*V"}, // Digamma
							{"ͷ",     "V"}, // Digamma
							{"Ζ",    "*Z"},
							{"ζ",     "Z"},
							{"Η",    "*H"},
							{"η",     "H"},
							{"Θ",    "*Q"},
							{"θ",     "Q"},
							{"Ι",    "*I"},
							{"ι",     "I"},
							{"Κ",    "*K"},
							{"κ",     "K"},
							{"Λ",    "*L"},
							{"λ",     "L"},
							{"Μ",    "*M"},
							{"μ",     "M"},
							{"Ν",    "*N"},
							{"ν",     "N"},
							{"Ξ",    "*C"},
							{"ξ",     "C"},
							{"Ο",    "*O"},
							{"ο",     "O"},
							{"Π",    "*P"},
							{"π",     "P"},
							{"Ρ",    "*R"},
							{"ρ",     "R"},
							{"Σ",    "*S"},
							{"σ",    "S1"},
							{"ς",    "S2"},
							{"Ϲ",   "*S3"}, // Lunate sigma
							{"ϲ",    "S3"}, // Lunate sigma
							{"Τ",    "*T"},
							{"τ",     "T"},
							{"Υ",    "*U"},
							{"υ",     "U"},
							{"Φ",    "*F"},
							{"φ",     "F"},
							{"Χ",    "*X"},
							{"χ",     "X"},
							{"Ψ",    "*Y"},
							{"ψ",     "Y"},
							{"Ω",    "*W"},
							{"ω",     "W"},

							// Uncommon letters
							{"Ϛ",   "*#2"}, // Stigma
							{"ϛ",    "#2"}, // Stigma
							{"Ϙ",   "*#3"}, // Koppa
							{"ϙ",    "#3"}, // Koppa
							{"Ϟ",   "*#3"}, // Koppa
							{"ϟ",    "#3"}, // Koppa
							{"Ϡ",   "*#5"}, // Sampi
							{"ϡ",    "#5"}, // Sampi
							{"Ͳ",   "*#5"}, // Sampi
							{"ͳ",    "#5"}, // Sampi
							{"Ϻ", "*#711"}, // San
							{"ϻ",  "#711"}, // San
							{"Ϳ", "*#401"}, // Yot
							{"ϳ",  "#401"}  // Yot
						};
					}
					else
					{
						// Exceptional replacements
						OverlineSub              = new RepeatedCombiningCharsSub('\u0305', "<",  ">");
						UnderlineSub             = new RepeatedCombiningCharsSub('\u0332', "<1", ">1");
						InvertedBreveSub         = new RepeatedCombiningCharsSub('\u0361', "<3", ">3", 3);
						BelowBreveSub            = new RepeatedCombiningCharsSub('\u035C', "<4", ">4", 3);
						BreveSub                 = new RepeatedCombiningCharsSub('\u035D', "<5", ">5", 3);
						DoubleUnderlineSub       = new RepeatedCombiningCharsSub('\u0333', "<8", ">8");
						DoubleSolidusBracketsSub = new EncapsulatingCharsSub("//", "//", "[81", "]81");
						SingleSolidusBracketsSub = new EncapsulatingCharsSub("/",  "/",  "[80", "]80");

						// All other replacements
						ReplacementTable = new ReplacementChart(StringComparer.Ordinal);
						CsvLoader.LoadCharacterMap(FullSetFileName, ReplacementTable,
							k => string.Concat(k
								.Split('+')
								.Select(c => char.ConvertFromUtf32(int.Parse(c, NumberStyles.HexNumber)))),
							v => v);
					}

					#endregion
				}

				private static string ReplaceEditorialBrackets(string text)
				{
					StringBuilder result = new(text.Length + 4);
					bool? openBracketType = null; // false for deletion bracket, true for dittography bracket
					foreach (char c in text)
					{
						switch (c)
						{
							case '├' when !openBracketType.HasValue:
								openBracketType = false;
								result.Append("[82");
								break;
							case '├' when openBracketType.Value:
								openBracketType = null;
								result.Append("]83");
								break;
							case '┤' when !openBracketType.HasValue:
								openBracketType = true;
								result.Append("[83");
								break;
							case '┤' when !openBracketType.Value:
								openBracketType = null;
								result.Append("]82");
								break;
							default:
								result.Append(c);
								break;
						}
					}
					return result.ToString();
				}

				/// <summary>
				/// Performs Beta Code Greek romanization on the given text.
				/// </summary>
				/// <param name="text">The text to romanize.</param>
				/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
				[Pure]
				public string Process(string text)
					=> text
						// General preparation, normalization
						.LanguageWidePreparation()
						// If doing full replacements, there are some that require special treatment
						.ExecuteIf(FullReplacements, t => t
							.ReplaceMany(
								OverlineSub,
								UnderlineSub,
								InvertedBreveSub,
								BelowBreveSub,
								BreveSub,
								DoubleUnderlineSub,
								DoubleSolidusBracketsSub,
								SingleSolidusBracketsSub))
						.Execute(ReplaceEditorialBrackets)
						// All other replacements
						.ReplaceFromChart(ReplacementTable);
			}
		}
	}
}
