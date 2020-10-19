using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.ChineseTests
{
	[TestClass]
	public class HanyuPinyinSystemTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("", Chinese.HanyuPinyin.Value.Process(""));
			Assert.AreEqual("zǎojiàolèwán wǒdìtónghuàhuìběn xiǎohǎitùdìgùshì", Chinese.HanyuPinyin.Value.Process("早教乐园 我的童话绘本 小海兔的故事"));
			Assert.AreEqual("xiàndài hànyǔ pínshuài cídiǎn", Chinese.HanyuPinyin.Value.Process("現代 漢語 頻率 詞典"));
		}

		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("zǎo[jiào jiāo][lè yuè][wán yuán] wǒ[dì dí de][tóng zhōng][huà hua]huì[běn bēn] xiǎohǎi[tù tú chān][dì dí de]gù[shì zì shi]", Chinese.HanyuPinyin.Value.ProcessWithReadings("早教乐园 我的童话绘本 小海兔的故事").ToString());
			Assert.AreEqual("xiàndài [hàn tān][yǔ yù] [pín bīn][shuài lǜ lüe l̈ù] cí[diǎn tiǎn]", Chinese.HanyuPinyin.Value.ProcessWithReadings("現代 漢語 頻率 詞典").ToString());
		}

		[TestMethod]
		public void OutsideBasicMultilingualPlaneTest()
		{
			Assert.AreEqual("hánghánghánghángháng hánghánghánghángháng", Chinese.HanyuPinyin.Value.Process("𤼍𤼍𤼍𤼍𤼍 𤼍𤼍𤼍𤼍𤼍"));
			Assert.AreEqual("hángtónghángtóngháng tónghángtónghángtóng", Chinese.HanyuPinyin.Value.Process("𤼍童𤼍童𤼍 童𤼍童𤼍童"));
		}
	}
}