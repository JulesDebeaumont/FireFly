#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogTable
{

    public static Dialog? GetDialogById(int dialogId)
    {
      return DefaultDialog; // TODO FIX
        if (Data.TryGetValue(dialogId, out Dialog dialogFound))
        {
          return dialogFound;
        }
        else
        {
          return null;
        }
    }

    private static Dictionary<int, Dialog> Data = new Dictionary<int, Dialog>
    {
        {0, DefaultDialog}
    };

    private readonly static Dialog DefaultDialog = new Dialog();
    private readonly static Dialog DefaultDialog2 = new Dialog()
    {
        PiecesOfText = new List<Dialog.PieceOfText>
        {
            new ()
            {
                Text = "Hello! sdfgsdfg",
                Color = Dialog.PieceOfText.EPieceOfTextColor.RED,
                Animation = Dialog.PieceOfText.EPieceOfTextAnimation.NONE
            },
            new ()
            {
                Text = " la suite pas animé",
                Color = Dialog.PieceOfText.EPieceOfTextColor.YELLOW,
                Animation = Dialog.PieceOfText.EPieceOfTextAnimation.WOOBLE
            }
        }
    };
}

public class Dialog
{
    public EDialogBackground Background = EDialogBackground.STANDARD;
    public bool InstantText = false;
    public float TextSpeed = 0.03f;
    public List<PieceOfText> PiecesOfText = new List<PieceOfText> { new PieceOfText() };
    public int? WorldFlagToSet;
    public int RowCount = 0;
    public Dialog? NextDialog;
    // TODO choices

    public class PieceOfText
    {
        public string Text = "Hello!";
        public EPieceOfTextColor Color = EPieceOfTextColor.WHITE;
        public EPieceOfTextAnimation Animation = EPieceOfTextAnimation.NONE;

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

        public enum EPieceOfTextAnimation
        {
            NONE,
            CREEPY,
            SUSPENSE,
            WOOBLE
        }
    }

    public enum EDialogBackground
    {
        STANDARD,
        NONE,
        WOOD
    }
}