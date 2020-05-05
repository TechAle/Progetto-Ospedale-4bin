/*
 * Nome: Program.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 04/05/2020 
 */
namespace ospedale
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            ospedale_cls ospedale = new ospedale_cls();

            ospedale.carica_csv();

        }
    }
}
