﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
//! Klasa odpowiedzialna za sterowanie postacia przy uzyciu pada
public class GamepadController : MonoBehaviour
{
  public CharacterController characterController; //<! kontroler postaci Unity
  public GameObject bombPrefab;                   //<! prefab bomby
  int cooldown = 0;                               //<! opoznienie w stawianiu bomby
  public Animator animator;                       //<! animator postaci
  public Camera camera;                           //<! kamera
  private Character character;                    //<! logika postaci
  public int speed;                               //<! szybkość poruszania się
  private void Awake()
  {
    // inicjalizacja logiki postaci
    character = gameObject.GetComponent<Character>();
  }

  /** Funkcja do obsługi ruchu postaci*/
  private void Move()
  {
    // sprawdzenie czy postac nie jest w trakcie animacja stawiania bomby
    if (animator.GetBool("isPlanting") == true)
      return;

    // inicjalizacja zmiennej czytającej pada
    var gamepad = Gamepad.current;

    // inicjalizacja wektora ruchu dwuwymiarowego i wpisanie mu wartosci z lewej galki 
    Vector2 movement;
    if (gamepad != null)
      movement = gamepad.leftStick.ReadValue();
    else
      movement = new Vector2(0, 0);

    // rozdzielenie wektora na dwie osie
    float horizontalMove = movement.x;
    float verticalMove = movement.y;

    // inicjalizcja wektora ruchu przy użyciu ustalonych zmiennych
    Vector3 move = new Vector3(1f * horizontalMove, 0f, 1f * verticalMove);

    // okreslenie rotacji
    if (move != Vector3.zero)
      transform.rotation = Quaternion.LookRotation(move) * Quaternion.Euler(0f, -90f, 0f);
    // "symulacja" grawitacji do sciagania gracza w dol po wejsciu na bombe
    move += transform.up * -1;
    // wykonanie ruchu przy uzyciu CharacterController
    characterController.Move(speed * Time.deltaTime * move);
    // przesuniecie kamery
    camera.transform.position = this.transform.position + new Vector3(0, 9, 0);
    // wlaczenie animacji chodzenia
    animator.SetBool("isWalking", horizontalMove != 0 || verticalMove != 0);
  }

  /** Funckja do obsługi stawiania bomby*/
  private void Bomb()
  {
    // inicjalizacja zmiennej czytającej pada
    var gamepad = Gamepad.current;
    if (gamepad == null)
      return;
    // jezeli wcisniety jest "poludniowy" przycisk (X/A), nie ma opoznienia i postac ma bomby
    if (gamepad.buttonSouth.wasPressedThisFrame && cooldown == 0 && character.bombs > 0)
    {
      // wyslanie do logiki postaci informacji o postawieniu bomby
      character.placeBomb();
      // wlaczenie animacji stawiania bomby
      animator.SetBool("isPlanting", true);
      // ustawienie opoznienia na ok. czas stawiania bomby
      cooldown = 150;
    }
    // ok. polowa czasu stawiania bomby
    if (cooldown == 75)
    {
	  //pobranie zasięgu i czasu życia bomby ze skryptu postaci
      int range = character.range;
      int bombLifetime = character.bombLifetime;
	  //obliczenie bazowego wektora (z float na int)
      Vector3Int intVector = new Vector3Int((int)this.transform.position.x, (int)this.transform.position.y, (int)this.transform.position.z);
	  //obliczenie wektora pozycji stawiającego bombę na środku pola
      Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
	  //stworzenie bomby
      GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
	  //przekazanie czasu życia
      bomb.GetComponent<Bomb>().maxLifetime = bombLifetime;
      //przekazanie zasięgu i czasów życia do obiektów odpowiadających za zadawanie obrażeń
      bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
      bomb.transform.GetChild(1).GetComponent<BombExplosion>().lifetime = bombLifetime;
      bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
      bomb.transform.GetChild(2).GetComponent<BombExplosion>().lifetime = bombLifetime;
      bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
      bomb.transform.GetChild(3).GetComponent<BombExplosion>().lifetime = bombLifetime;
	  //przekazanie wartości trójwymiarowości do bomby
      bomb.GetComponent<Bomb>().is3D = false;
	  //przekazanie zasięgu do bomby
      bomb.GetComponent<Bomb>().range = range;
    }
    if (cooldown != 0)
      cooldown--;
    // ok. koniec animacji
    if (cooldown == 0)
    {
      // wylaczenie animacji stawiania bomby
      animator.SetBool("isPlanting", false);
    }
  }

  // Update is called once per frame
  void Update()
  {
    Move();
    Bomb();
  }
}