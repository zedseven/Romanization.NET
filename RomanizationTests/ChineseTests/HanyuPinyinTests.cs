using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.ChineseTests
{
	[TestClass]
	public class HanyuPinyinTests
	{
		private readonly Chinese.HanyuPinyin _system = new Chinese.HanyuPinyin();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                                                _system.Process(""));
			Assert.AreEqual("zǎojiàolèwán wǒdìtónghuàhuìběn xiǎohǎitùdìgùshì", _system.Process("早教乐园 我的童话绘本 小海兔的故事"));
			Assert.AreEqual("xiàndài hànyǔ pínshuài cídiǎn",                   _system.Process("現代 漢語 頻率 詞典"));
		}

		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("zǎo[jiào jiāo][lè yuè][wán yuán] wǒ[dì dí de][tóng zhōng][huà hua]huì[běn bēn] xiǎohǎi[tù tú chān][dì dí de]gù[shì zì shi]", _system.ProcessWithReadings("早教乐园 我的童话绘本 小海兔的故事").ToString());
			Assert.AreEqual("xiàndài [hàn tān][yǔ yù] [pín bīn][shuài lǜ lüe l̈ù] cí[diǎn tiǎn]", _system.ProcessWithReadings("現代 漢語 頻率 詞典").ToString());
		}

		[TestMethod]
		public void OutsideBasicMultilingualPlaneTest()
		{
			Assert.AreEqual("hánghánghánghángháng hánghánghánghángháng", _system.Process("𤼍𤼍𤼍𤼍𤼍 𤼍𤼍𤼍𤼍𤼍"));
			Assert.AreEqual("hángtónghángtóngháng tónghángtónghángtóng", _system.Process("𤼍童𤼍童𤼍 童𤼍童𤼍童"));
		}
	}
}