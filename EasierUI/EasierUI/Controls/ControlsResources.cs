using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EasierUI
{

    public static class ControlsResources
    {  

        public struct Resources
        {
            public Sprite standard;

            public Sprite background;

            public Sprite childBackground;

            public Sprite inputField;

            public Sprite knob;

            public Sprite checkmark;

            public Sprite dropdown;

            public Sprite mask;

            public TMP_FontAsset font;
        }
        
        public static TMP_DefaultControls.Resources ConvertToTMP(Resources resources) {
            return new TMP_DefaultControls.Resources {
                standard = resources.standard,
                background = resources.background,
                inputField = resources.inputField,
                knob = resources.knob,
                checkmark = resources.checkmark,
                dropdown = resources.dropdown,
                mask = resources.mask
            };
        }

        public static DefaultControls.Resources ConvertToDefault(Resources resources) {
            return new DefaultControls.Resources {
                standard = resources.standard,
                background = resources.background,
                inputField = resources.inputField,
                knob = resources.knob,
                checkmark = resources.checkmark,
                dropdown = resources.dropdown,
                mask = resources.mask
            };
        }

    }

}