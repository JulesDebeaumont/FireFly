#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Sign : EnvironmentActor
{
    private bool _isBroken = false;

    public int DialogId = 0; // sera dans l'interface, à surcharger


    void Awake()
    {
      //ReadInput(); // TODO mettre dans un manager, on va pas s'amuser à avoir des propriétés dans tout les actors
    }

    void FixedUpdate()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
          var dialogFOund = DialogTable.GetDialogById(DialogId);
          Debug.Log(dialogFOund);
          DialogManager.Instance.SetupDialog(dialogFOund);
      }
      
        // detect player near and in front of the sign and A press and _isBeingRead == false
        // Read()

        // if _isBeingRead == true && DialogManager.instance.CurrentDialog == null
        // {
        //     StopRead();
        // }
    }

    public void Read()
    {
        // mettre ça dans une interface qu'on fera hérite
        // faire aussi une interface pour la detecttion de player in front of
        
        //PlayerManager.instance.PlayerAction.SetupReadMode();
        //DialogManager.instance.ReadDialog(dialog);
    }

    public void StopRead()
    {
        // mettre ça dans une interface qu'on fera hérite
        //PlayerManager.instance.PlayerAction.ExitReadMode();
    }

    public void Break()
    {

    }

    // pour les npcs uniquement, pas les signs
    private void SetupDialogIdByWorldFlag()
    {

    }

}
