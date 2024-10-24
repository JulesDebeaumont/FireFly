using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://youtu.be/yAsdUJtg4uQ?t=1103
public class PauseMenuManager : MonoBehaviour
{
  public bool IsMenuActive = false;
  private Vector2 _cursorPosition = new { 0f, 0f };
  private ItemMenu? _itemMenuOn = null;
  private float _toggleMenuDuration = 1f;

  private readonly _defaultBgColor = new Color(1f, 1f, 1f, 0.5f);
  private readonly ItemMenu[] _itemMenus = {
    new ItemMenu {
      Name = "Test Item",
      Description = "Test description lorem lipsum maleficia truc",
      CursorPosition = new Vector2(0f, 0f),
    }, 
  }

  public void EnablePauseMenu()
  {
    Time.timeScale = 0;
    // UI Element
  }

  public void DisablePauseMenu()
  {
    Time.timeScale = 1;
    // UI Element
  }

  private class ItemMenu
  {
    public string Name;
    public string Description;
    public Vector2 CursorPosition;
    
    public void AssignToCButton()
    {

    }
  }

}

