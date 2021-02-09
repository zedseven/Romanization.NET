using Romanization.Internal;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.InternalTests
{
	/// <summary>
	/// For testing the <see cref="CaseAwareSub"/> class.
	/// </summary>
	public class CaseAwareSubTests : TestClass
	{
		/// <summary>
		/// Aims to test substitutions that use no <see cref="System.Text.RegularExpressions.Regex"/> captures.
		/// </summary>
		[Fact]
		public void NoCapturesTest()
		{
			CaseAwareSub sub0 = new("(?:\\bρ|(?<=ρ)ρ(?!\\b|ρ))", "rh");
			Assert.Equal("rhδδ",   sub0.Replace("ρδδ"));
			Assert.Equal("δρrhδ",  sub0.Replace("δρρδ"));
			Assert.Equal("δρρrhδ", sub0.Replace("δρρρδ"));
			Assert.Equal("δρρ",    sub0.Replace("δρρ"));
			Assert.Equal("δρρ ",   sub0.Replace("δρρ "));
			Assert.Equal("Rhδδ",   sub0.Replace("Ρδδ"));
			Assert.Equal("RHΔΔ",   sub0.Replace("ΡΔΔ"));
			Assert.Equal("ΔΡRHΔ",  sub0.Replace("ΔΡΡΔ"));
		}

		/// <summary>
		/// Aims to test substitutions that use nothing but captures.
		/// </summary>
		[Fact]
		public void CapturesOnlyTest()
		{
			// Using this class for a sub like this would be overkill and a waste of resources
			CaseAwareSub sub0 = new("(\\w{3})(\\w{3})", "$1$0");
			Assert.Equal("defabc", sub0.Replace("abcdef"));
		}

		/// <summary>
		/// Aims to test the behaviour of empty substitutions (removing the matches from the original string).
		/// </summary>
		[Fact]
		public void EmptySubstitutionTest()
		{
			// Using this class for a sub like this would be overkill and a waste of resources
			CaseAwareSub sub0 = new("abc", "");
			Assert.Equal("def", sub0.Replace("abcdef"));
		}

		/// <summary>
		/// Aims to test the behaviour of substitutions that use only one capture/sub pair.
		/// </summary>
		[Fact]
		public void SingleCaptureTest()
		{
			CaseAwareSub sub0 = new("(\\w)b", "$0z");
			Assert.Equal("azcd",  sub0.Replace("abcd"));
			Assert.Equal("Azcd",  sub0.Replace("Abcd"));
			Assert.Equal("AZcd",  sub0.Replace("ABcd"));
			Assert.Equal("AZCD",  sub0.Replace("ABCD"));

			CaseAwareSub sub1 = new("(\\w)b", "$0zz");
			Assert.Equal("azzcd", sub1.Replace("abcd"));
			Assert.Equal("Azzcd", sub1.Replace("Abcd"));
			Assert.Equal("AZzcd", sub1.Replace("ABcd"));
			Assert.Equal("AZZCD", sub1.Replace("ABCD"));
			Assert.Equal("azzCD", sub1.Replace("abCD"));
			Assert.Equal("aZZCD", sub1.Replace("aBCD"));

			CaseAwareSub sub2 = new("(\\w)bc", "$0zz");
			Assert.Equal("azzd",  sub2.Replace("abcd"));
			Assert.Equal("Azzd",  sub2.Replace("Abcd"));
			Assert.Equal("AZzd",  sub2.Replace("ABcd"));
			Assert.Equal("AZZD",  sub2.Replace("ABCD"));
			Assert.Equal("azZD",  sub2.Replace("abCD"));
			Assert.Equal("aZZD",  sub2.Replace("aBCD"));
		}

		/// <summary>
		/// Aims to test substitutions using multiple captures, substitutions, order changes, etc.
		/// </summary>
		[Fact]
		public void MultiCaptureTest()
		{
			CaseAwareSub sub0 = new("a(bcd)e(fg)hi", "z$0z$1zz");
			Assert.Equal("zbcdzfgzz",            sub0.Replace("abcdefghi"));
			Assert.Equal("Zbcdzfgzz",            sub0.Replace("Abcdefghi"));
			Assert.Equal("zBcdzfgzz",            sub0.Replace("aBcdefghi"));
			Assert.Equal("zbCdZFgzZ",            sub0.Replace("abCdEFghI"));

			CaseAwareSub sub1 = new("abc(def)ghi(jkl)mn(o)pq(rs)tuv", "zzzz$1z$0z$2zzz$3zz");
			Assert.Equal("zzzzjklzdefzozzzrszz", sub1.Replace("abcdefghijklmnopqrstuv"));
			// TODO: This shows behaviour that *could* be improved, but is likely completely unnecessary since this system is only designed for small substitutions
			Assert.Equal("ZzzzjKlzdEfzOzzzrSzz", sub1.Replace("AbCdEfGhIjKlMnOpQrStUv"));
			Assert.Equal("ZZZZjKlZdEfZOZZZrSZZ", sub1.Replace("AbCdEfGhIjKlMnOpQrStUV"));
			//Assert.Equal("ZZZZjKlZdEfZOzzZrSzz", sub1.Replace("AbCdEfGhIjKlMnOpQrStUv"));

			CaseAwareSub sub2 = new("αβγ(δεζ)ηθι(κλμ)νξ(ο)πρ(στ)υφχ", "zzzz$1z$0z$2zzz$3zz");
			Assert.Equal("ZzzzκΛμzδΕζzΟzzzσΤzz", sub2.Replace("ΑβΓδΕζΗθΙκΛμΝξΟπΡσΤυΦχ"));
		}
	}
}
