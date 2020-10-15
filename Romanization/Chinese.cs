using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	/// <summary>
	/// The class for romanizing Chinese text.
	/// </summary>
	public static class Chinese
	{
		// System Singletons
		public static readonly Lazy<HanyuPinyinSystem> HanyuPinyin = new Lazy<HanyuPinyinSystem>(() => new HanyuPinyinSystem());

		/// <summary>
		/// The Hànyǔ Pīnyīn Chinese romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Hanyu_Pinyin'>https://en.wikipedia.org/wiki/Hanyu_Pinyin</a>
		/// </summary>
		public sealed class HanyuPinyinSystem : IRomanizationSystem
		{
			private static readonly Dictionary<string, string> TripleCharChart = new Dictionary<string, string>();
			private static readonly Dictionary<string, string> DoubleCharChart = new Dictionary<string, string>();
			private static readonly Dictionary<string, string> SingleCharChart = new Dictionary<string, string>();

			internal HanyuPinyinSystem()
			{
				#region Romanization Chart
				// Sourced from http://www.pinyin.info/romanization/compare/hanyu.html

				// 3-Characters
				TripleCharChart["ㄅㄧㄢ"] = "bian";
				TripleCharChart["ㄅㄧㄠ"] = "biao";
				TripleCharChart["ㄅㄧㄝ"] = "bie";
				TripleCharChart["ㄅㄧㄣ"] = "bin";
				TripleCharChart["ㄅㄧㄥ"] = "bing";
				TripleCharChart["ㄔㄨㄥ"] = "chong";
				TripleCharChart["ㄔㄨㄚ"] = "chua";
				TripleCharChart["ㄔㄨㄞ"] = "chuai";
				TripleCharChart["ㄔㄨㄢ"] = "chuan";
				TripleCharChart["ㄔㄨㄤ"] = "chuang";
				TripleCharChart["ㄔㄨㄟ"] = "chui";
				TripleCharChart["ㄔㄨㄣ"] = "chun";
				TripleCharChart["ㄔㄨㄛ"] = "chuo";
				TripleCharChart["ㄘㄨㄥ"] = "cong";
				TripleCharChart["ㄘㄨㄢ"] = "cuan";
				TripleCharChart["ㄘㄨㄟ"] = "cui";
				TripleCharChart["ㄘㄨㄣ"] = "cun";
				TripleCharChart["ㄘㄨㄛ"] = "cuo";
				TripleCharChart["ㄉㄧㄢ"] = "dian";
				TripleCharChart["ㄉㄧㄤ"] = "diang";
				TripleCharChart["ㄉㄧㄠ"] = "diao";
				TripleCharChart["ㄉㄧㄝ"] = "die";
				TripleCharChart["ㄉㄧㄥ"] = "ding";
				TripleCharChart["ㄉㄧㄡ"] = "diu";
				TripleCharChart["ㄉㄨㄥ"] = "dong";
				TripleCharChart["ㄉㄨㄢ"] = "duan";
				TripleCharChart["ㄉㄨㄟ"] = "dui";
				TripleCharChart["ㄉㄨㄣ"] = "dun";
				TripleCharChart["ㄉㄨㄛ"] = "duo";
				TripleCharChart["ㄍㄨㄥ"] = "gong";
				TripleCharChart["ㄍㄨㄚ"] = "gua";
				TripleCharChart["ㄍㄨㄞ"] = "guai";
				TripleCharChart["ㄍㄨㄢ"] = "guan";
				TripleCharChart["ㄍㄨㄤ"] = "guang";
				TripleCharChart["ㄍㄨㄟ"] = "gui";
				TripleCharChart["ㄍㄨㄣ"] = "gun";
				TripleCharChart["ㄍㄨㄛ"] = "guo";
				TripleCharChart["ㄏㄨㄥ"] = "hong";
				TripleCharChart["ㄏㄨㄚ"] = "hua";
				TripleCharChart["ㄏㄨㄞ"] = "huai";
				TripleCharChart["ㄏㄨㄢ"] = "huan";
				TripleCharChart["ㄏㄨㄤ"] = "huang";
				TripleCharChart["ㄏㄨㄟ"] = "hui";
				TripleCharChart["ㄏㄨㄣ"] = "hun";
				TripleCharChart["ㄏㄨㄛ"] = "huo";
				TripleCharChart["ㄐㄧㄚ"] = "jia";
				TripleCharChart["ㄐㄧㄢ"] = "jian";
				TripleCharChart["ㄐㄧㄤ"] = "jiang";
				TripleCharChart["ㄐㄧㄠ"] = "jiao";
				TripleCharChart["ㄐㄧㄝ"] = "jie";
				TripleCharChart["ㄐㄧㄣ"] = "jin";
				TripleCharChart["ㄐㄧㄥ"] = "jing";
				TripleCharChart["ㄐㄩㄥ"] = "jiong";
				TripleCharChart["ㄐㄧㄡ"] = "jiu";
				TripleCharChart["ㄐㄩㄢ"] = "juan";
				TripleCharChart["ㄐㄩㄝ"] = "jue";
				TripleCharChart["ㄐㄩㄣ"] = "jun";
				TripleCharChart["ㄎㄨㄥ"] = "kong";
				TripleCharChart["ㄎㄨㄚ"] = "kua";
				TripleCharChart["ㄎㄨㄞ"] = "kuai";
				TripleCharChart["ㄎㄨㄢ"] = "kuan";
				TripleCharChart["ㄎㄨㄤ"] = "kuang";
				TripleCharChart["ㄎㄨㄟ"] = "kui";
				TripleCharChart["ㄎㄨㄣ"] = "kun";
				TripleCharChart["ㄎㄨㄛ"] = "kuo";
				TripleCharChart["ㄌㄧㄚ"] = "lia";
				TripleCharChart["ㄌㄧㄢ"] = "lian";
				TripleCharChart["ㄌㄧㄤ"] = "liang";
				TripleCharChart["ㄌㄧㄠ"] = "liao";
				TripleCharChart["ㄌㄧㄝ"] = "lie";
				TripleCharChart["ㄌㄧㄣ"] = "lin";
				TripleCharChart["ㄌㄧㄥ"] = "ling";
				TripleCharChart["ㄌㄧㄡ"] = "liu";
				TripleCharChart["ㄌㄨㄥ"] = "long";
				TripleCharChart["ㄌㄨㄢ"] = "luan";
				TripleCharChart["ㄌㄨㄣ"] = "lun";
				TripleCharChart["ㄌㄨㄛ"] = "luo";
				TripleCharChart["ㄌㄩㄝ"] = "lüe";
				TripleCharChart["ㄌㄩㄣ"] = "lün";
				TripleCharChart["ㄇㄧㄢ"] = "mian";
				TripleCharChart["ㄇㄧㄠ"] = "miao";
				TripleCharChart["ㄇㄧㄝ"] = "mie";
				TripleCharChart["ㄇㄧㄣ"] = "min";
				TripleCharChart["ㄇㄧㄥ"] = "ming";
				TripleCharChart["ㄇㄧㄡ"] = "miu";
				TripleCharChart["ㄋㄧㄚ"] = "nia";
				TripleCharChart["ㄋㄧㄢ"] = "nian";
				TripleCharChart["ㄋㄧㄤ"] = "niang";
				TripleCharChart["ㄋㄧㄠ"] = "niao";
				TripleCharChart["ㄋㄧㄝ"] = "nie";
				TripleCharChart["ㄋㄧㄣ"] = "nin";
				TripleCharChart["ㄋㄧㄥ"] = "ning";
				TripleCharChart["ㄋㄧㄡ"] = "niu";
				TripleCharChart["ㄋㄨㄥ"] = "nong";
				TripleCharChart["ㄋㄨㄢ"] = "nuan";
				TripleCharChart["ㄋㄨㄣ"] = "nun";
				TripleCharChart["ㄋㄨㄛ"] = "nuo";
				TripleCharChart["ㄋㄩㄝ"] = "nüe";
				TripleCharChart["ㄆㄧㄢ"] = "pian";
				TripleCharChart["ㄆㄧㄠ"] = "piao";
				TripleCharChart["ㄆㄧㄝ"] = "pie";
				TripleCharChart["ㄆㄧㄣ"] = "pin";
				TripleCharChart["ㄆㄧㄥ"] = "ping";
				TripleCharChart["ㄑㄧㄚ"] = "qia";
				TripleCharChart["ㄑㄧㄢ"] = "qian";
				TripleCharChart["ㄑㄧㄤ"] = "qiang";
				TripleCharChart["ㄑㄧㄠ"] = "qiao";
				TripleCharChart["ㄑㄧㄝ"] = "qie";
				TripleCharChart["ㄑㄧㄣ"] = "qin";
				TripleCharChart["ㄑㄧㄥ"] = "qing";
				TripleCharChart["ㄑㄩㄥ"] = "qiong";
				TripleCharChart["ㄑㄧㄡ"] = "qiu";
				TripleCharChart["ㄑㄩㄢ"] = "quan";
				TripleCharChart["ㄑㄩㄝ"] = "que";
				TripleCharChart["ㄑㄩㄣ"] = "qun";
				TripleCharChart["ㄖㄨㄥ"] = "rong";
				TripleCharChart["ㄖㄨㄢ"] = "ruan";
				TripleCharChart["ㄖㄨㄟ"] = "rui";
				TripleCharChart["ㄖㄨㄣ"] = "run";
				TripleCharChart["ㄖㄨㄛ"] = "ruo";
				TripleCharChart["ㄕㄨㄥ"] = "shong";
				TripleCharChart["ㄕㄨㄚ"] = "shua";
				TripleCharChart["ㄕㄨㄞ"] = "shuai";
				TripleCharChart["ㄕㄨㄢ"] = "shuan";
				TripleCharChart["ㄕㄨㄤ"] = "shuang";
				TripleCharChart["ㄕㄨㄟ"] = "shui";
				TripleCharChart["ㄕㄨㄣ"] = "shun";
				TripleCharChart["ㄕㄨㄛ"] = "shuo";
				TripleCharChart["ㄙㄨㄥ"] = "song";
				TripleCharChart["ㄙㄨㄢ"] = "suan";
				TripleCharChart["ㄙㄨㄟ"] = "sui";
				TripleCharChart["ㄙㄨㄣ"] = "sun";
				TripleCharChart["ㄙㄨㄛ"] = "suo";
				TripleCharChart["ㄊㄧㄢ"] = "tian";
				TripleCharChart["ㄊㄧㄠ"] = "tiao";
				TripleCharChart["ㄊㄧㄝ"] = "tie";
				TripleCharChart["ㄊㄧㄥ"] = "ting";
				TripleCharChart["ㄊㄨㄥ"] = "tong";
				TripleCharChart["ㄊㄨㄢ"] = "tuan";
				TripleCharChart["ㄊㄨㄟ"] = "tui";
				TripleCharChart["ㄊㄨㄣ"] = "tun";
				TripleCharChart["ㄊㄨㄛ"] = "tuo";
				TripleCharChart["ㄒㄧㄚ"] = "xia";
				TripleCharChart["ㄒㄧㄢ"] = "xian";
				TripleCharChart["ㄒㄧㄤ"] = "xiang";
				TripleCharChart["ㄒㄧㄠ"] = "xiao";
				TripleCharChart["ㄒㄧㄝ"] = "xie";
				TripleCharChart["ㄒㄧㄣ"] = "xin";
				TripleCharChart["ㄒㄧㄥ"] = "xing";
				TripleCharChart["ㄒㄩㄥ"] = "xiong";
				TripleCharChart["ㄒㄧㄡ"] = "xiu";
				TripleCharChart["ㄒㄩㄢ"] = "xuan";
				TripleCharChart["ㄒㄩㄝ"] = "xue";
				TripleCharChart["ㄒㄩㄣ"] = "xun";
				TripleCharChart["ㄓㄨㄥ"] = "zhong";
				TripleCharChart["ㄓㄨㄚ"] = "zhua";
				TripleCharChart["ㄓㄨㄞ"] = "zhuai";
				TripleCharChart["ㄓㄨㄢ"] = "zhuan";
				TripleCharChart["ㄓㄨㄤ"] = "zhuang";
				TripleCharChart["ㄓㄨㄟ"] = "zhui";
				TripleCharChart["ㄓㄨㄣ"] = "zhun";
				TripleCharChart["ㄓㄨㄛ"] = "zhuo";
				TripleCharChart["ㄗㄨㄥ"] = "zong";
				TripleCharChart["ㄗㄨㄢ"] = "zuan";
				TripleCharChart["ㄗㄨㄟ"] = "zui";
				TripleCharChart["ㄗㄨㄣ"] = "zun";
				TripleCharChart["ㄗㄨㄛ"] = "zuo";

				// 2-Characters
				DoubleCharChart["ㄅㄚ"] = "ba";
				DoubleCharChart["ㄅㄞ"] = "bai";
				DoubleCharChart["ㄅㄢ"] = "ban";
				DoubleCharChart["ㄅㄤ"] = "bang";
				DoubleCharChart["ㄅㄠ"] = "bao";
				DoubleCharChart["ㄅㄟ"] = "bei";
				DoubleCharChart["ㄅㄣ"] = "ben";
				DoubleCharChart["ㄅㄥ"] = "beng";
				DoubleCharChart["ㄅㄧ"] = "bi";
				DoubleCharChart["ㄅㄛ"] = "bo";
				DoubleCharChart["ㄅㄨ"] = "bu";
				DoubleCharChart["ㄘㄚ"] = "ca";
				DoubleCharChart["ㄘㄞ"] = "cai";
				DoubleCharChart["ㄘㄢ"] = "can";
				DoubleCharChart["ㄘㄤ"] = "cang";
				DoubleCharChart["ㄘㄠ"] = "cao";
				DoubleCharChart["ㄘㄜ"] = "ce";
				DoubleCharChart["ㄘㄣ"] = "cen";
				DoubleCharChart["ㄘㄥ"] = "ceng";
				DoubleCharChart["ㄔㄚ"] = "cha";
				DoubleCharChart["ㄔㄞ"] = "chai";
				DoubleCharChart["ㄔㄢ"] = "chan";
				DoubleCharChart["ㄔㄤ"] = "chang";
				DoubleCharChart["ㄔㄠ"] = "chao";
				DoubleCharChart["ㄔㄜ"] = "che";
				DoubleCharChart["ㄔㄣ"] = "chen";
				DoubleCharChart["ㄔㄥ"] = "cheng";
				DoubleCharChart["ㄔㄡ"] = "chou";
				DoubleCharChart["ㄔㄨ"] = "chu";
				DoubleCharChart["ㄘㄡ"] = "cou";
				DoubleCharChart["ㄘㄨ"] = "cu";
				DoubleCharChart["ㄉㄚ"] = "da";
				DoubleCharChart["ㄉㄞ"] = "dai";
				DoubleCharChart["ㄉㄢ"] = "dan";
				DoubleCharChart["ㄉㄤ"] = "dang";
				DoubleCharChart["ㄉㄠ"] = "dao";
				DoubleCharChart["ㄉㄜ"] = "de";
				DoubleCharChart["ㄉㄟ"] = "dei";
				DoubleCharChart["ㄉㄣ"] = "den";
				DoubleCharChart["ㄉㄥ"] = "deng";
				DoubleCharChart["ㄉㄧ"] = "di";
				DoubleCharChart["ㄉㄡ"] = "dou";
				DoubleCharChart["ㄉㄨ"] = "du";
				DoubleCharChart["ㄈㄚ"] = "fa";
				DoubleCharChart["ㄈㄢ"] = "fan";
				DoubleCharChart["ㄈㄤ"] = "fang";
				DoubleCharChart["ㄈㄟ"] = "fei";
				DoubleCharChart["ㄈㄣ"] = "fen";
				DoubleCharChart["ㄈㄥ"] = "feng";
				DoubleCharChart["ㄈㄛ"] = "fo";
				DoubleCharChart["ㄈㄡ"] = "fou";
				DoubleCharChart["ㄈㄨ"] = "fu";
				DoubleCharChart["ㄍㄚ"] = "ga";
				DoubleCharChart["ㄍㄞ"] = "gai";
				DoubleCharChart["ㄍㄢ"] = "gan";
				DoubleCharChart["ㄍㄤ"] = "gang";
				DoubleCharChart["ㄍㄠ"] = "gao";
				DoubleCharChart["ㄍㄜ"] = "ge";
				DoubleCharChart["ㄍㄟ"] = "gei";
				DoubleCharChart["ㄍㄣ"] = "gen";
				DoubleCharChart["ㄍㄥ"] = "geng";
				DoubleCharChart["ㄍㄡ"] = "gou";
				DoubleCharChart["ㄍㄨ"] = "gu";
				DoubleCharChart["ㄏㄚ"] = "ha";
				DoubleCharChart["ㄏㄞ"] = "hai";
				DoubleCharChart["ㄏㄢ"] = "han";
				DoubleCharChart["ㄏㄤ"] = "hang";
				DoubleCharChart["ㄏㄠ"] = "hao";
				DoubleCharChart["ㄏㄜ"] = "he";
				DoubleCharChart["ㄏㄟ"] = "hei";
				DoubleCharChart["ㄏㄣ"] = "hen";
				DoubleCharChart["ㄏㄥ"] = "heng";
				DoubleCharChart["ㄏㄡ"] = "hou";
				DoubleCharChart["ㄏㄨ"] = "hu";
				DoubleCharChart["ㄐㄧ"] = "ji";
				DoubleCharChart["ㄐㄩ"] = "ju";
				DoubleCharChart["ㄎㄚ"] = "ka";
				DoubleCharChart["ㄎㄞ"] = "kai";
				DoubleCharChart["ㄎㄢ"] = "kan";
				DoubleCharChart["ㄎㄤ"] = "kang";
				DoubleCharChart["ㄎㄠ"] = "kao";
				DoubleCharChart["ㄎㄜ"] = "ke";
				DoubleCharChart["ㄎㄣ"] = "ken";
				DoubleCharChart["ㄎㄥ"] = "keng";
				DoubleCharChart["ㄎㄡ"] = "kou";
				DoubleCharChart["ㄎㄨ"] = "ku";
				DoubleCharChart["ㄌㄚ"] = "la";
				DoubleCharChart["ㄌㄞ"] = "lai";
				DoubleCharChart["ㄌㄢ"] = "lan";
				DoubleCharChart["ㄌㄤ"] = "lang";
				DoubleCharChart["ㄌㄠ"] = "lao";
				DoubleCharChart["ㄌㄜ"] = "le";
				DoubleCharChart["ㄌㄟ"] = "lei";
				DoubleCharChart["ㄌㄥ"] = "leng";
				DoubleCharChart["ㄌㄧ"] = "li";
				DoubleCharChart["ㄌㄛ"] = "lo";
				DoubleCharChart["ㄌㄡ"] = "lou";
				DoubleCharChart["ㄌㄨ"] = "lu";
				DoubleCharChart["ㄌㄩ"] = "lü";
				DoubleCharChart["ㄇㄚ"] = "ma";
				DoubleCharChart["ㄇㄞ"] = "mai";
				DoubleCharChart["ㄇㄢ"] = "man";
				DoubleCharChart["ㄇㄤ"] = "mang";
				DoubleCharChart["ㄇㄠ"] = "mao";
				DoubleCharChart["ㄇㄜ"] = "me";
				DoubleCharChart["ㄇㄟ"] = "mei";
				DoubleCharChart["ㄇㄣ"] = "men";
				DoubleCharChart["ㄇㄥ"] = "meng";
				DoubleCharChart["ㄇㄧ"] = "mi";
				DoubleCharChart["ㄇㄛ"] = "mo";
				DoubleCharChart["ㄇㄡ"] = "mou";
				DoubleCharChart["ㄇㄨ"] = "mu";
				DoubleCharChart["ㄋㄚ"] = "na";
				DoubleCharChart["ㄋㄞ"] = "nai";
				DoubleCharChart["ㄋㄢ"] = "nan";
				DoubleCharChart["ㄋㄤ"] = "nang";
				DoubleCharChart["ㄋㄠ"] = "nao";
				DoubleCharChart["ㄋㄜ"] = "ne";
				DoubleCharChart["ㄋㄟ"] = "nei";
				DoubleCharChart["ㄋㄣ"] = "nen";
				DoubleCharChart["ㄋㄥ"] = "neng";
				DoubleCharChart["ㄋㄧ"] = "ni";
				DoubleCharChart["ㄋㄡ"] = "nou";
				DoubleCharChart["ㄋㄨ"] = "nu";
				DoubleCharChart["ㄋㄩ"] = "nü";
				DoubleCharChart["ㄆㄚ"] = "pa";
				DoubleCharChart["ㄆㄞ"] = "pai";
				DoubleCharChart["ㄆㄢ"] = "pan";
				DoubleCharChart["ㄆㄤ"] = "pang";
				DoubleCharChart["ㄆㄠ"] = "pao";
				DoubleCharChart["ㄆㄟ"] = "pei";
				DoubleCharChart["ㄆㄣ"] = "pen";
				DoubleCharChart["ㄆㄥ"] = "peng";
				DoubleCharChart["ㄆㄧ"] = "pi";
				DoubleCharChart["ㄆㄛ"] = "po";
				DoubleCharChart["ㄆㄡ"] = "pou";
				DoubleCharChart["ㄆㄨ"] = "pu";
				DoubleCharChart["ㄑㄧ"] = "qi";
				DoubleCharChart["ㄑㄩ"] = "qu";
				DoubleCharChart["ㄖㄢ"] = "ran";
				DoubleCharChart["ㄖㄤ"] = "rang";
				DoubleCharChart["ㄖㄠ"] = "rao";
				DoubleCharChart["ㄖㄜ"] = "re";
				DoubleCharChart["ㄖㄣ"] = "ren";
				DoubleCharChart["ㄖㄥ"] = "reng";
				DoubleCharChart["ㄖㄡ"] = "rou";
				DoubleCharChart["ㄖㄨ"] = "ru";
				DoubleCharChart["ㄙㄚ"] = "sa";
				DoubleCharChart["ㄙㄞ"] = "sai";
				DoubleCharChart["ㄙㄢ"] = "san";
				DoubleCharChart["ㄙㄤ"] = "sang";
				DoubleCharChart["ㄙㄠ"] = "sao";
				DoubleCharChart["ㄙㄜ"] = "se";
				DoubleCharChart["ㄙㄟ"] = "sei";
				DoubleCharChart["ㄙㄣ"] = "sen";
				DoubleCharChart["ㄙㄥ"] = "seng";
				DoubleCharChart["ㄕㄚ"] = "sha";
				DoubleCharChart["ㄕㄞ"] = "shai";
				DoubleCharChart["ㄕㄢ"] = "shan";
				DoubleCharChart["ㄕㄤ"] = "shang";
				DoubleCharChart["ㄕㄠ"] = "shao";
				DoubleCharChart["ㄕㄜ"] = "she";
				DoubleCharChart["ㄕㄟ"] = "shei";
				DoubleCharChart["ㄕㄣ"] = "shen";
				DoubleCharChart["ㄕㄥ"] = "sheng";
				DoubleCharChart["ㄕㄡ"] = "shou";
				DoubleCharChart["ㄕㄨ"] = "shu";
				DoubleCharChart["ㄙㄡ"] = "sou";
				DoubleCharChart["ㄙㄨ"] = "su";
				DoubleCharChart["ㄊㄚ"] = "ta";
				DoubleCharChart["ㄊㄞ"] = "tai";
				DoubleCharChart["ㄊㄢ"] = "tan";
				DoubleCharChart["ㄊㄤ"] = "tang";
				DoubleCharChart["ㄊㄠ"] = "tao";
				DoubleCharChart["ㄊㄜ"] = "te";
				DoubleCharChart["ㄊㄥ"] = "teng";
				DoubleCharChart["ㄊㄧ"] = "ti";
				DoubleCharChart["ㄊㄡ"] = "tou";
				DoubleCharChart["ㄊㄨ"] = "tu";
				DoubleCharChart["ㄨㄚ"] = "wa";
				DoubleCharChart["ㄨㄞ"] = "wai";
				DoubleCharChart["ㄨㄢ"] = "wan";
				DoubleCharChart["ㄨㄤ"] = "wang";
				DoubleCharChart["ㄨㄟ"] = "wei";
				DoubleCharChart["ㄨㄣ"] = "wen";
				DoubleCharChart["ㄨㄥ"] = "weng";
				DoubleCharChart["ㄨㄛ"] = "wo";
				DoubleCharChart["ㄒㄧ"] = "xi";
				DoubleCharChart["ㄒㄩ"] = "xu";
				DoubleCharChart["ㄧㄚ"] = "ya";
				DoubleCharChart["ㄧㄢ"] = "yan";
				DoubleCharChart["ㄧㄤ"] = "yang";
				DoubleCharChart["ㄧㄠ"] = "yao";
				DoubleCharChart["ㄧㄝ"] = "ye";
				DoubleCharChart["ㄧㄣ"] = "yin";
				DoubleCharChart["ㄧㄥ"] = "ying";
				DoubleCharChart["ㄩㄥ"] = "yong";
				DoubleCharChart["ㄧㄡ"] = "you";
				DoubleCharChart["ㄩㄢ"] = "yuan";
				DoubleCharChart["ㄩㄝ"] = "yue";
				DoubleCharChart["ㄩㄣ"] = "yun";
				DoubleCharChart["ㄗㄚ"] = "za";
				DoubleCharChart["ㄗㄞ"] = "zai";
				DoubleCharChart["ㄗㄢ"] = "zan";
				DoubleCharChart["ㄗㄤ"] = "zang";
				DoubleCharChart["ㄗㄠ"] = "zao";
				DoubleCharChart["ㄗㄜ"] = "ze";
				DoubleCharChart["ㄗㄟ"] = "zei";
				DoubleCharChart["ㄗㄣ"] = "zen";
				DoubleCharChart["ㄗㄥ"] = "zeng";
				DoubleCharChart["ㄓㄚ"] = "zha";
				DoubleCharChart["ㄓㄞ"] = "zhai";
				DoubleCharChart["ㄓㄢ"] = "zhan";
				DoubleCharChart["ㄓㄤ"] = "zhang";
				DoubleCharChart["ㄓㄠ"] = "zhao";
				DoubleCharChart["ㄓㄜ"] = "zhe";
				DoubleCharChart["ㄓㄟ"] = "zhei";
				DoubleCharChart["ㄓㄣ"] = "zhen";
				DoubleCharChart["ㄓㄥ"] = "zheng";
				DoubleCharChart["ㄓㄡ"] = "zhou";
				DoubleCharChart["ㄓㄨ"] = "zhu";
				DoubleCharChart["ㄗㄡ"] = "zou";
				DoubleCharChart["ㄗㄨ"] = "zu";

				// 1-Character
				SingleCharChart["ㄚ"] = "a";
				SingleCharChart["ㄞ"] = "ai";
				SingleCharChart["ㄢ"] = "an";
				SingleCharChart["ㄤ"] = "ang";
				SingleCharChart["ㄠ"] = "ao";
				SingleCharChart["ㄔ"] = "chi";
				SingleCharChart["ㄘ"] = "ci";
				SingleCharChart["ㄜ"] = "e";
				SingleCharChart["ㄟ"] = "ei";
				SingleCharChart["ㄣ"] = "en";
				SingleCharChart["ㄦ"] = "er";
				SingleCharChart["ㄡ"] = "ou";
				SingleCharChart["ㄖ"] = "ri";
				SingleCharChart["ㄕ"] = "shi";
				SingleCharChart["ㄙ"] = "si";
				SingleCharChart["ㄨ"] = "wu";
				SingleCharChart["ㄧ"] = "yi";
				SingleCharChart["ㄩ"] = "yu";
				SingleCharChart["ㄓ"] = "zhi";
				SingleCharChart["ㄗ"] = "zi";

				#endregion
			}

			[Pure]
			public string Process(string text)
			{
				// Do three-character syllables first
				text = TripleCharChart.Keys.Aggregate(text, (current, yoonString)
					=> current.Replace(yoonString, TripleCharChart[yoonString]));

				// Do two-character syllables second
				text = DoubleCharChart.Keys.Aggregate(text, (current, yoonString)
					=> current.Replace(yoonString, DoubleCharChart[yoonString]));

				// Do one-character syllables last
				text = SingleCharChart.Keys.Aggregate(text, (current, yoonString)
					=> current.Replace(yoonString, SingleCharChart[yoonString]));

				return text;
			}
		}
	}
}
