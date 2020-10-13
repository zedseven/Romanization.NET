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
	public class Japanese
	{
		// General Constants
		private const string Vowels = "aeiouy";
		private const string Consonants = "bcdfghjklmnpqrstvwxz";
		private const char IdeographicFullStop = '。';

		private static readonly Dictionary<string, string> HepburnGojuonChart = new Dictionary<string, string>();
		private static readonly Dictionary<string, string> HepburnYoonChart = new Dictionary<string, string>();
		private const char SokuonHiragana = 'っ';
		private const char SokuonKatakana = 'ッ';
		private const char Choonpu = 'ー';
		private const string SyllabicNHiragana = "ん";
		private const string SyllabicNKatakana = "ン";

		// Regex Constants
		private static Regex LanguageBoundaryRegex;
		private const string LanguageBoundaryChars = @"a-z\.\t\-?!";

		private const string LanguageBoundarySubstitution = "${1}${3} ${2}${4}";
		private static Regex LongVowelRegexA;
		private static Regex LongVowelRegexE;
		private static Regex LongVowelRegexI;
		private static Regex LongVowelRegexO;
		private static Regex LongVowelRegexU;
		private const string MacronA = "ā";
		private const string MacronE = "ē";
		private const string MacronI = "ī";
		private const string MacronO = "ō";
		private const string MacronU = "ū";

		private static Regex SyllabicNVowelsRegex;
		private static Regex SyllabicNConsonantsRegex;
		private const string SyllabicNVowelsSubstitution = "n'${1}";
		private const string SyllabicNConsonantsSubstitution = "n${1}";

		private static Regex SokuonGeneralCaseRegex;
		private static Regex SokuonChCaseRegex;
		private const string SokuonGeneralCaseSubstitution = "${1}${1}";
		private const string SokuonChCaseSubstitution = "tch";

		// Main Class Singleton
		public static readonly Lazy<Japanese> Romanizer = new Lazy<Japanese>(() => new Japanese());

		private Japanese()
		{
			// Regex Setup - this is done here so the setup and compilation only occurs if the Japanese class is used
			LanguageBoundaryRegex =
				new Regex(
					$"(?:([{LanguageBoundaryChars}])([^ {LanguageBoundaryChars}])|([^ {LanguageBoundaryChars}])([{LanguageBoundaryChars}]))",
					RegexOptions.IgnoreCase | RegexOptions.Compiled);
			
			LongVowelRegexA = new Regex($"a{Choonpu}", RegexOptions.Compiled);
			LongVowelRegexE = new Regex($"e{Choonpu}", RegexOptions.Compiled);
			LongVowelRegexI = new Regex($"i{Choonpu}", RegexOptions.Compiled);
			LongVowelRegexO = new Regex($"o{Choonpu}", RegexOptions.Compiled);
			LongVowelRegexU = new Regex($"u{Choonpu}", RegexOptions.Compiled);

			SyllabicNVowelsRegex = new Regex($"[{SyllabicNHiragana}{SyllabicNKatakana}]([{Vowels}])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			SyllabicNConsonantsRegex = new Regex($"[{SyllabicNHiragana}{SyllabicNKatakana}]([{Consonants}])", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			SokuonGeneralCaseRegex = new Regex($"[{SokuonHiragana}{SokuonKatakana}]([{Consonants}])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			SokuonChCaseRegex = new Regex($"[{SokuonHiragana}{SokuonKatakana}]ch", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			#region Romanization Charts
			// Modified Hepburn - sourced from https://en.wikipedia.org/wiki/Hepburn_romanization#Romanization_charts
			// Each pair is hiragana, then katakana, respectively

			// Gojūon
			// Column 1
			HepburnGojuonChart["あ"] = "a";
			HepburnGojuonChart["ア"] = "a";
			HepburnGojuonChart["か"] = "ka";
			HepburnGojuonChart["カ"] = "ka";
			HepburnGojuonChart["さ"] = "sa";
			HepburnGojuonChart["サ"] = "sa";
			HepburnGojuonChart["た"] = "ta";
			HepburnGojuonChart["タ"] = "ta";
			HepburnGojuonChart["な"] = "na";
			HepburnGojuonChart["ナ"] = "na";
			HepburnGojuonChart["は"] = "ha";
			HepburnGojuonChart["ハ"] = "ha";
			HepburnGojuonChart["ま"] = "ma";
			HepburnGojuonChart["マ"] = "ma";
			HepburnGojuonChart["や"] = "ya";
			HepburnGojuonChart["ヤ"] = "ya";
			HepburnGojuonChart["ら"] = "ra";
			HepburnGojuonChart["ラ"] = "ra";
			HepburnGojuonChart["わ"] = "wa";
			HepburnGojuonChart["ワ"] = "wa";
			HepburnGojuonChart["が"] = "ga";
			HepburnGojuonChart["ガ"] = "ga";
			HepburnGojuonChart["ざ"] = "za";
			HepburnGojuonChart["ザ"] = "za";
			HepburnGojuonChart["だ"] = "da";
			HepburnGojuonChart["ダ"] = "da";
			HepburnGojuonChart["ば"] = "ba";
			HepburnGojuonChart["バ"] = "ba";
			HepburnGojuonChart["ぱ"] = "pa";
			HepburnGojuonChart["パ"] = "pa";
			// Column 2
			HepburnGojuonChart["い"] = "i";
			HepburnGojuonChart["イ"] = "i";
			HepburnGojuonChart["き"] = "ki";
			HepburnGojuonChart["キ"] = "ki";
			HepburnGojuonChart["し"] = "shi";
			HepburnGojuonChart["シ"] = "shi";
			HepburnGojuonChart["ち"] = "chi";
			HepburnGojuonChart["チ"] = "chi";
			HepburnGojuonChart["に"] = "ni";
			HepburnGojuonChart["ニ"] = "ni";
			HepburnGojuonChart["ひ"] = "hi";
			HepburnGojuonChart["ヒ"] = "hi";
			HepburnGojuonChart["み"] = "mi";
			HepburnGojuonChart["ミ"] = "mi";
			HepburnGojuonChart["り"] = "ri";
			HepburnGojuonChart["リ"] = "ri";
			HepburnGojuonChart["ゐ"] = "i"; // historical character
			HepburnGojuonChart["ヰ"] = "i"; // historical character
			HepburnGojuonChart["ぎ"] = "gi";
			HepburnGojuonChart["ギ"] = "gi";
			HepburnGojuonChart["じ"] = "ji";
			HepburnGojuonChart["ジ"] = "ji";
			HepburnGojuonChart["ぢ"] = "ji";
			HepburnGojuonChart["ヂ"] = "ji";
			HepburnGojuonChart["び"] = "bi";
			HepburnGojuonChart["ビ"] = "bi";
			HepburnGojuonChart["ぴ"] = "pi";
			HepburnGojuonChart["ピ"] = "pi";
			// Column 3
			HepburnGojuonChart["う"] = "u";
			HepburnGojuonChart["ウ"] = "u";
			HepburnGojuonChart["く"] = "ku";
			HepburnGojuonChart["ク"] = "ku";
			HepburnGojuonChart["す"] = "su";
			HepburnGojuonChart["ス"] = "su";
			HepburnGojuonChart["つ"] = "tsu";
			HepburnGojuonChart["ツ"] = "tsu";
			HepburnGojuonChart["ぬ"] = "nu";
			HepburnGojuonChart["ヌ"] = "nu";
			HepburnGojuonChart["ふ"] = "fu";
			HepburnGojuonChart["フ"] = "fu";
			HepburnGojuonChart["む"] = "mu";
			HepburnGojuonChart["ム"] = "mu";
			HepburnGojuonChart["ゆ"] = "yu";
			HepburnGojuonChart["ユ"] = "yu";
			HepburnGojuonChart["る"] = "ru";
			HepburnGojuonChart["ル"] = "ru";
			HepburnGojuonChart["ぐ"] = "gu";
			HepburnGojuonChart["グ"] = "gu";
			HepburnGojuonChart["ず"] = "zu";
			HepburnGojuonChart["ズ"] = "zu";
			HepburnGojuonChart["づ"] = "zu";
			HepburnGojuonChart["ヅ"] = "zu";
			HepburnGojuonChart["ぶ"] = "bu";
			HepburnGojuonChart["ブ"] = "bu";
			HepburnGojuonChart["ぷ"] = "pu";
			HepburnGojuonChart["プ"] = "pu";
			// Column 4
			HepburnGojuonChart["え"] = "e";
			HepburnGojuonChart["エ"] = "e";
			HepburnGojuonChart["け"] = "ke";
			HepburnGojuonChart["ケ"] = "ke";
			HepburnGojuonChart["せ"] = "se";
			HepburnGojuonChart["セ"] = "se";
			HepburnGojuonChart["て"] = "te";
			HepburnGojuonChart["テ"] = "te";
			HepburnGojuonChart["ね"] = "ne";
			HepburnGojuonChart["ネ"] = "ne";
			HepburnGojuonChart["へ"] = "he";
			HepburnGojuonChart["ヘ"] = "he";
			HepburnGojuonChart["め"] = "me";
			HepburnGojuonChart["メ"] = "me";
			HepburnGojuonChart["れ"] = "re";
			HepburnGojuonChart["レ"] = "re";
			HepburnGojuonChart["ゑ"] = "e"; // historical character
			HepburnGojuonChart["ヱ"] = "e"; // historical character
			HepburnGojuonChart["げ"] = "ge";
			HepburnGojuonChart["ゲ"] = "ge";
			HepburnGojuonChart["ぜ"] = "ze";
			HepburnGojuonChart["ゼ"] = "ze";
			HepburnGojuonChart["で"] = "de";
			HepburnGojuonChart["デ"] = "de";
			HepburnGojuonChart["べ"] = "be";
			HepburnGojuonChart["ベ"] = "be";
			HepburnGojuonChart["ぺ"] = "pe";
			HepburnGojuonChart["ペ"] = "pe";
			// Column 5
			HepburnGojuonChart["お"] = "o";
			HepburnGojuonChart["オ"] = "o";
			HepburnGojuonChart["こ"] = "ko";
			HepburnGojuonChart["コ"] = "ko";
			HepburnGojuonChart["そ"] = "so";
			HepburnGojuonChart["ソ"] = "so";
			HepburnGojuonChart["と"] = "to";
			HepburnGojuonChart["ト"] = "to";
			HepburnGojuonChart["の"] = "no";
			HepburnGojuonChart["ノ"] = "no";
			HepburnGojuonChart["ほ"] = "ho";
			HepburnGojuonChart["ホ"] = "ho";
			HepburnGojuonChart["も"] = "mo";
			HepburnGojuonChart["モ"] = "mo";
			HepburnGojuonChart["よ"] = "yo";
			HepburnGojuonChart["ヨ"] = "yo";
			HepburnGojuonChart["ろ"] = "ro";
			HepburnGojuonChart["ロ"] = "ro";
			HepburnGojuonChart["を"] = "o"; // rare particle
			HepburnGojuonChart["ヲ"] = "o"; // rare particle
			//HepburnGojuonChart["ん"] = "n";
			//HepburnGojuonChart["ン"] = "n";
			HepburnGojuonChart["ご"] = "go";
			HepburnGojuonChart["ゴ"] = "go";
			HepburnGojuonChart["ぞ"] = "zo";
			HepburnGojuonChart["ゾ"] = "zo";
			HepburnGojuonChart["ど"] = "do";
			HepburnGojuonChart["ド"] = "do";
			HepburnGojuonChart["ぼ"] = "bo";
			HepburnGojuonChart["ボ"] = "bo";
			HepburnGojuonChart["ぽ"] = "po";
			HepburnGojuonChart["ポ"] = "po";

			// Yōon
			// Column 1
			HepburnYoonChart["きゃ"] = "kya";
			HepburnYoonChart["キャ"] = "kya";
			HepburnYoonChart["しゃ"] = "sha";
			HepburnYoonChart["シャ"] = "sha";
			HepburnYoonChart["ちゃ"] = "cha";
			HepburnYoonChart["チャ"] = "cha";
			HepburnYoonChart["にゃ"] = "nya";
			HepburnYoonChart["ニャ"] = "nya";
			HepburnYoonChart["ひゃ"] = "hya";
			HepburnYoonChart["ヒャ"] = "hya";
			HepburnYoonChart["みゃ"] = "mya";
			HepburnYoonChart["ミャ"] = "mya";
			HepburnYoonChart["りゃ"] = "rya";
			HepburnYoonChart["リャ"] = "rya";
			HepburnYoonChart["ぎゃ"] = "gya";
			HepburnYoonChart["ギャ"] = "gya";
			HepburnYoonChart["じゃ"] = "ja";
			HepburnYoonChart["ジャ"] = "ja";
			HepburnYoonChart["ぢゃ"] = "ja";
			HepburnYoonChart["ヂャ"] = "ja";
			HepburnYoonChart["びゃ"] = "bya";
			HepburnYoonChart["ビャ"] = "bya";
			HepburnYoonChart["ぴゃ"] = "pya";
			HepburnYoonChart["ピャ"] = "pya";
			// Column 2
			HepburnYoonChart["きゅ"] = "kyu";
			HepburnYoonChart["キュ"] = "kyu";
			HepburnYoonChart["しゅ"] = "shu";
			HepburnYoonChart["シュ"] = "shu";
			HepburnYoonChart["ちゅ"] = "chu";
			HepburnYoonChart["チュ"] = "chu";
			HepburnYoonChart["にゅ"] = "nyu";
			HepburnYoonChart["ニュ"] = "nyu";
			HepburnYoonChart["ひゅ"] = "hyu";
			HepburnYoonChart["ヒュ"] = "hyu";
			HepburnYoonChart["みゅ"] = "myu";
			HepburnYoonChart["ミュ"] = "myu";
			HepburnYoonChart["りゅ"] = "ryu";
			HepburnYoonChart["リュ"] = "ryu";
			HepburnYoonChart["ぎゅ"] = "gyu";
			HepburnYoonChart["ギュ"] = "gyu";
			HepburnYoonChart["じゅ"] = "ju";
			HepburnYoonChart["ジュ"] = "ju";
			HepburnYoonChart["ぢゅ"] = "ju";
			HepburnYoonChart["ヂュ"] = "ju";
			HepburnYoonChart["びゅ"] = "byu";
			HepburnYoonChart["ビュ"] = "byu";
			HepburnYoonChart["ぴゅ"] = "pyu";
			HepburnYoonChart["ピュ"] = "pyu";
			// Column 3
			HepburnYoonChart["きょ"] = "kyo";
			HepburnYoonChart["キョ"] = "kyo";
			HepburnYoonChart["しょ"] = "sho";
			HepburnYoonChart["ショ"] = "sho";
			HepburnYoonChart["ちょ"] = "cho";
			HepburnYoonChart["チョ"] = "cho";
			HepburnYoonChart["にょ"] = "nyo";
			HepburnYoonChart["ニョ"] = "nyo";
			HepburnYoonChart["ひょ"] = "hyo";
			HepburnYoonChart["ヒョ"] = "hyo";
			HepburnYoonChart["みょ"] = "myo";
			HepburnYoonChart["ミョ"] = "myo";
			HepburnYoonChart["りょ"] = "ryo";
			HepburnYoonChart["リョ"] = "ryo";
			HepburnYoonChart["ぎょ"] = "gyo";
			HepburnYoonChart["ギョ"] = "gyo";
			HepburnYoonChart["じょ"] = "jo";
			HepburnYoonChart["ジョ"] = "jo";
			HepburnYoonChart["ぢょ"] = "jo";
			HepburnYoonChart["ヂョ"] = "jo";
			HepburnYoonChart["びょ"] = "byo";
			HepburnYoonChart["ビョ"] = "byo";
			HepburnYoonChart["ぴょ"] = "pyo";
			HepburnYoonChart["ピョ"] = "pyo";

			#endregion
		}

		/// <summary>
		/// Performs Modified Hepburn romanization on the given text.<br />
		/// Note this supports Hiragana and Katakana, but not Kanji.<br />
		/// Due to limits of machine romanization, certain details of the system are left out - namely vowel
		/// combinations, as the rules for combining vowels depend on whether or not the vowels share a
		/// morpheme (something not known to the program).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Hepburn_romanization'>https://en.wikipedia.org/wiki/Hepburn_romanization</a>
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <returns>A romanized version of the text, leaving non-recognized characters untouched.</returns>
		[Pure]
		public string ModifiedHepburn(string text)
		{
			// Insert spaces at boundaries between Latin characters and Japanese ones (ie. ニンテンドーDSiブラウザー)
			text = LanguageBoundaryRegex.Replace(text, LanguageBoundarySubstitution);

			// Do multi-char combinations first (Yōon)
			text = HepburnYoonChart.Keys.Aggregate(text, (current, yoonString)
					=> current.Replace(yoonString, HepburnYoonChart[yoonString]));
			// Then single-char replacements (Gojūon)
			text = HepburnGojuonChart.Keys.Aggregate(text, (current, gojuonChar)
					=> current.Replace(gojuonChar, HepburnGojuonChart[gojuonChar]));

			// Replace the sometimes-used ideographic full-stop with a period
			text = text.Replace(IdeographicFullStop, '.');

			// Convert chōonpu usage in original text into macrons to mark long vowels in a romanized manner
			text = LongVowelRegexA.Replace(
				LongVowelRegexE.Replace(
					LongVowelRegexI.Replace(
						LongVowelRegexO.Replace(
							LongVowelRegexU.Replace(text,
								MacronU), MacronO), MacronI), MacronE), MacronA);

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
