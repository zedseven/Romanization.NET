using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.GreekTests.AncientTests
{
	/// <summary>
	/// For testing the Beta Code Greek romanization system, <see cref="Greek.Ancient.BetaCode"/>.<br />
	/// The large test strings are modified versions of a passage from the Almagest.
	/// </summary>
	[TestClass]
	public class BetaCodeTests
	{
		private readonly Greek.Ancient.BetaCode _system = new Greek.Ancient.BetaCode();

		/// <summary>
		/// Aims to test use of the system with common replacements.
		/// </summary>
		[TestMethod]
		public void ProcessCommonTest()
		{
			Assert.AreEqual("*ALE/CANDROS2 *G# O *MAKEDW/N", _system.Process("Αλέξανδρος Γʹ ο Μακεδών"));
			Assert.AreEqual("IA#. E(NDE/KATO/S2 E)S1TI PARA/LLHLOS2, KAQ' O(\\N A)\\N GE/NOITO H( MEGI/S1TH H(ME/RA " +
							"W(RW=N I)S1HMERINW=N I#D#∠##. A)PE/XEI D' OU(=TOS2 TOU= I)S1HMERINOU= MOI/RAS2 L#2# KAI\\ " +
							"GRA/FETAI DIA\\ ῾*RO/DOU. KAI/ E)S1TIN E)NTAU=QA, OI(/WN O( GNW/MWN C#, TOIOU/TWN H( ME\\N " +
							"QERINH\\ S1KIA\\ IB#∠##G##IB##, H( DE\\ I)S1HMERINH\\ MG∠##G##, H( DE\\ XEIMERINH\\ RG# G##",
				_system.Process("ιαʹ. ἑνδέκατός ἐστι παράλληλος, καθ’ ὃν ἂν γένοιτο ἡ μεγίστη ἡμέρα ὡρῶν ἰσημερινῶν " +
								"ιʹδʹ∠ʹʹ. ἀπέχει δ’ οὗτος τοῦ ἰσημερινοῦ μοίρας λϛʹ καὶ γράφεται διὰ ῾Ρόδου. καί ἐστιν " +
								"ἐνταῦθα, οἵων ὁ γνώμων ξʹ, τοιούτων ἡ μὲν θερινὴ σκιὰ ιβʹ∠ʹʹγʹʹιβʹʹ, ἡ δὲ ἰσημερινὴ " +
								"μγ∠ʹʹγʹʹ, ἡ δὲ χειμερινὴ ργʹ γʹʹ"));
		}
	}
}
