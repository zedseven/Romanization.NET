using Romanization.LanguageAgnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
			/// Greek numerals were introduced around 3rd century BCE, replacing Attic numerals.
			/// They are still used today very occassionally.<br />
			/// For more information, visit:
			/// <a href='https://en.wikipedia.org/wiki/Greek_numerals'>https://en.wikipedia.org/wiki/Greek_numerals</a>
			/// </summary>
			public sealed class GreekNumerals : INumeralParsingSystem
			{
				// System-Specific Constants
				private readonly Dictionary<char, int> ValueTable     = new Dictionary<char, int>();
				private const    char   CanonicalDoubleUpperKeraia    = '″';
				private const    char   CanonicalSingleUpperKeraia    = 'ʹ';
				private const    char   CanonicalLowerKeraia          = '͵';
				private const    char   CanonicalOverbar              = '\u0305';
				private readonly char[] UpperKeraiaDoubleReplacements = { 'ʺ', '"' };
				private readonly char[] UpperKeraiaSingleReplacements = { 'ʹ', '\'' };
				private readonly char[] LowerKeraiaReplacements       = { ',' };
				private readonly char[] OverbarChars                  = { '\u0305', '‾' };
				private const    string SigmaTauDigraph               = "ΣΤ";

				private readonly Regex OverbarBoundaryRegex = new Regex("\u0305(?!.\u0305|\u0305)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

				private readonly Regex NumeralDetectionRegex =
					new Regex(
						"(?:(?:[\\w͵∠]\u0305)+(?:\\s*[\\w∠]+[ʹʹ'])*|[\\w͵∠]+[ʹʹ'](?:\\s*[\\w∠]+(?:[″ʺ\\\"]|[ʹʹ'][ʹʹ']))*)",
						RegexOptions.Compiled | RegexOptions.IgnoreCase);

				/// <summary>
				/// Instantiates a copy of the system to process romanizations.
				/// </summary>
				public GreekNumerals()
				{
					#region Romanization Chart

					// Sourced from:
					// https://en.wikipedia.org/wiki/Greek_numerals
					// https://www.foundalis.com/lan/grknum.htm
					// https://www.opoudjis.net/unicode/numerals.html
					// https://web.archive.org/web/20120302122752/http://www.tlg.uci.edu/~opoudjis//unicode/numerals.html

					// Contains both uppercase and lowercase values, since we do case-sensitive comparisons

					//ValueTable['∠'] = (decimal) 1/2;
					ValueTable['Α'] =   1;
					ValueTable['α'] =   1;
					ValueTable['Β'] =   2;
					ValueTable['β'] =   2;
					ValueTable['Γ'] =   3;
					ValueTable['γ'] =   3;
					ValueTable['Δ'] =   4;
					ValueTable['δ'] =   4;
					ValueTable['Ε'] =   5;
					ValueTable['ε'] =   5;
					ValueTable['Ϝ'] =   6; // Digamma
					ValueTable['ϝ'] =   6; // Digamma
					ValueTable['Ϛ'] =   6; // Stigma (sigma-tau)
					ValueTable['ϛ'] =   6; // Stigma (sigma-tau)
					ValueTable['ς'] =   6; // Incorrect, but sometimes used in place of stigma
					ValueTable['ς'] =   6; // Incorrect, but sometimes used in place of stigma
					ValueTable['Ζ'] =   7;
					ValueTable['ζ'] =   7;
					ValueTable['Η'] =   8;
					ValueTable['η'] =   8;
					ValueTable['Θ'] =   9;
					ValueTable['θ'] =   9;
					ValueTable['Ι'] =  10;
					ValueTable['ι'] =  10;
					ValueTable['Κ'] =  20;
					ValueTable['κ'] =  20;
					ValueTable['Λ'] =  30;
					ValueTable['λ'] =  30;
					ValueTable['Μ'] =  40;
					ValueTable['μ'] =  40;
					ValueTable['Ν'] =  50;
					ValueTable['ν'] =  50;
					ValueTable['Ξ'] =  60;
					ValueTable['ξ'] =  60;
					ValueTable['Ο'] =  70;
					ValueTable['ο'] =  70;
					ValueTable['Π'] =  80;
					ValueTable['π'] =  80;
					ValueTable['Ϙ'] =  90;
					ValueTable['ϙ'] =  90;
					ValueTable['Ϟ'] =  90; // Koppa (qoppa)
					ValueTable['ϟ'] =  90; // Koppa (qoppa)
					ValueTable['Ρ'] = 100;
					ValueTable['ρ'] = 100;
					ValueTable['Σ'] = 200;
					ValueTable['σ'] = 200;
					ValueTable['Τ'] = 300;
					ValueTable['τ'] = 300;
					ValueTable['Υ'] = 400;
					ValueTable['υ'] = 400;
					ValueTable['Φ'] = 500;
					ValueTable['φ'] = 500;
					ValueTable['Χ'] = 600;
					ValueTable['χ'] = 600;
					ValueTable['Ψ'] = 700;
					ValueTable['ψ'] = 700;
					ValueTable['Ω'] = 800;
					ValueTable['ω'] = 800;
					ValueTable['Ͳ'] = 900;
					ValueTable['ͳ'] = 900;
					ValueTable['Ϡ'] = 900; // Sampi
					ValueTable['ϡ'] = 900; // Sampi

					#endregion
				}

				/// <summary>
				/// Parses the numeric value of a Greek numeral.
				/// </summary>
				/// <param name="text">The numeral text to parse.</param>
				/// <returns>A numeric value representing the value of <paramref name="text"/>.</returns>
				[Pure]
				public NumeralValue Process(string text)
					=> Process(text, null);

				/// <summary>
				/// Parses the numeric value of a Greek numeral.
				/// </summary>
				/// <param name="text">The numeral text to parse.</param>
				/// <param name="textUsesOverbars">Whether or not the text is known to use overbars. This is for
				/// parsing of larger texts with multiple numerals contained within.</param>
				/// <returns>A numeric value representing the value of <paramref name="text"/>.</returns>
				[Pure]
				internal NumeralValue Process(string text, bool? textUsesOverbars)
				{
					// Clean, normalize, and prepare the text
					// TODO: This could be done much more efficiently, but this system should only ever be used for small strings so the impact shouldn't be very high
					text = text
						// General prep, Unicode normalization
						.LanguageWidePreparation()
						.Trim()
						// Replace common kludges with their intended meanings
						.Replace(SigmaTauDigraph, "Ϛ")
						.Replace('!', CanonicalSingleUpperKeraia)
						// Replace potential alternate forms
						.ReplaceMultipleChars(LowerKeraiaReplacements, CanonicalLowerKeraia)
						.ReplaceMultipleChars(UpperKeraiaSingleReplacements, CanonicalSingleUpperKeraia)
						.ReplaceMultipleChars(UpperKeraiaDoubleReplacements, CanonicalDoubleUpperKeraia)
						.Replace($"{CanonicalSingleUpperKeraia}{CanonicalSingleUpperKeraia}",
							$"{CanonicalDoubleUpperKeraia}")
						.Replace('‾', CanonicalOverbar);

					// Normalize the older usage format with overbars into the modern one using single and double keraiae
					if (!textUsesOverbars.HasValue && text.Contains(CanonicalOverbar) || textUsesOverbars.HasValue && textUsesOverbars.Value)
					{
						// Convert any existing single keraia into doubles
						text = text.Replace(CanonicalSingleUpperKeraia, CanonicalDoubleUpperKeraia);
						// Replace overbars at boundaries between whole and fractional numbers into single keraia
						text = OverbarBoundaryRegex.Replace(text, $"{CanonicalSingleUpperKeraia}");
						// Remove all other overbars
						text = text.Replace($"{CanonicalOverbar}", "");
					}

					decimal totalValue = 0;
					int runningValue = 0;
					int value;
					for (int i = 0; i < text.Length; i++)
					{
						switch (text[i])
						{
							// Thousands (lower keraia)
							case CanonicalLowerKeraia:
								if (i + 1 >= text.Length || !ValueTable.TryGetValue(text[i + 1], out value))
									continue;
								runningValue += 1000 * value;
								i++;
								continue;
							// Single keraia, denotes the end of a whole number in this case
							case CanonicalSingleUpperKeraia:
								totalValue += runningValue;
								runningValue = 0;
								continue;
							// Special half character
							case '∠':
								totalValue += runningValue + (decimal) 1/2;
								runningValue = 0;
								continue;
							// Fractions
							case CanonicalDoubleUpperKeraia when runningValue == 0:
								continue;
							case CanonicalDoubleUpperKeraia:
								totalValue += (decimal) 1 / runningValue;
								runningValue = 0;
								continue;
						}

						if (!ValueTable.TryGetValue(text[i], out value))
							continue;
						runningValue += value;
					}

					totalValue += runningValue;

					return new NumeralValue(totalValue);
				}

				/// <summary>
				/// Processes all Greek numerals in the text.
				/// </summary>
				/// <param name="text">The text to search for numerals.</param>
				/// <param name="numeralProcessor">The function to use to transform the value from <see cref="Process(string)"/>
				/// into a string to put in the text.</param>
				/// <returns>A copy of <paramref name="text"/>, but with all detected Greek numerals processed using
				/// <paramref name="numeralProcessor"/>.</returns>
				public string ProcessNumeralsInText(string text, Func<NumeralValue, string> numeralProcessor)
				{
					text = text.LanguageWidePreparation();
					bool usesOverbars = text.Any(c => OverbarChars.Contains(c));

					StringBuilder result = new StringBuilder(text.Length);
					bool foundMatch = false;
					int startIndex = 0;
					Match match = NumeralDetectionRegex.Match(text);
					while (match.Success)
					{
						foundMatch = true;
						result.Append(text, startIndex, match.Index - startIndex);

						// Handle replacement
						result.Append(numeralProcessor(Process(match.Value, usesOverbars)));

						startIndex = match.Index + match.Length;

						match = match.NextMatch();
					}

					// Append any remaining parts of the original text
					if (startIndex < text.Length)
						result.Append(text, startIndex, text.Length - startIndex);

					return foundMatch ? result.ToString() : text;
				}
			}
		}
	}
}
