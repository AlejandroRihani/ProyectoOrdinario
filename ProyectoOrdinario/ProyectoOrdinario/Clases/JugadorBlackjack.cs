using ProyectoOrdinario.Enumeradores;
using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;

namespace ProyectoOrdinario.Clases
{
    internal class BlackjackJugador : IJugador
    {
        private List<ICarta> mano;
        private readonly IDealer dealer;
        public BlackjackJugador(IDealer dealer)
        {
            mano = new List<ICarta>();
            this.dealer = dealer;
        }

        public void RealizarJugada()
        {
            Console.WriteLine($"El jugador en Blackjack realiza una jugada:");

            // Simulate player's moves
            while (CalcularValorMano() < 17)
            {
                // Draw a card from the dealer
                ObtenerCartas(dealer.RepartirCartas(1));

                Console.WriteLine($"El jugador ha recibido una carta.");
                MostrarEstadoMano();
            }

            Console.WriteLine("El jugador ha finalizado su jugada.");
        }

        public void ObtenerCartas(List<ICarta> cartas)
        {
            mano.AddRange(cartas);
        }

        public ICarta DevolverCarta(int indiceCarta)
        {
            if (indiceCarta >= 0 && indiceCarta < mano.Count)
            {
                ICarta carta = mano[indiceCarta];
                mano.RemoveAt(indiceCarta);
                return carta;
            }
            else
            {
                Console.WriteLine("Índice de carta inválido.");
                return null;
            }
        }

        public List<ICarta> DevolverTodasLasCartas()
        {
            List<ICarta> cartas = new List<ICarta>(mano);
            mano.Clear();
            return cartas;
        }

        public List<ICarta> MostrarCartas()
        {
            return new List<ICarta>(mano);
        }

        public ICarta MostrarCarta(int indiceCarta)
        {
            if (indiceCarta >= 0 && indiceCarta < mano.Count)
            {
                return mano[indiceCarta];
            }
            else
            {
                Console.WriteLine("Índice de carta inválido.");
                return null;
            }
        }

        // Helper method to calculate the value of the player's hand
        private int CalcularValorMano()
        {
            // Implement the logic to calculate the value of the player's hand
            // based on the Blackjack rules. Adapt this according to your game's rules.
            int valor = 0;
            foreach (var carta in mano)
            {
                valor += ObtenerValorCarta(carta);
            }
            return valor;
        }

        // Helper method to get the value of a card
        private int ObtenerValorCarta(ICarta carta)
        {
            // Implement the logic to get the value of a card based on the Blackjack rules.
            // Adapt this according to your game's rules.
            switch (carta.Valor)
            {
                case ValoresCartasEnum.As:
                    return 11; // Assuming Ace value is 11, you can modify this based on rules
                case ValoresCartasEnum.Dos:
                case ValoresCartasEnum.Tres:
                case ValoresCartasEnum.Cuatro:
                case ValoresCartasEnum.Cinco:
                case ValoresCartasEnum.Seis:
                case ValoresCartasEnum.Siete:
                case ValoresCartasEnum.Ocho:
                case ValoresCartasEnum.Nueve:
                case ValoresCartasEnum.Diez:
                case ValoresCartasEnum.Jota:
                case ValoresCartasEnum.Reina:
                case ValoresCartasEnum.Rey:
                    return (int)carta.Valor;
                default:
                    return 0; // Handle other cases as needed
            }
        }

        // Helper method to display the current state of the player's hand
        private void MostrarEstadoMano()
        {
            Console.WriteLine("Estado de la mano del jugador:");
            foreach (var carta in mano)
            {
                Console.WriteLine($"   {carta.Valor} de {carta.Figura}");
            }
            Console.WriteLine($"Valor total de la mano: {CalcularValorMano()}");
        }
    }
}
