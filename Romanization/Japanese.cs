using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	/// <summary>
	/// The class for romanizing Japanese text. (Rōmaji)
	/// </summary>
	public static class Japanese
	{
		// Japanese Characters
		private const char SokuonHiragana = 'っ';
		private const char SokuonKatakana = 'ッ';
		private const char Choonpu = 'ー';
		private const string SyllabicNHiragana = "ん";
		private const string SyllabicNKatakana = "ン";

		// System Singletons
		public static readonly Lazy<ModifiedHepburnSystem> ModifiedHepburn = new Lazy<ModifiedHepburnSystem>(() => new ModifiedHepburnSystem());

		/// <summary>
		/// The Modified Hepburn Japanese romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Hepburn_romanization'>https://en.wikipedia.org/wiki/Hepburn_romanization</a>
		/// </summary>
		public sealed class ModifiedHepburnSystem : IRomanizationSystem
		{
			// System-Specific Constants
			private static readonly Dictionary<string, string> GojuonChart = new Dictionary<string, string>();
			private static readonly Dictionary<string, string> YoonChart = new Dictionary<string, string>();

			private static Regex LongVowelRegexA;
			private static Regex LongVowelRegexE;
			private static Regex LongVowelRegexI;
			private static Regex LongVowelRegexO;
			private static Regex LongVowelRegexU;

			private static Regex SyllabicNVowelsRegex;
			private static Regex SyllabicNConsonantsRegex;
			private const string SyllabicNVowelsSubstitution = "n'${1}";
			private const string SyllabicNConsonantsSubstitution = "n${1}";

			private static Regex SokuonGeneralCaseRegex;
			private static Regex SokuonChCaseRegex;
			private const string SokuonGeneralCaseSubstitution = "${1}${1}";
			private const string SokuonChCaseSubstitution = "tch";

			internal ModifiedHepburnSystem()
			{
				LongVowelRegexA = new Regex($"a{Choonpu}", RegexOptions.Compiled);
				LongVowelRegexE = new Regex($"e{Choonpu}", RegexOptions.Compiled);
				LongVowelRegexI = new Regex($"i{Choonpu}", RegexOptions.Compiled);
				LongVowelRegexO = new Regex($"o{Choonpu}", RegexOptions.Compiled);
				LongVowelRegexU = new Regex($"u{Choonpu}", RegexOptions.Compiled);

				SyllabicNVowelsRegex     = new Regex($"[{SyllabicNHiragana}{SyllabicNKatakana}]([{LanguageAgnostic.Vowels}])",     RegexOptions.Compiled | RegexOptions.IgnoreCase);
				SyllabicNConsonantsRegex = new Regex($"[{SyllabicNHiragana}{SyllabicNKatakana}]([{LanguageAgnostic.Consonants}])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

				SokuonGeneralCaseRegex = new Regex($"[{SokuonHiragana}{SokuonKatakana}]([{LanguageAgnostic.Consonants}])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
				SokuonChCaseRegex      = new Regex($"[{SokuonHiragana}{SokuonKatakana}]ch",                                RegexOptions.Compiled | RegexOptions.IgnoreCase);

				#region Romanization Chart
				// Sourced from https://en.wikipedia.org/wiki/Hepburn_romanization#Romanization_charts
				// Each pair is hiragana, then katakana, respectively

				// Gojūon
				// Column 1
				GojuonChart["あ"] = "a";
				GojuonChart["ア"] = "a";
				GojuonChart["か"] = "ka";
				GojuonChart["カ"] = "ka";
				GojuonChart["さ"] = "sa";
				GojuonChart["サ"] = "sa";
				GojuonChart["た"] = "ta";
				GojuonChart["タ"] = "ta";
				GojuonChart["な"] = "na";
				GojuonChart["ナ"] = "na";
				GojuonChart["は"] = "ha";
				GojuonChart["ハ"] = "ha";
				GojuonChart["ま"] = "ma";
				GojuonChart["マ"] = "ma";
				GojuonChart["や"] = "ya";
				GojuonChart["ヤ"] = "ya";
				GojuonChart["ら"] = "ra";
				GojuonChart["ラ"] = "ra";
				GojuonChart["わ"] = "wa";
				GojuonChart["ワ"] = "wa";
				GojuonChart["が"] = "ga";
				GojuonChart["ガ"] = "ga";
				GojuonChart["ざ"] = "za";
				GojuonChart["ザ"] = "za";
				GojuonChart["だ"] = "da";
				GojuonChart["ダ"] = "da";
				GojuonChart["ば"] = "ba";
				GojuonChart["バ"] = "ba";
				GojuonChart["ぱ"] = "pa";
				GojuonChart["パ"] = "pa";
				// Column 2
				GojuonChart["い"] = "i";
				GojuonChart["イ"] = "i";
				GojuonChart["き"] = "ki";
				GojuonChart["キ"] = "ki";
				GojuonChart["し"] = "shi";
				GojuonChart["シ"] = "shi";
				GojuonChart["ち"] = "chi";
				GojuonChart["チ"] = "chi";
				GojuonChart["に"] = "ni";
				GojuonChart["ニ"] = "ni";
				GojuonChart["ひ"] = "hi";
				GojuonChart["ヒ"] = "hi";
				GojuonChart["み"] = "mi";
				GojuonChart["ミ"] = "mi";
				GojuonChart["り"] = "ri";
				GojuonChart["リ"] = "ri";
				GojuonChart["ゐ"] = "i"; // historical character
				GojuonChart["ヰ"] = "i"; // historical character
				GojuonChart["ぎ"] = "gi";
				GojuonChart["ギ"] = "gi";
				GojuonChart["じ"] = "ji";
				GojuonChart["ジ"] = "ji";
				GojuonChart["ぢ"] = "ji";
				GojuonChart["ヂ"] = "ji";
				GojuonChart["び"] = "bi";
				GojuonChart["ビ"] = "bi";
				GojuonChart["ぴ"] = "pi";
				GojuonChart["ピ"] = "pi";
				// Column 3
				GojuonChart["う"] = "u";
				GojuonChart["ウ"] = "u";
				GojuonChart["く"] = "ku";
				GojuonChart["ク"] = "ku";
				GojuonChart["す"] = "su";
				GojuonChart["ス"] = "su";
				GojuonChart["つ"] = "tsu";
				GojuonChart["ツ"] = "tsu";
				GojuonChart["ぬ"] = "nu";
				GojuonChart["ヌ"] = "nu";
				GojuonChart["ふ"] = "fu";
				GojuonChart["フ"] = "fu";
				GojuonChart["む"] = "mu";
				GojuonChart["ム"] = "mu";
				GojuonChart["ゆ"] = "yu";
				GojuonChart["ユ"] = "yu";
				GojuonChart["る"] = "ru";
				GojuonChart["ル"] = "ru";
				GojuonChart["ぐ"] = "gu";
				GojuonChart["グ"] = "gu";
				GojuonChart["ず"] = "zu";
				GojuonChart["ズ"] = "zu";
				GojuonChart["づ"] = "zu";
				GojuonChart["ヅ"] = "zu";
				GojuonChart["ぶ"] = "bu";
				GojuonChart["ブ"] = "bu";
				GojuonChart["ぷ"] = "pu";
				GojuonChart["プ"] = "pu";
				// Column 4
				GojuonChart["え"] = "e";
				GojuonChart["エ"] = "e";
				GojuonChart["け"] = "ke";
				GojuonChart["ケ"] = "ke";
				GojuonChart["せ"] = "se";
				GojuonChart["セ"] = "se";
				GojuonChart["て"] = "te";
				GojuonChart["テ"] = "te";
				GojuonChart["ね"] = "ne";
				GojuonChart["ネ"] = "ne";
				GojuonChart["へ"] = "he";
				GojuonChart["ヘ"] = "he";
				GojuonChart["め"] = "me";
				GojuonChart["メ"] = "me";
				GojuonChart["れ"] = "re";
				GojuonChart["レ"] = "re";
				GojuonChart["ゑ"] = "e"; // historical character
				GojuonChart["ヱ"] = "e"; // historical character
				GojuonChart["げ"] = "ge";
				GojuonChart["ゲ"] = "ge";
				GojuonChart["ぜ"] = "ze";
				GojuonChart["ゼ"] = "ze";
				GojuonChart["で"] = "de";
				GojuonChart["デ"] = "de";
				GojuonChart["べ"] = "be";
				GojuonChart["ベ"] = "be";
				GojuonChart["ぺ"] = "pe";
				GojuonChart["ペ"] = "pe";
				// Column 5
				GojuonChart["お"] = "o";
				GojuonChart["オ"] = "o";
				GojuonChart["こ"] = "ko";
				GojuonChart["コ"] = "ko";
				GojuonChart["そ"] = "so";
				GojuonChart["ソ"] = "so";
				GojuonChart["と"] = "to";
				GojuonChart["ト"] = "to";
				GojuonChart["の"] = "no";
				GojuonChart["ノ"] = "no";
				GojuonChart["ほ"] = "ho";
				GojuonChart["ホ"] = "ho";
				GojuonChart["も"] = "mo";
				GojuonChart["モ"] = "mo";
				GojuonChart["よ"] = "yo";
				GojuonChart["ヨ"] = "yo";
				GojuonChart["ろ"] = "ro";
				GojuonChart["ロ"] = "ro";
				GojuonChart["を"] = "o"; // rare particle
				GojuonChart["ヲ"] = "o"; // rare particle
				/*GojuonChart["ん"] = "n";
				GojuonChart["ン"] = "n";*/
				GojuonChart["ご"] = "go";
				GojuonChart["ゴ"] = "go";
				GojuonChart["ぞ"] = "zo";
				GojuonChart["ゾ"] = "zo";
				GojuonChart["ど"] = "do";
				GojuonChart["ド"] = "do";
				GojuonChart["ぼ"] = "bo";
				GojuonChart["ボ"] = "bo";
				GojuonChart["ぽ"] = "po";
				GojuonChart["ポ"] = "po";

				// Yōon
				// Column 1
				YoonChart["きゃ"] = "kya";
				YoonChart["キャ"] = "kya";
				YoonChart["しゃ"] = "sha";
				YoonChart["シャ"] = "sha";
				YoonChart["ちゃ"] = "cha";
				YoonChart["チャ"] = "cha";
				YoonChart["にゃ"] = "nya";
				YoonChart["ニャ"] = "nya";
				YoonChart["ひゃ"] = "hya";
				YoonChart["ヒャ"] = "hya";
				YoonChart["みゃ"] = "mya";
				YoonChart["ミャ"] = "mya";
				YoonChart["りゃ"] = "rya";
				YoonChart["リャ"] = "rya";
				YoonChart["ぎゃ"] = "gya";
				YoonChart["ギャ"] = "gya";
				YoonChart["じゃ"] = "ja";
				YoonChart["ジャ"] = "ja";
				YoonChart["ぢゃ"] = "ja";
				YoonChart["ヂャ"] = "ja";
				YoonChart["びゃ"] = "bya";
				YoonChart["ビャ"] = "bya";
				YoonChart["ぴゃ"] = "pya";
				YoonChart["ピャ"] = "pya";
				// Column 2
				YoonChart["きゅ"] = "kyu";
				YoonChart["キュ"] = "kyu";
				YoonChart["しゅ"] = "shu";
				YoonChart["シュ"] = "shu";
				YoonChart["ちゅ"] = "chu";
				YoonChart["チュ"] = "chu";
				YoonChart["にゅ"] = "nyu";
				YoonChart["ニュ"] = "nyu";
				YoonChart["ひゅ"] = "hyu";
				YoonChart["ヒュ"] = "hyu";
				YoonChart["みゅ"] = "myu";
				YoonChart["ミュ"] = "myu";
				YoonChart["りゅ"] = "ryu";
				YoonChart["リュ"] = "ryu";
				YoonChart["ぎゅ"] = "gyu";
				YoonChart["ギュ"] = "gyu";
				YoonChart["じゅ"] = "ju";
				YoonChart["ジュ"] = "ju";
				YoonChart["ぢゅ"] = "ju";
				YoonChart["ヂュ"] = "ju";
				YoonChart["びゅ"] = "byu";
				YoonChart["ビュ"] = "byu";
				YoonChart["ぴゅ"] = "pyu";
				YoonChart["ピュ"] = "pyu";
				// Column 3
				YoonChart["きょ"] = "kyo";
				YoonChart["キョ"] = "kyo";
				YoonChart["しょ"] = "sho";
				YoonChart["ショ"] = "sho";
				YoonChart["ちょ"] = "cho";
				YoonChart["チョ"] = "cho";
				YoonChart["にょ"] = "nyo";
				YoonChart["ニョ"] = "nyo";
				YoonChart["ひょ"] = "hyo";
				YoonChart["ヒョ"] = "hyo";
				YoonChart["みょ"] = "myo";
				YoonChart["ミョ"] = "myo";
				YoonChart["りょ"] = "ryo";
				YoonChart["リョ"] = "ryo";
				YoonChart["ぎょ"] = "gyo";
				YoonChart["ギョ"] = "gyo";
				YoonChart["じょ"] = "jo";
				YoonChart["ジョ"] = "jo";
				YoonChart["ぢょ"] = "jo";
				YoonChart["ヂョ"] = "jo";
				YoonChart["びょ"] = "byo";
				YoonChart["ビョ"] = "byo";
				YoonChart["ぴょ"] = "pyo";
				YoonChart["ピョ"] = "pyo";

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
			{
				// Insert spaces at boundaries between Latin characters and Japanese ones (ie. ニンテンドーDSiブラウザー)
				text = LanguageAgnostic.SeparateLanguageBoundaries(text);

				// Do multi-char combinations first (Yōon)
				text = YoonChart.Keys.Aggregate(text, (current, yoonString)
						=> current.Replace(yoonString, YoonChart[yoonString]));
				// Then single-char replacements (Gojūon)
				text = GojuonChart.Keys.Aggregate(text, (current, gojuonChar)
						=> current.Replace(gojuonChar, GojuonChart[gojuonChar]));

				// Replace common alternate characters
				text = LanguageAgnostic.RemoveCommonAlternates(text);

				// Convert chōonpu usage in original text into macrons to mark long vowels in a romanized manner
				text = LongVowelRegexA.Replace(
					LongVowelRegexE.Replace(
						LongVowelRegexI.Replace(
							LongVowelRegexO.Replace(
								LongVowelRegexU.Replace(text,
									LanguageAgnostic.MacronU),
								LanguageAgnostic.MacronO),
							LanguageAgnostic.MacronI),
						LanguageAgnostic.MacronE),
					LanguageAgnostic.MacronA);

				// Render syllabic n as either "n'" or "n" based on whether or not it preceeds a vowel or consonant, respectively
				text = SyllabicNConsonantsRegex.Replace(
					SyllabicNVowelsRegex.Replace(text, SyllabicNVowelsSubstitution),
					SyllabicNConsonantsSubstitution);

				// Take sokuon usage into account (repeating the following consonant to mark long consonants)
				text = SokuonGeneralCaseRegex.Replace(
					SokuonChCaseRegex.Replace(text, SokuonChCaseSubstitution),
					SokuonGeneralCaseSubstitution);

				return text;
			}
		}
	}
}
