using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static class Korean
	{
		// System Singletons
		public static readonly Lazy<RevisedRomanizationSystem> RevisedRomanization = new Lazy<RevisedRomanizationSystem>(() => new RevisedRomanizationSystem());

		private static readonly Lazy<Dictionary<char, Jamo>> HangeulAlphabet = new Lazy<Dictionary<char, Jamo>>(()
			=> new Dictionary<char, Jamo>
			{
				// Vowels
				['ㅏ'] = new Jamo('ㅏ', true, "a"),
				['ㅐ'] = new Jamo('ㅐ', true, "ae"),
				['ㅑ'] = new Jamo('ㅑ', true, "ya"),
				['ㅒ'] = new Jamo('ㅒ', true, "yae"),
				['ㅓ'] = new Jamo('ㅓ', true, "eo"),
				['ㅔ'] = new Jamo('ㅔ', true, "e"),
				['ㅕ'] = new Jamo('ㅕ', true, "yeo"),
				['ㅖ'] = new Jamo('ㅖ', true, "ye"),
				['ㅗ'] = new Jamo('ㅗ', true, "o"),
				['ㅘ'] = new Jamo('ㅘ', true, "wa"),
				['ㅙ'] = new Jamo('ㅙ', true, "wae"),
				['ㅚ'] = new Jamo('ㅚ', true, "oe"),
				['ㅛ'] = new Jamo('ㅛ', true, "yo"),
				['ㅜ'] = new Jamo('ㅜ', true, "u"),
				['ㅝ'] = new Jamo('ㅝ', true, "wo"),
				['ㅞ'] = new Jamo('ㅞ', true, "we"),
				['ㅟ'] = new Jamo('ㅟ', true, "wi"),
				['ㅠ'] = new Jamo('ㅠ', true, "yu"),
				['ㅡ'] = new Jamo('ㅡ', true, "eu"),
				['ㅢ'] = new Jamo('ㅢ', true, "ui"),
				['ㅣ'] = new Jamo('ㅣ', true, "i"),

				// Consonants
				['ㄱ'] = new Jamo('ㄱ', false, "g", "k"),
				['ㄲ'] = new Jamo('ㄲ', false, "kk", "k"),
				['ㄴ'] = new Jamo('ㄴ', false, "n"),
				['ㄷ'] = new Jamo('ㄷ', false, "d", "t"),
				['ㄸ'] = new Jamo('ㄸ', false, "tt", ""),
				['ㄹ'] = new Jamo('ㄹ', false, "r", "l"),
				['ㅁ'] = new Jamo('ㅁ', false, "m"),
				['ㅂ'] = new Jamo('ㅂ', false, "b", "p"),
				['ㅃ'] = new Jamo('ㅃ', false, "pp", ""),
				['ㅅ'] = new Jamo('ㅅ', false, "s", "t"),
				['ㅆ'] = new Jamo('ㅆ', false, "ss", "t"),
				['ㅇ'] = new Jamo('ㅇ', false, "", "ng"),
				['ㅈ'] = new Jamo('ㅈ', false, "j", "t"),
				['ㅉ'] = new Jamo('ㅉ', false, "jj", ""),
				['ㅊ'] = new Jamo('ㅊ', false, "ch", "t"),
				['ㅋ'] = new Jamo('ㅋ', false, "k"),
				['ㅌ'] = new Jamo('ㅌ', false, "t"),
				['ㅍ'] = new Jamo('ㅍ', false, "p"),
				['ㅎ'] = new Jamo('ㅎ', false, "h", "t")
			});

		/// <summary>
		/// A Korean Hangeul letter, representing a consonant or vowel.
		/// </summary>
		private class Jamo
		{
			public char Character;
			public bool Vowel;
			public string InitialRomanization;
			public string FinalRomanization;

			public Jamo(char character, bool vowel, string initialRomanization, string finalRomanization)
			{
				Character = character;
				Vowel = vowel;
				InitialRomanization = initialRomanization;
				FinalRomanization = finalRomanization;
			}

			public Jamo(char character, bool vowel, string romanization)
			{
				Character = character;
				Vowel = vowel;
				InitialRomanization = romanization;
				FinalRomanization = romanization;
			}
		}

		/// <summary>
		/// The Revised Romanization of Korean system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean'>https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean</a>
		/// </summary>
		public sealed class RevisedRomanizationSystem : IRomanizationSystem
		{
			// System-Specific Constants
			private static readonly Dictionary<string, string> GojuonChart = new Dictionary<string, string>();
			private static readonly Dictionary<string, string> YoonChart = new Dictionary<string, string>();

			

			internal RevisedRomanizationSystem()
			{
				#region Romanization Chart
				// Sourced from 



				#endregion
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text)
			{
				return text;
			}
		}
	}
}
