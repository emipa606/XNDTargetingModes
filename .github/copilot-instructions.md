# GitHub Copilot Instructions for [XND] Targeting Modes (Continued)

## Mod Overview and Purpose

The [XND] Targeting Modes (Continued) mod brings a strategic combat enhancement to RimWorld by integrating mechanics inspired by the V.A.T.S system from the Fallout universe. The mod allows players to define custom body part targeting priorities for their colonists, obedience-trained animals, and mechanoids. Each targeting mode comes with its own strategic benefits and trade-offs, creating an additional layer of combat strategy.

## Key Features and Systems

- **Targeting Modes**: Five distinct modes, each affecting accuracy and hit probability:
  - *General*: Default mode with no penalties.
  - *Head*: Prioritizes headshots with a significant accuracy penalty.
  - *Body*: Focuses on body hits with a minor accuracy penalty.
  - *Arms*: Targets arms to hinder manipulation with a moderate penalty.
  - *Legs*: Aims for legs to reduce movement capability with a moderate penalty.
- **Automatic Reset**: Targeting modes reset to 'General' after a period of inactivity in combat.
- **Enemy AI**: Raiders and some mechanoids can utilize these modes, providing smarter enemy behavior.
- **Customization**: Mod settings allow players to configure aspects like accuracy effects and the likelihood of enemy usage.

## Coding Patterns and Conventions

- **Classes and Naming**: Adherence to common C# naming conventions such as PascalCase for class names and camelCase for method parameters.
- **Modular Design**: Defined clear class responsibilities, such as `CompTargetingMode` for component-based enhancements and `TargetingModeDef` for definition of targeting modes.
- **Interface Usage**: Utilizes interfaces, like `ITargetModeSettable`, for defining contract-based implementations.
  
## XML Integration

- **Defs**: Utilize XML definition files to declare targeting modes (`TargetingModeDefOf`, `TargetingModeDef`), allowing easy expansion and configuration of modes through XML.

## Harmony Patching

- **Purpose and Usage**: Extensively utilizes the Harmony library for dynamically altering the functionality of the base game without modifying its original code.
- **Implemented Patches**: Includes patches for hitting parts during combat (`Patch_ChooseHitPart` across multiple damage types like `Cut`, `Stab`, etc.), enhancing combat logic.
  
## Suggestions for Copilot

- **Code Completion**: Use Copilot for auto-completing similar method structures and parameter patterns within mod-specific class files.
- **Patching Suggestions**: Leverage Copilot to suggest baseline Harmony patch structures when adding new functionality or fixing edge cases.
- **Error Handling**: Utilize Copilot for generating consistent and clear error handling practices, particularly in `Try-Catch` constructs.

By adhering to these guidelines, contributors can effectively enhance and maintain the mod, ensuring consistent functionality and compatibility with the ongoing development in RimWorld's community.

---
For more information, visit the [GitHub repository](Link to GitHub releases) or [join us on Discord](Link to Discord server).

This structured markdown document provides an overview specific to your modding project and serves as an informative guide for copilot usage and further development. Be sure to replace placeholders like "Link to GitHub releases" and "Link to Discord server" with actual URLs.
