using ProyectoOrdinario.Clases;
using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;

namespace ProyectoOrdinario
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Choose a game: ");
            Console.WriteLine("1. Poker");
            Console.WriteLine("2. 21 Blackjack");

            int choice = int.Parse(Console.ReadLine());

            IJuego selectedGame = choice switch
            {
                1 => new JuegoPoker(),
                2 => new BlackjackJuego(),
                _ => throw new ArgumentException("Invalid choice"),
            };

            Console.WriteLine("Enter the number of players: ");
            int numberOfPlayers = int.Parse(Console.ReadLine());

            List<IJugador> players = new List<IJugador>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                IDealer dealer = selectedGame.Dealer;
                IJugador player = choice switch
                {
                    1 => new JugadorPoker(dealer),
                    2 => new BlackjackJugador(dealer),
                    _ => throw new ArgumentException("Invalid choice"),
                };

                players.Add(player);
            }

            // Start the selected game
            foreach (var player in players)
            {
                selectedGame.AgregarJugador(player);
            }

            selectedGame.IniciarJuego();
            selectedGame.JugarRonda();
            selectedGame.MostrarGanador();
        }
    }
}
