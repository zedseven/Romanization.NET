namespace Romanization
{
	/// <summary>
	/// The type of a system - this is an important consideration depending on the purpose of romanizing the text.<br />
	/// For more information, visit:
	/// <a href='https://en.wikipedia.org/wiki/Romanization#Methods'>https://en.wikipedia.org/wiki/Romanization#Methods</a>
	/// </summary>
	public enum SystemType
	{
		/// <summary>
		/// A transliteration system, which is primarily concerned with maintaining an unambiguous map between
		/// the source language and the target script so it can be reconstructed from the romanized text later
		/// if need be.<br />
		/// This does not mean they aren't viable as romanization methods, but for ease of pronunciation a
		/// transcription system is the better choice.<br />
		/// Some languages only have transliteration systems.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Transliteration'>https://en.wikipedia.org/wiki/Transliteration</a>
		/// </summary>
		Transliteration,
		/// <summary>
		/// A phonemic transcription system, which intends to make the source text pronounceable to a reader of
		/// the target script.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Phonemic_orthography'>https://en.wikipedia.org/wiki/Phonemic_orthography</a>
		/// </summary>
		PhonemicTranscription,
		/// <summary>
		/// A phonetic transcription system, which attempts to record the sounds of the language, not necessarily
		/// making it easy to pronounce for a reader.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Phonetic_transcription'>https://en.wikipedia.org/wiki/Phonetic_transcription</a>
		/// </summary>
		PhoneticTranscription
	}
}
