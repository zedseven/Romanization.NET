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
	public static partial class Japanese
	{
		/// <summary>
		/// The Modified Hepburn Japanese romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Hepburn_romanization'>https://en.wikipedia.org/wiki/Hepburn_romanization</a>
		/// </summary>
		public sealed class ModifiedHepburn : IRomanizationSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.PhonemicTranscription;

			private const string DefaultNativeCulture = "ja-JP";
			private readonly CultureInfo CultureNative = CultureInfo.GetCultureInfo(DefaultNativeCulture);

			// System-Specific Constants
			private readonly ReplacementChart GojuonChart;
			private readonly ReplacementChart YoonChart;

			private readonly CharSub LongASub = new($"a{Choonpu}", Constants.MacronA, false);
			private readonly CharSub LongESub = new($"e{Choonpu}", Constants.MacronE, false);
			private readonly CharSub LongISub = new($"i{Choonpu}", Constants.MacronI, false);
			private readonly CharSub LongOSub = new($"o{Choonpu}", Constants.MacronO, false);
			private readonly CharSub LongUSub = new($"u{Choonpu}", Constants.MacronU, false);

			private readonly CharSub SyllabicNVowelsSub =
				new($"[{SyllabicNHiragana}{SyllabicNKatakana}]([{Constants.LatinVowels}])", "n'${1}");
			private readonly CharSub SyllabicNConsonantsSub =
				new($"[{SyllabicNHiragana}{SyllabicNKatakana}]([{Constants.LatinConsonants}])", "n${1}");

			private readonly CharSub SokuonGeneralCaseSub =
				new($"[{SokuonHiragana}{SokuonKatakana}]([{Constants.LatinConsonants}])", "${1}${1}", false);
			private readonly CharSub SokuonChCaseSub = new($"[{SokuonHiragana}{SokuonKatakana}]ch", "tch", false);

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public ModifiedHepburn()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Hepburn_romanization#Romanization_charts

				StringComparer comparer = StringComparer.Create(CultureNative, CompareOptions.IgnoreKanaType);

				// Gojūon
				GojuonChart = new ReplacementChart(comparer)
				{
					// Column 1
					{"あ",   "a"},
					{"か",  "ka"},
					{"さ",  "sa"},
					{"た",  "ta"},
					{"な",  "na"},
					{"は",  "ha"},
					{"ま",  "ma"},
					{"や",  "ya"},
					{"ら",  "ra"},
					{"わ",  "wa"},
					{"が",  "ga"},
					{"ざ",  "za"},
					{"だ",  "da"},
					{"ば",  "ba"},
					{"ぱ",  "pa"},
					// Column 2
					{"い",   "i"},
					{"き",  "ki"},
					{"し", "shi"},
					{"ち", "chi"},
					{"に",  "ni"},
					{"ひ",  "hi"},
					{"み",  "mi"},
					{"り",  "ri"},
					{"ゐ",   "i"}, // historical character
					{"ぎ",  "gi"},
					{"じ",  "ji"},
					{"ぢ",  "ji"},
					{"び",  "bi"},
					{"ぴ",  "pi"},
					// Column 3
					{"う",   "u"},
					{"く",  "ku"},
					{"す",  "su"},
					{"つ", "tsu"},
					{"ぬ",  "nu"},
					{"ふ",  "fu"},
					{"む",  "mu"},
					{"ゆ",  "yu"},
					{"る",  "ru"},
					{"ぐ",  "gu"},
					{"ず",  "zu"},
					{"づ",  "zu"},
					{"ぶ",  "bu"},
					{"ぷ",  "pu"},
					// Column 4
					{"え",   "e"},
					{"け",  "ke"},
					{"せ",  "se"},
					{"て",  "te"},
					{"ね",  "ne"},
					{"へ",  "he"},
					{"め",  "me"},
					{"れ",  "re"},
					{"ゑ",   "e"}, // historical character
					{"げ",  "ge"},
					{"ぜ",  "ze"},
					{"で",  "de"},
					{"べ",  "be"},
					{"ぺ",  "pe"},
					// Column 5
					{"お",   "o"},
					{"こ",  "ko"},
					{"そ",  "so"},
					{"と",  "to"},
					{"の",  "no"},
					{"ほ",  "ho"},
					{"も",  "mo"},
					{"よ",  "yo"},
					{"ろ",  "ro"},
					{"を",   "o"}, // rare particle
					//{"ん",   "n"},
					{"ご",  "go"},
					{"ぞ",  "zo"},
					{"ど",  "do"},
					{"ぼ",  "bo"},
					{"ぽ",  "po"}
				};

				// Yōon
				YoonChart = new ReplacementChart(comparer)
				{
					// Column 1
					{"きゃ", "kya"},
					{"しゃ", "sha"},
					{"ちゃ", "cha"},
					{"にゃ", "nya"},
					{"ひゃ", "hya"},
					{"みゃ", "mya"},
					{"りゃ", "rya"},
					{"ぎゃ", "gya"},
					{"じゃ",  "ja"},
					{"ぢゃ",  "ja"},
					{"びゃ", "bya"},
					{"ぴゃ", "pya"},
					// Column 2
					{"きゅ", "kyu"},
					{"しゅ", "shu"},
					{"ちゅ", "chu"},
					{"にゅ", "nyu"},
					{"ひゅ", "hyu"},
					{"みゅ", "myu"},
					{"りゅ", "ryu"},
					{"ぎゅ", "gyu"},
					{"じゅ",  "ju"},
					{"ぢゅ",  "ju"},
					{"びゅ", "byu"},
					{"ぴゅ", "pyu"},
					// Column 3
					{"きょ", "kyo"},
					{"しょ", "sho"},
					{"ちょ", "cho"},
					{"にょ", "nyo"},
					{"ひょ", "hyo"},
					{"みょ", "myo"},
					{"りょ", "ryo"},
					{"ぎょ", "gyo"},
					{"じょ",  "jo"},
					{"ぢょ",  "jo"},
					{"びょ", "byo"},
					{"ぴょ", "pyo"}
				};

				#endregion
			}

			/// <summary>
			/// Performs Modified Hepburn romanization on the given text.<br />
			/// Note this supports Hiragana and Katakana, but not Kanji.<br />
			/// Due to limits of machine romanization, certain details of the system are left out - namely vowel
			/// combinations, as the rules for combining vowels depend on whether or not the vowels share a
			/// morpheme (something not known to the program).
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text)
				=> CulturalOperations.RunWithCulture(CultureNative, () => text
					// Replace common alternate characters
					.ReplaceCommonAlternates()
					// Insert spaces at boundaries between Latin characters and Japanese ones (ie. ニンテンドーDSiブラウザー)
					.SeparateLanguageBoundaries()
					// Do multi-char combinations first (Yōon)
					.ReplaceFromChartCaseAware(YoonChart)
					// Then single-char replacements (Gojūon)
					.ReplaceFromChartCaseAware(GojuonChart)
					// Do special subsitutions
					.ReplaceMany(
						// Convert chōonpu usage in original text into macrons to mark long vowels in a romanized manner
						LongASub, LongESub, LongISub, LongOSub, LongUSub,
						// Render syllabic n as either "n'" or "n" based on whether or not it preceeds a vowel or consonant, respectively
						SyllabicNVowelsSub, SyllabicNConsonantsSub,
						// Take sokuon usage into account (repeating the following consonant to mark long consonants)
						SokuonChCaseSub, SokuonGeneralCaseSub));
		}
	}
}
