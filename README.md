# Extended Button
 This is a simple Unity package that extends the capabilities of the original Button

![example](https://ibb.co/VLWWSF6)

## Dependencies
- [DOTween](https://dotween.demigiant.com/documentation.php)

## Installation
1) Install [DOTween](https://dotween.demigiant.com/documentation.php)
2) Import this from Unity Package Manager. You can [download and import it from your hard drive](https://docs.unity3d.com/Manual/upm-ui-local.html), or [link to it from github directly](https://docs.unity3d.com/Manual/upm-ui-giturl.html).

```
https://github.com/RenKOFFF/ExtendedButton.git
```

## How to use
0) You can replace a regular Button with an ExtendedButton without losing serialized fields by calling the "Replace to ExtendedButton" method from the Button context menu
1) After adding the Extended Button component, you can use it like a regular button and, since it inherits from Button, you can use all the original methods and fields.
2) Use the Transitions field to configure the transformation logic. 
   - Existing transitions:
      - None - no transitions,
      - ImageColor,
      - ImageSize,
      - ImageSprite,
      - TextColor,
      - TextSize 
   ##### Transitions can be combined with each other in any desired way


## Other Information
- The Transition field has been replaced with Transitions and now the original field is not used and is hidden from the editor as unnecessary.
- Navigation block is cut out and not used