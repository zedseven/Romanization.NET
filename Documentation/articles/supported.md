# Supported Languages and Systems
The goal of Romanization.NET is to provide a simple, extensive way to romanize widely-used languages as accurately as possible.

Below is a list of all supported languages and systems, with explanations of caveats and limitations if necessary. Languages are ordered lexicographically ascending.



## Chinese
### [Hànyǔ Pīnyīn](https://en.wikipedia.org/wiki/Pinyin)
The Hànyǔ Pīnyīn system is considered a [Readings System](readingsSystems), and supports all [Hànzì](https://en.wikipedia.org/wiki/Chinese_characters) characters in the [Unihan database](http://www.unicode.org/charts/unihan.html).

The [reading types](/api/Romanization.Chinese.HanyuPinyinSystem.ReadingTypes.html) to use can be specified, but default to using all of them.

The order in which readings are returned is as follows:
1. Hànyǔ Pīnyīn
2. Hànyǔ Pínlǜ - Hànyǔ Pīnyīn as it appeared in [Xiàndài Hànyǔ Pínlǜ Cídiǎn](https://www.unicode.org/reports/tr38/index.html#kHanyuPinlu)
3. XHC - Hànyǔ Pīnyīn as it appeared in [Xiàndài Hànyǔ Cídiǎn](https://www.unicode.org/reports/tr38/index.html#kXHC1983)



## Japanese
### [Modified Hepburn](https://en.wikipedia.org/wiki/Hepburn_romanization)
This system is a revised version of the romanization system first published by James Curtis Hepburn, and the one in most widespread use in Japan.

It only supports [Kana](https://en.wikipedia.org/wiki/Kana) ([Hiragana](https://en.wikipedia.org/wiki/Hiragana) and [Katakana](https://en.wikipedia.org/wiki/Katakana)), not Kanji. See below for Kanji support.

This supports [syllabic n](https://en.wikipedia.org/wiki/Hepburn_romanization#Syllabic_n) (ん), [long consonants](https://en.wikipedia.org/wiki/Hepburn_romanization#Long_consonants) (sokuon, or っ), and [long vowels](https://en.wikipedia.org/wiki/Hepburn_romanization#Loanwords) (chōonpu (ー) only).

#### Limitations
In the Modified Hepburn system, certain pairs of subsequent vowels in the romanized result are [to be combined into single long vowels](https://en.wikipedia.org/wiki/Hepburn_romanization#Long_vowels), often indicated with a macron (aa => ā, for example).

The issue is, according to the spec for the system, these combinations depend on whether the two vowels belong to different morphemes - this is not something known to the program.
As a result, while some vowel combinations *could* be done (not all have this requirement), to remain consistent in output, no vowel combination is done.


### [Kanji (Kun & On) Readings](https://en.wikipedia.org/wiki/Kanji#Readings)
Kanji are effectively Japan's Hànzì, and share many of the same considerations and even symbols.

While Kana are syllabaries (each character is one syllable, and therefore maps neatly to a distinct sound), Kanji are their own symbols that can be a variable number of syllables.
To make things more complicated, each can have multiple readings - in both [Kun'yomi](https://en.wikipedia.org/wiki/Kanji#Kun'yomi_(native_reading)) and [On'yomi](https://en.wikipedia.org/wiki/Kanji#On'yomi_(Sino-Japanese_reading)).

This is why this system is considered a [Readings System](readingsSystems) for the purposes of this library, which means you can get every known reading from the [Unihan database](http://www.unicode.org/charts/unihan.html) for each character.

The two reading types supported are:
1. Kun'yomi - often referred to as just Kun - the native reading
2. On'yomi - often referred to as just On - the Sino-Japanese reading

#### Additional Notes
Because Kanji often appear alongside supplementary Kana, the system also has a small convenience function that romanizes both Kanji and Kana, using the system of your choice for Kana.



## Korean
### [Revised Romanization of Korean](https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean)
The Revised Romanization of Korean system is the most commonly used, and does not make use of accents or macrons.

The system has a few provisions for certain kinds of content, which change the romanization somewhat:
- Certain special pairs of Jamo are not combined in given names
- Whether or not aspiration is reflected in the romanization depends on whether or not the word is a noun
- Sometimes it can be helpful to hyphenate syllables, which occassionally makes a difference in disambiguating words with the same romanization (ga-eul vs. gae-ul)

The library's implementation of this system supports all of these provisions as options that can be supplied to the function.


### [Hanja](https://en.wikipedia.org/wiki/Hanja) => [Hangeul](https://en.wikipedia.org/wiki/Hangul) Readings
Hanja, like Kanji, came from China and share their symbols with Hànzì. As a result, this is also considered a [Readings System](readingsSystems) as some Hanja have multiple possible readings.

As with the other Hànzì-related characters, the supported Hanja are all from the [Unihan database](http://www.unicode.org/charts/unihan.html).

Only one reading type is supported, which is the Hangeul equivalent pronunciation for each Hanja character.

#### Additional Notes
Because the goal of this package is, as the name suggests, romanization, the implementation also includes a function for first converting the Hanja to Hangeul, then romanizing the Hangeul using the system of your choice.



## Russian
At the time of writing, Russian has no single international standard of romanization/transliteration. Instead different systems are used by different groups for different purposes. As a result, there are many systems all implemented with very similar transliterations.

### [BGN/PCGN](https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian)
Developed jointly by the Unites States Board on Geographic Names and the Permanent Committee on Geographical Names for British Official Use, it is designed to be easier for anglophones to pronounce.

Because of this, it's likely a solid choice for romanizing text specifically for English speakers (US/CA/UK audience).


### [GOST 7.79-2000 System A](https://en.wikipedia.org/wiki/GOST_7.79-2000) / [ISO 9](https://en.wikipedia.org/wiki/ISO_9)
GOST 7.79-2000(A) focuses on mapping one Cyrillic character to one Latin character, potentially with diacritics.

ISO 9:1995 is the current standard for Slavic transliteration from the ISO, and is based on ISO/R 9:1968.

The two systems are functionally identical and in this library are combined into one, under the name of GOST 7.79-2000 System A. This is to retain consistency with the other GOST systems included, as it may be strange to have GOST 7.79-2000 System B but have A under a different name.


### [GOST 7.79-2000 System B](https://en.wikipedia.org/wiki/GOST_7.79-2000)
In contrast to the above, GOST 7.79-2000(B) focuses on mapping one Cyrillic character to potentially several Latin characters (eg. `щ -> shh`), but without the use of diacritics.


### [GOST 16876-71 Table 1 (UNGEGN)](https://en.wikipedia.org/wiki/GOST_16876-71)
GOST 16876-71(1) focuses on mapping one Cyrillic character to one Latin character, potentially with diacritics.

It was recommended by the [United Nations Group of Experts on Geographical Names (UNGEGN)](https://en.wikipedia.org/wiki/United_Nations_Group_of_Experts_on_Geographical_Names) in 1987.

GOST 16876-71 was most recently updated in 1980, and was abandoned in favour of GOST 7.79-2000 in 2002 by the Russian Federation.


### [GOST 16876-71 Table 2](https://en.wikipedia.org/wiki/GOST_16876-71)
GOST 16876-71(2) is another table in GOST 16876-71, and focuses on mapping one Cyrillic character to potentially several Latin characters (eg. `щ -> shh`), but without the use of diacritics.


### [Scholarly/Scientific Transliteration](https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic)
The Scholarly transliteration system for Russian actually covers many slavic languages, with Russian being one of them. It tries to preserve pronunciation of the original characters while remaining unambiguous about it's transformations.


### [ISO Recommendation No. 9 (ISO/R 9:1968)](https://en.wikipedia.org/wiki/ISO_9#ISO/R_9)
Similar to the scholarly system, ISO/R 9 was created 1954 and updated in 1968. It also supports many Slavic languages, and was the ISO's earliest adoption of scholarly transliteration.


### [American Library Association and Library of Congress (ALA-LC) System](https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian)
This system was initially established in 1904, and remains largely unchanged since 1941. It's primary purpose is in US, Canadian, and British libraries.

This system uses some diacritics and uses two-letter tie characters for some Cyrillic characters.


### [British Standard 2979:1958](https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard)
It is the main system of Oxford University Press, and was used by the British Library up until 1975.

The ALA-LC system is now used by the British Library instead.


### [ICAO Doc 9303](https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf)
Created by the International Civil Aviation Organization, a UN agency, the document is designed to make travel documents machine-readable.

It contains tables for transliteration to Latin characters from many alphabets, including Cyrillic. The system uses no diacritics whatsoever, only standard ASCII characters.

The system was put into effect by the Russian government in 2013 for all citizen passports.


### [General Road Signs](https://en.wikipedia.org/wiki/Romanization_of_Russian#Road_signs_note)
This is the system generally used for romanization for road signs and the like.

This originally followed GOST 10807-78 (tables 17, 18), but now follows GOST R 52290-2004 (tables Г.4, Г.5).
