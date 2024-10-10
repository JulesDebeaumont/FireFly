#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogTable
{

    public static Dialog? GetDialogById(int dialogId)
    {
        if (Data.TryGetValue(dialogId, out Dialog dialogFound))
        {
            return dialogFound;
        }
        else
        {
            return null;
        }
    }

    private readonly static Dialog DefaultDialog = new Dialog();
    private readonly static Dialog DefaultDialog2 = new Dialog()
    {
        PiecesOfText = new List<Dialog.PieceOfText>
        {
            new ()
            {
                Text = "Hello! sdfgsdfg",
                Color = Dialog.PieceOfText.EPieceOfTextColor.RED,
                Animation = Dialog.PieceOfText.EPieceOfTextAnimation.CREEPY
            },
            new ()
            {
                Text = " la suite pas animé",
                Color = Dialog.PieceOfText.EPieceOfTextColor.YELLOW,
                Animation = Dialog.PieceOfText.EPieceOfTextAnimation.WOOBLE
            }
        },

    };

    private static Dictionary<int, Dialog> Data = new Dictionary<int, Dialog>
    {
        {0, DefaultDialog},
        {1, DefaultDialog2}
    };
}

public class Dialog
{
    public EDialogBackground Background = EDialogBackground.STANDARD;
    public bool InstantText = false;
    public bool CanBeSkipped = true;
    public float TextSpeed = 0.03f;
    public List<PieceOfText> PiecesOfText = new List<PieceOfText> { new PieceOfText() };
    public List<PieceOfTextChoice> Choices = new List<PieceOfTextChoice>{}; // TODO
    public int? WorldFlagIdToSet = null; // TODO ici ou alors dans l'actor ?
    public int RowCount = 1;
    public int? NextDialogId = null;

    public class PieceOfText
    {
        public string Text = "Hello!";
        public virtual EPieceOfTextColor Color { get; set;} = EPieceOfTextColor.WHITE;
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
            WOOBLE
        }
    }

    public class PieceOfTextChoice : PieceOfText
    {
        public override EPieceOfTextColor Color { get; set;} = EPieceOfTextColor.GREEN;
        public int? NextDialogId = null;
    }

    public enum EDialogBackground
    {
        STANDARD,
        NONE,
        WOOD
    }
}