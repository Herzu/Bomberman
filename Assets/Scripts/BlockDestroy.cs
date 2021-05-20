using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za zasygnalizowanie do GameControllera zniszczenia bloku w celu stworzenia powerupa
public class BlockDestroy : MonoBehaviour
{
    public GameController GameController;   //!< obiekt GameControllera
    private void OnDestroy()
    {
        if (GameController != null)
        {
			//dodanie powerupa do listy powerupów do stworzenia (w celu uniknięcia błędów wynikłych z intancjonowania obiektów w metodzie OnDestroy)
            GameController.AddPowerup(this.transform.position);
        }
    }
}
