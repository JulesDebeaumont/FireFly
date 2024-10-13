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
    Sequences = new Dialog.DialogSequence[] {
      new ()
      {
        PiecesOfText = new Dialog.PieceOfText[]
        {
            new () { Text = "Hello! I'm a test !" }
        },
      },
      new ()
      {
        PiecesOfText = new Dialog.PieceOfText[]
        {
            new ()
            {
                Text = "This is woobling oh yeah",
                Color = Dialog.EPieceOfTextColor.BLUE,
                Animation = Dialog.EPieceOfTextAnimation.WOOBLE
            }
        },
      },
      new ()
      {
        PiecesOfText = new Dialog.PieceOfText[]
        {
          new ()
            {
                Text = "You should.. "
            }
        }
      },
      new ()
      {
        RevealSpeed = 1f,
        PiecesOfText = new Dialog.PieceOfText[]
        {
          new ()
          {
              Text = "run.",
              Color = Dialog.EPieceOfTextColor.RED,
              Animation = Dialog.EPieceOfTextAnimation.CREEPY
          }
        }
      }
    }
  };

  private static Dictionary<int, Dialog> Data = new Dictionary<int, Dialog>
    {
        {0, DefaultDialog},
        {1, DefaultDialog2}
    };
}

public class Dialog
{
  public DialogSequence[] Sequences = { new DialogSequence() };
  public class DialogSequence
  {
    public EDialogBackground Background = EDialogBackground.STANDARD;
    public bool InstantText = false; // TODO a tester
    public bool CanBeSkipped = true; // TODO a tester
    public float RevealSpeed = 0.03f;
    public PieceOfText[] PiecesOfText = { new PieceOfText() };
    public PieceOfTextChoice[] Choices = { }; // TODO
    public int? WorldFlagIdToSet = null; // TODO ici ou alors dans l'actor ?
    public int RowCount = 1;
  }

  public class PieceOfText
  {
    public string Text = "Hello!";
    public virtual EPieceOfTextColor Color { get; set; } = EPieceOfTextColor.WHITE;
    public EPieceOfTextAnimation Animation = EPieceOfTextAnimation.NONE;
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

  public enum EPieceOfTextAnimation
  {
    NONE,
    CREEPY,
    WOOBLE
  }

  public class PieceOfTextChoice : PieceOfText
  {
    public override EPieceOfTextColor Color { get; set; } = EPieceOfTextColor.GREEN;
    public int? NextDialogId = null;
  }

  public enum EDialogBackground
  {
    STANDARD,
    NONE,
    WOOD
  }
}