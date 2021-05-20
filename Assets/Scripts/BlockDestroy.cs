using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//klasa odpowiedzialna za zasygnalizowanie do GameControllera zniszczenia bloku w celu stworzenia powerupa
public class BlockDestroy : MonoBehaviour
{
    
	//klasa GameControllera
    public GameController GameController;
    private void OnDestroy()
    {
        if (GameController != null)
        {
			//dodanie powerupa do listy powerupów do stworzenia (w celu uniknięcia błędów wynikłych z intancjonowania obiektów w metodzie OnDestroy)
            GameController.AddPowerup(this.transform.position);
        }
    }
}
