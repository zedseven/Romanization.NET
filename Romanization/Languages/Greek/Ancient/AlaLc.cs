using Romanization.Internal;
using System;
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
			/// The ALA-LC (American Library Association and Library of Congress) Greek romanization system.<br />
			/// Has two separate modes - one for *very* old Greek
			/// (<a href='https://en.wikipedia.org/wiki/Attic_numerals'>Attic numerals</a>, obelistic full-stops (⁚)),
			/// and one for more recent old Greek
			/// (<a href='https://en.wikipedia.org/wiki/Greek_numerals'>Greek numerals</a>, modern-ish punctuation). If
			/// you don't know the difference, use the more recent version.<br />
			/// For more information, visit:<br />
			/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Greek'>https://en.wikipedia.org/wiki/Romanization_of_Greek</a><br />
			/// and<br />
			/// <a href='https://www.loc.gov/catdir/cpso/romanization/greek.pdf'>https://www.loc.gov/catdir/cpso/romanization/greek.pdf</a>
			/// </summary>
			/// <remarks>Attic numeral support is somewhat contrived. Check out
			/// <see cref="AtticNumerals.ProcessNumeralsInText"/> for more information.</remarks>
			public sealed class AlaLc : IMultiInOutCultureSystem
			{
				/// <inheritdoc />
				public SystemType Type => SystemType.Transliteration;

				/// <summary>
				/// The default culture this system will use.
				/// </summary>
				public const string DefaultNativeCulture = "el-GR";

				/// <inheritdoc />
				public CultureInfo NativeCulture { get; }

				/// <inheritdoc />
				public CultureInfo RomanizedCulture { get; }

				/// <inheritdoc cref="OutputNumeralType"/>
				public readonly OutputNumeralType OutputNumeralType;

				/// <summary>
				/// Whether or not this system should parse as if the text is very old. In very old Greek, a different
				/// punctuation system was used and Attic numerals were used instead of newer Greek numerals.<br />
				/// While the use of this should largely depend on the text you intend to use it with, a decent
				/// rule-of-thumb is that if Attic numerals are a possibility, this should be true.
				/// </summary>
				public readonly bool VeryOld;

				// Sub-systems
				private readonly INumeralParsingSystem NumeralsSystem;

				// System-Specific Constants
				private readonly ReplacementChart RomanizationTable;

				private readonly CaseAwareSub RhoAspiratedSub = new("(?:\\bρ|(?<=ρ)ρ(?!\\b|ρ))", "rh");

				private readonly CaseAwareSub RoughBreathingVowelSub =
					new($"(?<=[{GreekAllVowels}]|{GreekVowelDiphthongs})\u0314", "h", true);

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.
				/// </summary>
				/// <param name="outputNumeralType">What kind of numeral to romanize to.<br />
				/// Greek numerals are traditionally romanized to Roman numerals, except for when in
				/// official/government documents.</param>
				/// <param name="veryOld">Whether or not this system should parse as if the text is very old. In very
				/// old a different punctuation system was used and Attic numerals were used instead.</param>
				public AlaLc(OutputNumeralType outputNumeralType = OutputNumeralType.Roman, bool veryOld = false) :
					this(CultureInfo.GetCultureInfo(DefaultNativeCulture), CultureInfo.CurrentCulture, outputNumeralType,
						veryOld) {}

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.<br />
				/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the
				/// country code is <c>el</c>.
				/// </summary>
				/// <param name="nativeCulture">The culture to romanize from.</param>
				/// <param name="outputNumeralType">What kind of numeral to romanize to.<br />
				/// Greek numerals are traditionally romanized to Roman numerals, except for when in
				/// official/government documents.</param>
				/// <param name="veryOld">Whether or not this system should parse as if the text is very old. In very
				/// old Greek, a different punctuation system was used and Attic numerals were used instead.</param>
				/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
				/// language/region.</exception>
				public AlaLc(CultureInfo nativeCulture, OutputNumeralType outputNumeralType = OutputNumeralType.Roman,
					bool veryOld = false) :
					this(nativeCulture, CultureInfo.CurrentCulture, outputNumeralType, veryOld) {}

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.<br />
				/// Supports providing a specific <paramref name="nativeCulture"/> (as long as the
				/// country code is <c>el</c>) to romanize from, and a
				/// <paramref name="romanizedCulture"/> to romanize to.
				/// </summary>
				/// <param name="nativeCulture">The culture to romanize from.</param>
				/// <param name="romanizedCulture">The culture to romanize to.</param>
				/// <param name="outputNumeralType">What kind of numeral to romanize to.<br />
				/// Greek numerals are traditionally romanized to Roman numerals, except for when in
				/// official/government documents.</param>
				/// <param name="veryOld">Whether or not this system should parse as if the text is very old. In very
				/// old Greek, a different punctuation system was used and Attic numerals were used instead.</param>
				/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
				/// language/region.</exception>
				public AlaLc(CultureInfo nativeCulture, CultureInfo romanizedCulture,
					OutputNumeralType outputNumeralType = OutputNumeralType.Roman, bool veryOld = false)
				{
					if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "el")
						throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

					NativeCulture     = nativeCulture;
					RomanizedCulture  = romanizedCulture;
					OutputNumeralType = outputNumeralType;
					VeryOld           = veryOld;
					NumeralsSystem    = !VeryOld ? new GreekNumerals() : new AtticNumerals();

					#region Romanization Chart

					// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Greek

					// Main characters (2021)
					RomanizationTable =
						new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
						{
							{"α",        "a"},
							{"β",        "b"},
							{"γ",        "g"}, // has special provisions
							{"δ",        "d"},
							{"ε",        "e"},
							{"ζ",        "z"},
							{"η",        "ē"},
							{"θ",       "th"},
							{"ι",        "i"},
							{"κ",        "k"},
							{"λ",        "l"},
							{"μ",        "m"},
							{"ν",        "n"},
							{"ξ",        "x"},
							{"ο",        "o"},
							{"π",        "p"},
							{"ρ\u0314", "rh"},
							{"ρ",        "r"}, // has special provisions
							{"σ",        "s"},
							//{"ς",        "s"},
							{"τ",        "t"},
							{"υ",        "y"}, // has special provisions
							{"φ",       "ph"},
							{"χ",       "ch"},
							{"ψ",       "ps"},
							{"ω",        "ō"},
							{"ϝ",        "w"}, // Digamma
							{"ͷ",        "w"}, // Pamphylian Digamma
							{"ϙ",        "ḳ"}, // Koppa
							{"ϟ",        "ḳ"}, // Koppa
							{"ϡ",         ""}, // Sampi
							{"ͳ",         ""}, // Sampi
							{"ϻ",         ""}, // San
							//{"Ϲ",        "s"}, // Lunate sigma
							//{"ϲ",        "s"}, // Lunate sigma
							//{"ϳ",         ""}  // Yot - https://github.com/dotnet/runtime/issues/48321

							// Diphthongs
							{"αι", "ae"},
							{"ει", "ei"},
							{"οι", "oi"},
							{"ου", "ou"},
							{"υι", "ui"},
							{"αυ", "au"},
							{"ευ", "eu"},
							{"ηυ", "eu"},
							//{"ωυ", "ōu"}

							// Gamma velar stop combinations
							{"γγ",  "ng"},
							{"γκ",  "nk"},
							{"γξ",  "nx"},
							{"γχ", "nch"}
						};

					// Punctuation
					if (VeryOld)
					{
						RomanizationTable.Add(".",      ","); // Low dot (in ancient Greek this acted as a short breath, or comma)
						RomanizationTable.Add("·",      ";"); // Mid dot (in ancient Greek this acted as a long breath, or semicolon)
						//RomanizationTable.Add("\u0387", ";"); // Distinct from above but visually the same
						RomanizationTable.Add("\u02D9", "."); // High dot (in ancient Greek this acted as a full stop)
						RomanizationTable.Add("\u205A", "."); // In ancient texts the Greek two-dot punctuation mark (looks like a colon) served as the full stop
						RomanizationTable.Add("\u203F", "-"); // Papyrological hyphen
						RomanizationTable.Add("\u035C", "-"); // Papyrological hyphen
					}
					RomanizationTable.Add(";",      "?");
					//RomanizationTable.Add("\u037E", "?"); // Distinct from above but visually the same
					RomanizationTable.Add("’",      "h"); // Sometimes used as an aspiration mark

					#endregion
				}

				/// <summary>
				/// Performs ALA-LC Greek romanization on the given text.
				/// </summary>
				/// <param name="text">The text to romanize.</param>
				/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
				[Pure]
				public string Process(string text)
					=> CulturalOperations.RunWithCulture(NativeCulture, () =>
						text
							// General preparation, normalization
							.LanguageWidePreparation()
							// Remove diacritics that this system ignores
							.WithoutChars("\u0300\u0340" + // Grave accent
							              "\u0301\u0341" + // Acute accent
							              "\u0313\u0343" + // Smooth breathing/koronis
							              "\u0303\u0342" + // Tilde
							              "\u0311" +       // Inverted breve
							              "\u0345")        // Iota subscript
							// Convert numerals
							.ExecuteIfElse(OutputNumeralType == OutputNumeralType.Roman,
								t => NumeralsSystem.ProcessNumeralsInText(t, value => value.Value.ToRomanNumerals()),
								t => NumeralsSystem.ProcessNumeralsInText(t,
									value => value.Value.ToArabicNumerals(RomanizedCulture)))
							// Do special provisions
							.ReplaceMany(RhoAspiratedSub, RoughBreathingVowelSub)
							// Do character replacement
							.ReplaceFromChartCaseAware(RomanizationTable));
			}
		}
	}
}
