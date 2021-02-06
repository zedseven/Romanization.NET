using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization.LanguageAgnostic;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.InternalTests
{
	/// <summary>
	/// For testing the <see cref="CaseAwareSub"/> class.
	/// </summary>
	[TestClass]
	public class CaseAwareSubTests
	{
		/// <summary>
		/// Aims to test substitutions that use no <see cref="System.Text.RegularExpressions.Regex"/> captures.
		/// </summary>
		[TestMethod]
		public void NoCapturesTest()
		{
			CaseAwareSub sub0 = new CaseAwareSub("(?:\\bρ|(?<=ρ)ρ(?!\\b|ρ))", "rh");
			Assert.AreEqual("rhδδ",   sub0.Replace("ρδδ"));
			Assert.AreEqual("δρrhδ",  sub0.Replace("δρρδ"));
			Assert.AreEqual("δρρrhδ", sub0.Replace("δρρρδ"));
			Assert.AreEqual("δρρ",    sub0.Replace("δρρ"));
			Assert.AreEqual("δρρ ",   sub0.Replace("δρρ "));
			Assert.AreEqual("Rhδδ",   sub0.Replace("Ρδδ"));
			Assert.AreEqual("RHΔΔ",   sub0.Replace("ΡΔΔ"));
			Assert.AreEqual("ΔΡRHΔ",  sub0.Replace("ΔΡΡΔ"));
		}

		/// <summary>
		/// Aims to test substitutions that use nothing but captures.
		/// </summary>
		[TestMethod]
		public void CapturesOnlyTest()
		{
			// Using this class for a sub like this would be overkill and a waste of resources
			CaseAwareSub sub0 = new CaseAwareSub("(\\w{3})(\\w{3})", "$1$0");
			Assert.AreEqual("defabc", sub0.Replace("abcdef"));
		}

		/// <summary>
		/// Aims to test the behaviour of empty substitutions (removing the matches from the original string).
		/// </summary>
		[TestMethod]
		public void EmptySubstitutionTest()
		{
			// Using this class for a sub like this would be overkill and a waste of resources
			CaseAwareSub sub0 = new CaseAwareSub("abc", "");
			Assert.AreEqual("def", sub0.Replace("abcdef"));
		}

		/// <summary>
		/// Aims to test the behaviour of substitutions that use only one capture/sub pair.
		/// </summary>
		[TestMethod]
		public void SingleCaptureTest()
		{
			CaseAwareSub sub0 = new CaseAwareSub("(\\w)b", "$0z");
			Assert.AreEqual("azcd",  sub0.Replace("abcd"));
			Assert.AreEqual("Azcd",  sub0.Replace("Abcd"));
			Assert.AreEqual("AZcd",  sub0.Replace("ABcd"));
			Assert.AreEqual("AZCD",  sub0.Replace("ABCD"));

			CaseAwareSub sub1 = new CaseAwareSub("(\\w)b", "$0zz");
			Assert.AreEqual("azzcd", sub1.Replace("abcd"));
			Assert.AreEqual("Azzcd", sub1.Replace("Abcd"));
			Assert.AreEqual("AZzcd", sub1.Replace("ABcd"));
			Assert.AreEqual("AZZCD", sub1.Replace("ABCD"));
			Assert.AreEqual("azzCD", sub1.Replace("abCD"));
			Assert.AreEqual("aZZCD", sub1.Replace("aBCD"));

			CaseAwareSub sub2 = new CaseAwareSub("(\\w)bc", "$0zz");
			Assert.AreEqual("azzd",  sub2.Replace("abcd"));
			Assert.AreEqual("Azzd",  sub2.Replace("Abcd"));
			Assert.AreEqual("AZzd",  sub2.Replace("ABcd"));
			Assert.AreEqual("AZZD",  sub2.Replace("ABCD"));
			Assert.AreEqual("azZD",  sub2.Replace("abCD"));
			Assert.AreEqual("aZZD",  sub2.Replace("aBCD"));
		}

		/// <summary>
		/// Aims to test substitutions using multiple captures, substitutions, order changes, etc.
		/// </summary>
		[TestMethod]
		public void MultiCaptureTest()
		{
			CaseAwareSub sub0 = new CaseAwareSub("a(bcd)e(fg)hi", "z$0z$1zz");
			Assert.AreEqual("zbcdzfgzz",            sub0.Replace("abcdefghi"));
			Assert.AreEqual("Zbcdzfgzz",            sub0.Replace("Abcdefghi"));
			Assert.AreEqual("zBcdzfgzz",            sub0.Replace("aBcdefghi"));
			Assert.AreEqual("zbCdZFgzZ",            sub0.Replace("abCdEFghI"));

			CaseAwareSub sub1 = new CaseAwareSub("abc(def)ghi(jkl)mn(o)pq(rs)tuv", "zzzz$1z$0z$2zzz$3zz");
			Assert.AreEqual("zzzzjklzdefzozzzrszz", sub1.Replace("abcdefghijklmnopqrstuv"));
			// TODO: This shows behaviour that *could* be improved, but is likely completely unnecessary since this system is only designed for small substitutions
			Assert.AreEqual("ZzzzjKlzdEfzOzzzrSzz", sub1.Replace("AbCdEfGhIjKlMnOpQrStUv"));
			Assert.AreEqual("ZZZZjKlZdEfZOZZZrSZZ", sub1.Replace("AbCdEfGhIjKlMnOpQrStUV"));
			//Assert.AreEqual("ZZZZjKlZdEfZOzzZrSzz", sub1.Replace("AbCdEfGhIjKlMnOpQrStUv"));

			CaseAwareSub sub2 = new CaseAwareSub("αβγ(δεζ)ηθι(κλμ)νξ(ο)πρ(στ)υφχ", "zzzz$1z$0z$2zzz$3zz");
			Assert.AreEqual("ZzzzκΛμzδΕζzΟzzzσΤzz", sub2.Replace("ΑβΓδΕζΗθΙκΛμΝξΟπΡσΤυΦχ"));
		}
	}
}