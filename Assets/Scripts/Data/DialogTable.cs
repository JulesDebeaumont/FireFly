using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTable
{

    public Dialog GetDialogById(int dialogId)
    {
        Data.TryGetValue(dialogId, out Dialog dialogFound);
        return dialogFound;
    }

    private Dictionary<int, Dialog> Data = new Dictionary<int, Dialog>
    {
        {0, DefaultDialog}
    };

    private readonly static Dialog DefaultDialog = new Dialog();
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
            BLUE
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