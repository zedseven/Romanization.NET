# Supported Languages and Systems
The goal of Romanization.NET is to provide a simple, extensive way to romanize widely-used languages as accurately as possible.

Below is a list of all supported languages and systems, with explanations of caveats and limitations if necessary.



## Chinese
### [Hànyǔ Pīnyīn](https://en.wikipedia.org/wiki/Pinyin)
The Hànyǔ Pīnyīn system is considered a [Readings System](readingsSystems.md), and supports all [Hànzì](https://en.wikipedia.org/wiki/Chinese_characters) characters in the [Unihan database](http://www.unicode.org/charts/unihan.html).

The [reading types](/api/Romanization.Chinese.HanyuPinyinSystem.ReadingTypes.html) to use can be specified, but default to using all of them.

The order in which readings are returned is as follows:
1. Hànyǔ Pīnyīn
2. Hànyǔ Pínlǜ - Hànyǔ Pīnyīn as it appeared in [Xiàndài Hànyǔ Pínlǜ Cídiǎn](https://www.unicode.org/reports/tr38/index.html#kHanyuPinlu)
3. XHC - Hànyǔ Pīnyīn as it appeared in [Xiàndài Hànyǔ Cídiǎn](https://www.unicode.org/reports/tr38/index.html#kXHC1983)



## Japanese
### [Modified Hepburn](https://en.wikipedia.org/wiki/Hepburn_romanization)
This system is a revised version of the romanization system first published by James Curtis Hepburn, and the one in most widespread use in Japan.

It only supports [kana](https://en.wikipedia.org/wiki/Kana) ([hiragana](https://en.wikipedia.org/wiki/Hiragana) and [katakana](https://en.wikipedia.org/wiki/Katakana)), not Kanji. See below for Kanji support.

This supports [syllabic n](https://en.wikipedia.org/wiki/Hepburn_romanization#Syllabic_n) (ん), [long consonants](https://en.wikipedia.org/wiki/Hepburn_romanization#Long_consonants) (sokuon, or っ), and [long vowels](https://en.wikipedia.org/wiki/Hepburn_romanization#Loanwords) (chōonpu (ー) only).

#### Limitations
In the Modified Hepburn system, certain pairs of subsequent vowels in the romanized result are [to be combined into single long vowels](https://en.wikipedia.org/wiki/Hepburn_romanization#Long_vowels), often indicated with a macron (aa => ā, for example).

The issue is, according to the spec for the system, these combinations depend on whether the two vowels belong to different morphemes - this is not something known to the program.
As a result, while some vowel combinations *could* be done (not all have this requirement), to remain consistent in output, no vowel combination is done.


### [Kanji (Kun & On) Readings](https://en.wikipedia.org/wiki/Kanji#Readings)
Kanji are effectively Japan's Hànzì, and share many of the same considerations and even symbols.

While kana are syllabaries (each character is one syllable, and therefore maps neatly to a distinct sound), Kanji are their own symbols that can be a variable number of syllables.
To make things more complicated, each can have multiple readings - in both [Kun'yomi](https://en.wikipedia.org/wiki/Kanji#Kun'yomi_(native_reading)) and [On'yomi](https://en.wikipedia.org/wiki/Kanji#On'yomi_(Sino-Japanese_reading)).

This is why this system is considered a [Readings System](readingsSystems.md) for the purposes of this library, which means you can get every known reading from the [Unihan database](http://www.unicode.org/charts/unihan.html) for each character.

The two reading types supported are:
1. Kun'yomi - often referred to as just Kun - the native reading
2. On'yomi - often referred to as just On - the Sino-Japanese reading

#### Additional Notes
Because Kanji often appear alongside supplementary kana, the system also has a small convenience function that romanizes both Kanji and Kana, using the system of your choice for kana.



## Korean
### [Revised Romanization of Korean](https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean)
The Revised Romanization of Korean system is the most commonly used, and does not make use of accents or macrons.

The system has a few provisions for certain kinds of content, which change the romanization somewhat:
- Certain special pairs of jamo are not combined in given names
- Whether or not aspiration is reflected in the romanization depends on whether or not the word is a noun
- Sometimes it can be helpful to hyphenate syllables, which occassionally makes a difference in disambiguating words with the same romanization (ga-eul vs. gae-ul)

The library's implementation of this system supports all of these provisions as options that can be supplied to the function.


### [Hanja](https://en.wikipedia.org/wiki/Hanja) => [Hangeul](https://en.wikipedia.org/wiki/Hangul) Readings
Hanja, like Kanji, came from China and share their symbols with Hànzì. As a result, this is also considered a [Readings System](readingsSystems.md) as some Hanja have multiple possible readings.

As with the other Hànzì-related characters, the supported Hanja are all from the [Unihan database](http://www.unicode.org/charts/unihan.html).

Only one reading type is supported, which is the Hangeul equivalent pronunciation for each Hanja character.

#### Additional Notes
Because the goal of this package is, as the name suggests, romanization, the implementation also includes a function for first converting the Hanja to Hangeul, then romanizing the Hangeul using the system of your choice.
