# Rimworld Readable Numbers

## [What does the mod do?]

Makes numbers in Rimworld easier to read by adding digit separators.
e.g. 1000000 -> 1,000,000

## [Details]
4 different formats to choose from:

- Comma separator with Period decimal 	(1,000,000.00) *-Default-*
- Period separator with Comma decimal 	(1.000.000,00)
- Space separator with Period decimal 	(1 000 000.00)
- Space separator with Comma decimal 	(1 000 000,00)

Formatting will be applied to most modded UI elements.

Can easily enabled/disabled number formatting mid-game from the Mod Settings.

A blacklist feature is available in settings to exclude for any text that shouldn't be modified.

## [F.A.Q.]
**Q.** Will this affect performance?

**A.** There should be no noticable impact on performance. All processed text in game is cached for fast retrieval. Even without the cache, a lot of effort has been put in to optimising the formatting process to take as few CPU operations as possible and to quickly skip over any text that doesn't have enough sequential digits to need processing.

---

**Q.** The mod is formatting a number I don't want it to! How can I fix this? In this case my pawn's name has lots of digits.

**A.** There is a blacklist available in the mod settings. Add all or part of the text that is incorrectly being modified. Then Readable Numbers will avoid formatting any text in game that contains those specific characters. WARNING: An exceptionally long blacklist might cause performance impact.

---

**Q.** Was this vibe coded with AI?

**A.** No AI generated code was used in this project. Feel free to peruse the commits in the [Github](https://github.com/7h0m4s/RimworldReadableNumbers) to confirm. I got a lot of practice in optimising a simple task to be as performant as possible.

## [Compatibility]
This mod is safe to be added and removed at any point. As it does not make any changes to the save file.

There shouldn't be any clashes with the majority of mods so long as they don't make changes to how text is rendered on screen in game.

There is the possibility for a mod to encounter a visual bug when the slightly larger number text is now too big for the textbox. Causing the text to wraparound or become truncated. If this happens you can try adding a keyword or symbol in that textbox to the blacklist in this mods settings. To have any text that matches to be excluded from formatting.


## [License]
GNU GENERAL PUBLIC LICENSE V3


**Github:** [https://github.com/7h0m4s/RimworldReadableNumbers](https://github.com/7h0m4s/RimworldReadableNumbers)