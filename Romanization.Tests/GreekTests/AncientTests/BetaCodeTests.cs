using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.GreekTests.AncientTests
{
	/// <summary>
	/// For testing the Beta Code Greek romanization system, <see cref="Greek.Ancient.BetaCode"/>.<br />
	/// The large test strings are modified versions of a passage from the Almagest.
	/// </summary>
	public class BetaCodeTests : TestClass
	{
		private readonly Greek.Ancient.BetaCode _systemCommon = new(false);
		private readonly Greek.Ancient.BetaCode _systemFull   = new(true);

		/// <summary>
		/// Aims to test use of the system with common replacements.
		/// </summary>
		[Fact]
		public void ProcessCommonTest()
		{
			Assert.Equal("*ALE/CANDROS2 *G# O *MAKEDW/N", _systemCommon.Process("Αλέξανδρος Γʹ ο Μακεδών"));
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI PARA/LLHLOS2, KAQ' O(\\N A)\\N GE/NOITO H( MEGI/S1TH H(ME/RA " +
							"W(RW=N I)S1HMERINW=N I#D#∠##. A)PE/XEI D' OU(=TOS2 TOU= I)S1HMERINOU= MOI/RAS2 L#2# " +
							"KAI\\ GRA/FETAI DIA\\ ῾*RO/DOU. KAI/ E)S1TIN E)NTAU=QA, OI(/WN O( GNW/MWN C#, TOIOU/TWN " +
							"H( ME\\N QERINH\\ S1KIA\\ IB#∠##G##IB##, H( DE\\ I)S1HMERINH\\ MG∠##G##, H( DE\\ " +
							"XEIMERINH\\ RG# G##",
				_systemCommon.Process("ιαʹ. ἑνδέκατός ἐστι παράλληλος, καθ’ ὃν ἂν γένοιτο ἡ μεγίστη ἡμέρα ὡρῶν " +
				                      "ἰσημερινῶν ιʹδʹ∠ʹʹ. ἀπέχει δ’ οὗτος τοῦ ἰσημερινοῦ μοίρας λϛʹ καὶ γράφεται " +
				                      "διὰ ῾Ρόδου. καί ἐστιν ἐνταῦθα, οἵων ὁ γνώμων ξʹ, τοιούτων ἡ μὲν θερινὴ σκιὰ " +
				                      "ιβʹ∠ʹʹγʹʹιβʹʹ, ἡ δὲ ἰσημερινὴ μγ∠ʹʹγʹʹ, ἡ δὲ χειμερινὴ ργʹ γʹʹ"));
		}

		/// <summary>
		/// Aims to test use of the system with all (full) replacements.
		/// </summary>
		[Fact]
		public void ProcessFullTest()
		{
			Assert.Equal("*ALE/CANDROS2 *G# O *MAKEDW/N", _systemFull.Process("Αλέξανδρος Γʹ ο Μακεδών"));
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI PARA/LLHLOS2, KAQ' O(\\N A)\\N GE/NOITO H( MEGI/S1TH H(ME/RA " +
							"W(RW=N I)S1HMERINW=N I#D##20##. A)PE/XEI D' OU(=TOS2 TOU= I)S1HMERINOU= MOI/RAS2 L#2# " +
							"KAI\\ GRA/FETAI DIA\\ ῾*RO/DOU. KAI/ E)S1TIN E)NTAU=QA, OI(/WN O( GNW/MWN C#, TOIOU/TWN " +
							"H( ME\\N QERINH\\ S1KIA\\ IB##20##G##IB##, H( DE\\ I)S1HMERINH\\ MG#20##G##, H( DE\\ " +
							"XEIMERINH\\ RG# G##",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι παράλληλος, καθ’ ὃν ἂν γένοιτο ἡ μεγίστη ἡμέρα ὡρῶν " +
				                    "ἰσημερινῶν ιʹδʹ∠ʹʹ. ἀπέχει δ’ οὗτος τοῦ ἰσημερινοῦ μοίρας λϛʹ καὶ γράφεται διὰ " +
				                    "῾Ρόδου. καί ἐστιν ἐνταῦθα, οἵων ὁ γνώμων ξʹ, τοιούτων ἡ μὲν θερινὴ σκιὰ " +
				                    "ιβʹ∠ʹʹγʹʹιβʹʹ, ἡ δὲ ἰσημερινὴ μγ∠ʹʹγʹʹ, ἡ δὲ χειμερινὴ ργʹ γʹʹ"));
		}

		/// <summary>
		/// Aims to test the variety of bracket types supported by Beta Code.
		/// </summary>
		[Fact]
		public void FullBracketsTest()
		{
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI [80PARA/LLHLOS2]80, KAQ' O(\\N A)\\N [81GE/NOITO]81 H( MEGI/S1TH " +
			             "H(ME/RA W(RW=N",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι /παράλληλος/, καθ’ ὃν ἂν //γένοιτο// ἡ μεγίστη ἡμέρα ὡρῶν"));
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI [84PARA/LLHLOS2]84, KAQ' O(\\N A)\\N [84GE/NOITO]84 H( MEGI/S1TH " +
			             "H(ME/RA W(RW=N",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι ⊂παράλληλος⊃, καθ’ ὃν ἂν ⊂γένοιτο⊃ ἡ μεγίστη ἡμέρα ὡρῶν"));
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI [85PARA/LLHLOS2]85, KAQ' O(\\N A)\\N [85GE/NOITO]85 H( MEGI/S1TH " +
			             "H(ME/RA W(RW=N",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι ((παράλληλος)), καθ’ ὃν ἂν ((γένοιτο)) ἡ μεγίστη ἡμέρα ὡρῶν"));
		}

		/// <summary>
		/// Aims to test the editorial brackets (editorial deletion bracket and editorial dittography bracket), as they
		/// both use the same characters so special treatment is required.
		/// </summary>
		[Fact]
		public void FullEditorialBrackets()
		{
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI [82PARA/LLHLOS2]82, KAQ' O(\\N A)\\N [83GE/NOITO]83 H( MEGI/S1TH " +
			             "H(ME/RA W(RW=N",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι ├παράλληλος┤, καθ’ ὃν ἂν ┤γένοιτο├ ἡ μεγίστη ἡμέρα ὡρῶν"));
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI [83PARA/LLHLOS2]83, KAQ' O(\\N A)\\N [82GE/NOITO]82 H( MEGI/S1TH " +
			             "H(ME/RA W(RW=N",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι ┤παράλληλος├, καθ’ ὃν ἂν ├γένοιτο┤ ἡ μεγίστη ἡμέρα ὡρῶν"));
			Assert.Equal("IA#. E(NDE/KATO/S2 E)S1TI [82PARA/LLHLOS2]82, [82KAQ']82 [83O(\\N]83 [82A)\\N]82 " +
			             "[83GE/NOITO]83 H( [83MEGI/S1TH]83 H(ME/RA W(RW=N",
				_systemFull.Process("ιαʹ. ἑνδέκατός ἐστι ├παράλληλος┤, ├καθ’┤ ┤ὃν├ ├ἂν┤ ┤γένοιτο├ ἡ ┤μεγίστη├ " +
				                    "ἡμέρα ὡρῶν"));
		}
	}
}
