using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data.Utils
{
    public static class DialogLoader
    {
        private const string DialogFolder = "Dialogs";

        public static Dictionary<int, Dialog> GetDialogsFromFile(string fileName)
        {
            var jsonString = Resources.Load<TextAsset>(Path.Combine(DialogFolder, fileName)).text;
            if (jsonString == null)
            {
                return new Dictionary<int, Dialog> { { 0, new Dialog() } };
            }
            var arrayDialog = JsonUtility.FromJson<Dialog[]>(jsonString);
            return arrayDialog.ToDictionary(dialog => dialog.Id);
        }
    }
    
    public class Dialog
    {
        public int Id = 0;
        
        public enum EDialogBackground
        {
            STANDARD,
            NONE,
            WOOD
        }

        public enum EPieceOfTextAnimation
        {
            NONE,
            CREEPY,
            WOOBLE
        }

        public enum EPieceOfTextColor
        {
            WHITE,
            BLACK,
            RED,
            YELLOW,
            GREEN,
            BLUE,
            TRANSPARENT
        }

        public DialogSequence[] Sequences = { new() };

        public class DialogSequence
        {
            public bool AutoTransition = false; // TODO
            private readonly string _background;
            public bool CanBeSkipped = true; // TODO a tester
            public PieceOfTextChoice[] Choices = { }; // TODO
            public bool InstantText = false; // TODO a tester
            public PieceOfText[] PiecesOfText = { new() };
            public float RevealSpeed = 0.03f;
            public int RowCount = 1;
            public EDialogBackground Background => Enum.TryParse(_background, true, out EDialogBackground backgroundEnumed) ? backgroundEnumed : EDialogBackground.NONE;
        }

        public class PieceOfText
        {
            private readonly string _animation;
            public const string Text = "Hello!";
            protected string _color;
            public EPieceOfTextAnimation Animation => Enum.TryParse(_animation, true, out EPieceOfTextAnimation animationEnumed) ? animationEnumed : EPieceOfTextAnimation.NONE;
            public EPieceOfTextColor Color => Enum.TryParse(_color, true, out EPieceOfTextColor colorEnumed) ? colorEnumed : EPieceOfTextColor.WHITE;
        }

        public class PieceOfTextChoice : PieceOfText
        {
            public int? NextDialogId = null;
            public new EPieceOfTextColor Color => Enum.TryParse(_color, true, out EPieceOfTextColor colorEnumed) ? colorEnumed : EPieceOfTextColor.GREEN;
        }
    }
}