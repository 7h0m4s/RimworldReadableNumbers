# Rimworld Readable Numbers

## What does the mod do?
Makes numbers in Rimworld easier to read by adding digit separators.  
**e.g. 1000000 -> 1,000,000**

## Details

**Separators and Decimals can be any character you want:**

- **(1,000,000.00)** Comma separator with Period decimal **-Default-**
- **(1.000.000,00)** Period separator with Comma decimal
- **(1 000 000.00)** Space separator with Period decimal
- **(1 000 000,00)** Space separator with Comma decimal
- **(1'000'000.00)** Apostrophe separator with Period decimal
- **(1'000'000,00)** Apostrophe separator with Comma decimal
- **(1#000#000@00)** **Custom:** *Any* combination of 2 characters

**Choose Your Preferred Digit Grouping Style:**

- **(100,000,000,000)** Three Digits **-Default-**
- **(1,00,00,00,00,000)** Twos Then Three Digits
- **(1000,0000,0000)** Four Digits
- **(100000000000)** None

Formatting will be applied to most modded UI elements.

Can easily enable/disable number formatting mid-game from the Mod Settings.

A blacklist feature is available in settings to exclude any text that shouldn't be modified.

## F.A.Q.

**Q.** **Will this affect performance?**  
**A.** There should be no noticeable impact on performance. All processed text in game is cached for fast retrieval. Even without the cache, a lot of effort has been put in to optimising the formatting process to take as few CPU operations as possible and to quickly skip over any text that doesn't have enough sequential digits to need processing.

**Q.** **The mod is formatting a number I don't want it to! How can I fix this? In this case my pawn's name has lots of digits.**  
**A.** There is a blacklist available in the mod settings. Add all or part of the text that is incorrectly being modified. Then Readable Numbers will avoid formatting any text in game that contains those specific characters. WARNING: An exceptionally long blacklist might cause performance impact.

**Q.** **Was this vibe coded with AI?**  
**A.** No AI generated code was used in this project. Feel free to peruse the commits in the [Github](https://github.com/7h0m4s/RimworldReadableNumbers) to confirm. I got a lot of practice in optimising a simple task to be as performant as possible.

## Compatibility

This mod is safe to be added and removed at any point. As it does not make any changes to the save file.

There shouldn't be any clashes with the majority of mods so long as they don't make changes to how text is rendered on screen in game.

There is the possibility for a mod to encounter a visual bug when the slightly larger number text is now too big for the textbox. Causing the text to wrap around or become truncated. If this happens you can try adding a keyword or symbol in that textbox to the blacklist in this mod's settings. To have any text that matches to be excluded from formatting.

## Change Log

**1.1.0:**

- Tidied Mod Settings
- Expanded Separator And Decimal choices to include Apostrophe separator
- Added support for any characters to be used as Separator and Decimal
- Added Digit Grouping options
- Allowed mod to format the Decimal for numbers less than 1000 by default (e.g. 9.18kg -> 9,18kg)
- Added setting option to disable small number formatting
- Removed redundant Harmony.dll and PNG files
- Bug Fix: Quest Rewards not displaying $ value
- Bug Fix: Date year in History->Messages dialogue getting formatted

**1.0.0:**

- Mod Release
## License

GNU GENERAL PUBLIC LICENSE V3

**Github:** [https://github.com/7h0m4s/RimworldReadableNumbers](https://github.com/7h0m4s/RimworldReadableNumbers)