using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.ChineseTests
{
	/// <summary>
	/// For testing the Chinese Hànyǔ Pīnyīn romanization system, <see cref="Chinese.HanyuPinyin"/>.
	/// </summary>
	[TestClass]
	public class HanyuPinyinTests
	{
		private readonly Chinese.HanyuPinyin _system = new Chinese.HanyuPinyin();

		/// <summary>
		/// Aims to test quick readings where *a* reading of each character is used. (the first in the Unihan database
		/// for the character)
		/// </summary>
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                                                _system.Process(""));
			Assert.AreEqual("zǎojiàolèwán wǒdìtónghuàhuìběn xiǎohǎitùdìgùshì", _system.Process("早教乐园 我的童话绘本 小海兔的故事"));
			Assert.AreEqual("xiàndài hànyǔ pínshuài cídiǎn",                   _system.Process("現代 漢語 頻率 詞典"));
		}

		/// <summary>
		/// Aims to test processing with all readings for every character.<br />
		/// This test does not demonstrate realistic use of this system.
		/// </summary>
		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("zǎo[jiào jiāo][lè yuè][wán yuán] wǒ[dì dí de][tóng zhōng][huà hua]huì[běn bēn] xiǎohǎi[tù tú chān][dì dí de]gù[shì zì shi]", _system.ProcessWithReadings("早教乐园 我的童话绘本 小海兔的故事").ToString());
			Assert.AreEqual("xiàndài [hàn tān][yǔ yù] [pín bīn][shuài lǜ lüe l̈ù] cí[diǎn tiǎn]", _system.ProcessWithReadings("現代 漢語 頻率 詞典").ToString());
		}

		/// <summary>
		/// Aims to test characters that are outside the Unicode Basic Multilingual Plane (BMP), and therefore take more
		/// than one <see cref="char"/> to encode.
		/// </summary>
		[TestMethod]
		public void OutsideBasicMultilingualPlaneTest()
		{
			Assert.AreEqual("hánghánghánghángháng hánghánghánghángháng", _system.Process("𤼍𤼍𤼍𤼍𤼍 𤼍𤼍𤼍𤼍𤼍"));
			Assert.AreEqual("hángtónghángtóngháng tónghángtónghángtóng", _system.Process("𤼍童𤼍童𤼍 童𤼍童𤼍童"));
		}
	}
}
