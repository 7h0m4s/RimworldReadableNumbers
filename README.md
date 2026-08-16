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
- **(1,00,00,00,00,000)** Three Then Two Digits
- **(1000,0000,0000)** Four Digits
- **(100000000000)** None

Formatting will be applied to most modded UI elements.

Can easily be enabled/disabled via Mod Settings.

A blacklist feature is available in settings to exclude text that shouldn't be modified.

You can customise how long a number must be before formatting is applied.

## F.A.Q.

**Q.** **Will this affect performance?**  
**A.** 
* For years (for example 2026 -> 2,026), go to mod settings and increase the minimum number of digits a number must have to get separator formatting.
* For everything else such as pawn or item names (for example M1911 -> M,1911). There is a blacklist available in mod settings. Add all or part of the text that is incorrectly being modified. The mod will avoid formatting any text in game that contains those specific characters. WARNING: An exceptionally long blacklist might cause performance impact.

**Q.** **The mod is formatting a number I don't want it to! How can I fix this? In this case my pawn's name has lots of digits.**  
**A.** There is a blacklist available in the mod settings. Add all or part of the text that is incorrectly being modified. Then Readable Numbers will avoid formatting any text in game that contains those specific characters. WARNING: An exceptionally long blacklist might cause performance impact.

**Q.** **Was this vibe coded with AI?**  
**A.** No AI generated code was used in this project. Feel free to peruse the commits in the [Github](https://github.com/7h0m4s/RimworldReadableNumbers) to confirm. I got a lot of practice in optimising a simple task to be as performant as possible.

## Compatibility

This mod is safe to be added and removed at any point. As it does not make any changes to the save file.

There shouldn't be any clashes with the majority of mods so long as they don't make changes to how text is rendered on screen in game.

There is the possibility for a mod to encounter a visual bug when the slightly larger number text is now too big for the textbox. Causing the text to wrap around or become truncated. If this happens you can try adding a keyword or symbol in that textbox to the blacklist in this mod's settings. To have any text that matches to be excluded from formatting.

## Change Log
**1.2.0:**

- Improved support for all Mods. Should now be far less likely for text in modded screens to be displayed incorrectly.
- Added mod setting to set the minimum number of digets a number must have before sparators are added. Specifically to allow users to exclude years (e.g. 2,026) and weapon names (e.g. M1,911) from being formatted.
- Minor performance improvements

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

 ### [**Github**](https://github.com/7h0m4s/RimworldReadableNumbers)

 ### [**Ko-Fi**](https://github.com/7h0m4s/RimworldReadableNumbers)