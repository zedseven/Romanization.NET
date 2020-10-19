# Romanization.NET
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://api.travis-ci.org/zedseven/Romanization.NET.svg?branch=main)](https://travis-ci.org/zedseven/Romanization.NET)

A library for [romanization](https://en.wikipedia.org/wiki/Romanization) of widely-used languages using common romanization systems.

Still a work in progress. Originally made as part of the [NUSRipper](https://github.com/zedseven/NusRipper) project.

The languages currently supported are:
| Language       | Supported Romanization Systems                                         | Limitations |
| :------------: | :--------------------------------------------------------------------: | :---------: |
| Chinese        | [Hànyǔ Pīnyīn](https://en.wikipedia.org/wiki/Pinyin)                   |  |
| Japanese       | [Modified Hepburn](https://en.wikipedia.org/wiki/Hepburn_romanization), [Kanji (Kun & On)](https://en.wikipedia.org/wiki/Kanji#Readings) | Vowel combination is not done, as it requires knowledge of morpheme boundaries, which vary from word to word. |
| Korean         | [Revised Romanization of Korean](https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean), [Hanja Hangeul Readings](https://en.wikipedia.org/wiki/Hanja) |  |
