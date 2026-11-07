# GitHub Copilot Instructions for CIWS_Core (Continued) RimWorld Mod

## Mod Overview and Purpose

CIWS_Core (Continued) is an update and continuation of the original CIWS_Core mod for the popular space colony simulation game RimWorld. This mod adds a variety of new items, recipes, and systems to enhance the gameplay experience by introducing futuristic technology and advanced medical treatments.

## Key Features and Systems

### New Items and Medicine
- **Biomimetic Components**: Includes nanorobots, language translation dentures, and intelligent bionic legs.
- **Medicine**: Introduced new powders like 37 powder and Baiyao powder, with unique medical properties.
- **Fruit and Plants**: Added pistachio, a fruit that provides significant happiness to colonists.
- **Articles**: Various new items such as helium 3 fuel tanks, compound fertilizers, and special production licenses.

### New Recipes
- **Vegetarian Roughage**: Allows for the production of vegan animal feed in batch, reducing the need for meat.

### Caravan System
- **Near Defense Trading Company**: A caravan and spaceship that can trade a wide range of goods, providing additional economic opportunities.

## Coding Patterns and Conventions

### C# Files
- **Structure**: The mod follows a modular structure with separate classes for different functionalities, such as medical effects and hediff components.
- **Naming**: Classes and methods use descriptive names to convey their purpose clearly.
- **Documentation**: Methods and classes are well-documented to help with understanding and maintaining the code.

### XML Integration
- **Defs**: Definitions for new items, recipes, and thoughts are defined in XML files.
- **Mod Compatibility**: The mod ensures compatibility with other RimWorld mods by following standard RimWorld def structures.

## Harmony Patching

The mod uses Harmony patching to extend or modify vanilla RimWorld behavior. Key patches include:
- **Medical Effects**: Custom medical effects for new items and powders.
- **Hediff Comp**: Custom hediff components to manage conditions like bio-age and other medical states.

## Suggestions for Copilot

### General Tips
- **Context**: When generating code or XML, provide context by specifying the purpose or feature you want to implement.
- **Examples**: If possible, provide examples of similar code or XML definitions to guide Copilot in generating accurate content.

### XML Generation
- **Def Structure**: When generating XML defs, ensure they follow the standard RimWorld def structure.
- **Attributes**: Include all necessary attributes and ensure they are correctly formatted.

### C# Code Generation
- **Class Structure**: When generating C# classes, follow the existing modular structure with clear separation of concerns.
- **Method Implementation**: Focus on implementing methods that accurately represent the desired functionality, especially for medical effects and hediff components.

### Harmony Patching
- **Patch Location**: Specify the location in the vanilla code where the patch should be applied.
- **Patch Behavior**: Clearly define the behavior of the patch, such as adding or modifying existing functionality.

By following these guidelines, Copilot can assist in generating high-quality code and XML definitions for the CIWS_Core (Continued) mod, ensuring a seamless integration with the RimWorld game.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

