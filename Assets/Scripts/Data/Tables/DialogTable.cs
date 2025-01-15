#nullable enable
using System.Collections.Generic;

namespace Data.Tables
{
    public static class DialogTable // TODO refacto mettre les dialogs dans les actors / cutscenes
    {
        private static readonly Dialog DefaultDialog = new();

        private static readonly Dialog DefaultDialog2 = new()
        {
            Sequences = new Dialog.DialogSequence[]
            {
                new()
                {
                    PiecesOfText = new Dialog.PieceOfText[]
                    {
                        new() { Text = "Hello! I'm a test !" }
                    }
                },
                new()
                {
                    PiecesOfText = new Dialog.PieceOfText[]
                    {
                        new()
                        {
                            Text = "This is woobling oh yeah",
                            Color = Dialog.EPieceOfTextColor.BLUE,
                            Animation = Dialog.EPieceOfTextAnimation.WOOBLE
                        }
                    }
                },
                new()
                {
                    PiecesOfText = new Dialog.PieceOfText[]
                    {
                        new()
                        {
                            Text = "You should.. "
                        }
                    }
                },
                new()
                {
                    RevealSpeed = 1f,
                    PiecesOfText = new Dialog.PieceOfText[]
                    {
                        new()
                        {
                            Text = "run.",
                            Color = Dialog.EPieceOfTextColor.RED,
                            Animation = Dialog.EPieceOfTextAnimation.CREEPY
                        }
                    }
                }
            }
        };

        private static readonly Dictionary<int, Dialog> Data = new()
        {
            { 0, DefaultDialog },
            { 1, DefaultDialog2 }
        };

        public static Dialog? GetDialogById(int dialogId)
        {
            if (Data.TryGetValue(dialogId, out var dialogFound)) return dialogFound;

            return null;
        }
    }

    public class Dialog
    {
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
            public EDialogBackground Background = EDialogBackground.STANDARD;
            public bool CanBeSkipped = true; // TODO a tester
            public PieceOfTextChoice[] Choices = { }; // TODO
            public bool InstantText = false; // TODO a tester
            public PieceOfText[] PiecesOfText = { new() };
            public float RevealSpeed = 0.03f;
            public int RowCount = 1;
        }

        public class PieceOfText
        {
            public EPieceOfTextAnimation Animation = EPieceOfTextAnimation.NONE;
            public string Text = "Hello!";
            public virtual EPieceOfTextColor Color { get; set; } = EPieceOfTextColor.WHITE;
        }

        public class PieceOfTextChoice : PieceOfText
        {
            public int? NextDialogId = null;
            public override EPieceOfTextColor Color { get; set; } = EPieceOfTextColor.GREEN;
        }
    }
}