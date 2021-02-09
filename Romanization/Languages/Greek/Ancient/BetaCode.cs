using Romanization.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

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
			/// The Beta Code Greek romanization system.<br />
			/// For more information, visit:
			/// <a href='https://en.wikipedia.org/wiki/Beta_Code'>https://en.wikipedia.org/wiki/Beta_Code</a>
			/// </summary>
			public sealed class BetaCode : IMultiInCultureSystem
			{
				/// <inheritdoc />
				public SystemType Type => SystemType.Transliteration;

				/// <inheritdoc />
				public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("el-GR");

				// System-Specific Constants
				private readonly Dictionary<string, string> DiacriticsTable		= new();
				private readonly Dictionary<string, string> PunctuationTable	   = new();
				private readonly Dictionary<string, string> CommonReplacementTable = new();
				//private readonly Dictionary<string, string> FullReplacementTable   = new Dictionary<string, string>();

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.
				/// </summary>
				public BetaCode()
				{
					#region Common Replacement Chart

					// Sourced from https://en.wikipedia.org/wiki/Beta_Code and http://stephanus.tlg.uci.edu/encoding/BCM.pdf

					// Diacritics
					DiacriticsTable["\u0313"] =  ")"; // Smooth breathing
					DiacriticsTable["\u0314"] =  "("; // Rough breathing
					DiacriticsTable["\u0301"] =  "/"; // Acute accent
					DiacriticsTable["\u0342"] =  "="; // Circumflex accent
					DiacriticsTable["\u0300"] = "\\"; // Grave accent
					DiacriticsTable["\u0308"] =  "+"; // Diaeresis
					DiacriticsTable["\u0345"] =  "|"; // Iota subscript
					DiacriticsTable["\u0304"] =  "&"; // Macron
					DiacriticsTable["\u0306"] =  "'"; // Breve

					// Punctuation
					//PunctuationTable["."] =	   "."; // Low dot
					//PunctuationTable[","] =	   ","; // Comma
					PunctuationTable["\u0387"] =	":"; // Mid dot
					//PunctuationTable[";"] =	   ";"; // Question mark
					PunctuationTable["\u037E"] =	";"; // Distinct from above but visually the same
					PunctuationTable["\u02D9"] = "#762"; // High dot (in ancient Greek this acted as a full stop)
					PunctuationTable["\u205A"] =  "#52"; // In ancient texts the Greek two-dot punctuation mark (looks like a colon) served as the full stop
					//PunctuationTable["'"] =	   "'"; // Apostrophe
					PunctuationTable["’"] =		 "'"; // Distinct from above but visually the same
					//PunctuationTable["-"] =	   "-"; // Hyphen
					PunctuationTable["‐"] =		 "-"; // Distinct from above but visually the same
					PunctuationTable["—"] =		 "_"; // Dash
					PunctuationTable["ʹ"] =		 "#"; // Keraia
					PunctuationTable["ʹ"] =		 "#"; // Distinct from above but visually the same
					PunctuationTable["ʺ"] =		 "#"; // Double Keraia

					// Main characters (2021)
					CommonReplacementTable["Α"] =  "*A";
					CommonReplacementTable["α"] =   "A";
					CommonReplacementTable["Β"] =  "*B";
					CommonReplacementTable["β"] =   "B";
					CommonReplacementTable["Γ"] =  "*G";
					CommonReplacementTable["γ"] =   "G";
					CommonReplacementTable["Δ"] =  "*D";
					CommonReplacementTable["δ"] =   "D";
					CommonReplacementTable["Ε"] =  "*E";
					CommonReplacementTable["ε"] =   "E";
					CommonReplacementTable["ϝ"] =  "*V"; // Digamma
					CommonReplacementTable["ͷ"] =   "V"; // Digamma
					CommonReplacementTable["Ζ"] =  "*Z";
					CommonReplacementTable["ζ"] =   "Z";
					CommonReplacementTable["Η"] =  "*H";
					CommonReplacementTable["η"] =   "H";
					CommonReplacementTable["Θ"] =  "*Q";
					CommonReplacementTable["θ"] =   "Q";
					CommonReplacementTable["Ι"] =  "*I";
					CommonReplacementTable["ι"] =   "I";
					CommonReplacementTable["Κ"] =  "*K";
					CommonReplacementTable["κ"] =   "K";
					CommonReplacementTable["Λ"] =  "*L";
					CommonReplacementTable["λ"] =   "L";
					CommonReplacementTable["Μ"] =  "*M";
					CommonReplacementTable["μ"] =   "M";
					CommonReplacementTable["Ν"] =  "*N";
					CommonReplacementTable["ν"] =   "N";
					CommonReplacementTable["Ξ"] =  "*C";
					CommonReplacementTable["ξ"] =   "C";
					CommonReplacementTable["Ο"] =  "*O";
					CommonReplacementTable["ο"] =   "O";
					CommonReplacementTable["Π"] =  "*P";
					CommonReplacementTable["π"] =   "P";
					CommonReplacementTable["Ρ"] =  "*R";
					CommonReplacementTable["ρ"] =   "R";
					CommonReplacementTable["Σ"] =  "*S";
					CommonReplacementTable["σ"] =  "S1";
					CommonReplacementTable["ς"] =  "S2";
					CommonReplacementTable["Ϲ"] = "*S3"; // Lunate sigma
					CommonReplacementTable["ϲ"] =  "S3"; // Lunate sigma
					CommonReplacementTable["Τ"] =  "*T";
					CommonReplacementTable["τ"] =   "T";
					CommonReplacementTable["Υ"] =  "*U";
					CommonReplacementTable["υ"] =   "U";
					CommonReplacementTable["Φ"] =  "*F";
					CommonReplacementTable["φ"] =   "F";
					CommonReplacementTable["Χ"] =  "*X";
					CommonReplacementTable["χ"] =   "X";
					CommonReplacementTable["Ψ"] =  "*Y";
					CommonReplacementTable["ψ"] =   "Y";
					CommonReplacementTable["Ω"] =  "*W";
					CommonReplacementTable["ω"] =   "W";

					// Uncommon letters
					CommonReplacementTable["Ϛ"] =   "*#2"; // Stigma
					CommonReplacementTable["ϛ"] =	"#2"; // Stigma
					CommonReplacementTable["Ϙ"] =   "*#3"; // Koppa
					CommonReplacementTable["ϙ"] =	"#3"; // Koppa
					CommonReplacementTable["Ϟ"] =   "*#3"; // Koppa
					CommonReplacementTable["ϟ"] =	"#3"; // Koppa
					CommonReplacementTable["Ϡ"] =   "*#5"; // Sampi
					CommonReplacementTable["ϡ"] =	"#5"; // Sampi
					CommonReplacementTable["Ͳ"] =   "*#5"; // Sampi
					CommonReplacementTable["ͳ"] =	"#5"; // Sampi
					CommonReplacementTable["Ϻ"] = "*#711"; // San
					CommonReplacementTable["ϻ"] =  "#711"; // San
					CommonReplacementTable["Ϳ"] = "*#401"; // Yot
					CommonReplacementTable["ϳ"] =  "#401"; // Yot

					#endregion
				}

				/// <summary>
				/// Performs Beta Code Greek romanization on the given text.<br />
				/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the
				/// country code is <c>el</c>.
				/// </summary>
				/// <param name="text">The text to romanize.</param>
				/// <param name="nativeCulture">The culture to romanize from.</param>
				/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
				/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
				/// language/region.</exception>
				[Pure]
				public string Process(string text, CultureInfo nativeCulture)
				{
					if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "el")
						throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));
					return CulturalOperations.RunWithCulture(nativeCulture, () => text
						// General preparation, normalization
						.LanguageWidePreparation()
						// Do common replacements
						.ReplaceFromChart(DiacriticsTable, StringComparison.Ordinal)
						.ReplaceFromChart(PunctuationTable, StringComparison.Ordinal)
						.ReplaceFromChart(CommonReplacementTable));
				}

				/// <summary>
				/// Performs Beta Code Greek romanization on the given text.
				/// </summary>
				/// <param name="text">The text to romanize.</param>
				/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
				[Pure]
				public string Process(string text)
					=> Process(text, DefaultCulture);
			}
		}
	}
}
