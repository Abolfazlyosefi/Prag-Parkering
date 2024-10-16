using System;

class PragueParking
{
    static string[] parkingGarage = new string[100];


    static void Main(string[] args)
    {

        while (true)
        {
            ShowMenu();
            HandleMenuChoice();

        }
    }
    static void ShowMenu()
    {
        Console.WriteLine("\nVälkommen till Prague parking! \n");
        Console.WriteLine("1. Parkera fordon");
        Console.WriteLine("2. Hämta fordon");
        Console.WriteLine("3. Flytta fordon");
        Console.WriteLine("4. Sök efter fordon");
        Console.WriteLine("5. Visa enskild parkeringsplats");
        Console.WriteLine("6. Avsluta");
    }
    static void HandleMenuChoice()
    {

        switch (Console.ReadLine())
        {
            case "1":
                AddVehicle();
                break;

            case "2":
                RemoveVehicle();
                break;

            case "3":
                MoveVehicle();
                break;

            case "4":
                SearchVehicle();
                break;

            case "5":
                PrintParkingSpot();
                break;

            case "6":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("ogiltig val");
                break;
        }




        static void AddVehicle()
        {
            string vehicleType = GetInput("\nAnge fordonstyp (CAR eller MC):").ToUpper();
            string regNumber = GetInput("\nAnge registreringsnummer: ").ToUpper();

            if (regNumber.Length < 6 || regNumber.Length > 10)
            {
                Console.WriteLine("\nRegistreringsnumret måste vara mellan 6 och 10 tecken långt.");
                Console.WriteLine("\n\n-------------------------\n\n");
                return;

            }





            if (vehicleType == "CAR" || vehicleType == "MC")

            {
                int freeSpot = FindFreeSpot(vehicleType);
                if (freeSpot != -1)
                {
                    ParkVehicle(freeSpot, vehicleType, regNumber);
                    Console.WriteLine($"\n{vehicleType} med registreringsnummer {regNumber} parkerades på plats {freeSpot + 1}");
                    Console.WriteLine("\n\n-------------------------\n\n");
                }
                else
                {
                    Console.WriteLine($"\nInga lediga för {vehicleType}.");
                    Console.WriteLine("\n\n-------------------------\n\n");

                }

            }
            else
            {
                Console.WriteLine("\nOgiltig fordonstyp");
                Console.WriteLine("\n\n-------------------------\n\n");

            }
        }

        static void ParkVehicle(int spot, string vehicleType, string regNumber)
        {
            if (vehicleType == "MC" && parkingGarage[spot]?.StartsWith("MC#") == true)
            {
                parkingGarage[spot] += $"|MC# {regNumber}";
            }
            else
            {
                parkingGarage[spot] = $"{vehicleType}#{regNumber}";
            }
        }



        static int FindFreeSpot(string vehicleType)
        {
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (vehicleType == "CAR" && parkingGarage[i] == null)
                    return i;

                if (vehicleType == "MC" && (parkingGarage[i] == null || (parkingGarage[i].StartsWith("MC#") && !parkingGarage[i].Contains("|"))))
                    return i;

            }
            return -1;
        }



    }
    static void MoveVehicle()
    {
        string regNumber = GetInput("\nAnge registreringsnummer på fordonet som ska flyttas:").ToUpper();
        int currentSpot = FindVehicle(regNumber);

        if (currentSpot != -1)
        {
            int newSpot = int.Parse(GetInput($"Ange ny plats för fordonet (tillfälligt på plats {currentSpot + 1}): ")) - 1;

            if (IsValidSpot(newSpot) && parkingGarage[newSpot] == null)
            {
                parkingGarage[newSpot] = parkingGarage[currentSpot];
                ClearSpot(currentSpot);
                Console.WriteLine($"\nFordonet flyttades till plats {newSpot + 1}");
                Console.WriteLine("\n\n-------------------------\n\n");

            }
            else
            {
                Console.WriteLine("\nDen nya platsen är antingen ogiltig eller upptagen.\n\n Vänligen försök igen.");
                Console.WriteLine("\n\n-------------------------\n\n");

            }
        }
        else
        {
            Console.WriteLine("\nFordonet hittades inte. \n\n Vänligen försök igen.");
            Console.WriteLine("\n\n-------------------------\n\n");

        }

    }

    static void RemoveVehicle()
    {
        string regNumber = GetInput("\nAnge registreringsnummer på fordonet som ska hämtas: ").ToUpper();
        int spot = FindVehicle(regNumber);

        if (spot != -1)
        {
            if (parkingGarage[spot].Contains("|"))
            {
                parkingGarage[spot] = RemoveMCFromSpot(spot, regNumber);
            }
            else
            {
                ClearSpot(spot);
            }
            Console.WriteLine($"\nFordonet med registreringsnummer {regNumber} togs bort från plats {spot + 1}. ");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
        else
        {
            Console.WriteLine("\nFordonet hittades inte. \n Var vänligen försök igen.");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
    }

    static string RemoveMCFromSpot(int spot, string regNumber)
    {
        string[] Vehicles = parkingGarage[spot].Split('|');
        return string.Join("|", Array.FindAll(Vehicles, v => !v.Contains(regNumber)));

    }


    static void ClearSpot(int spot)
    {
        parkingGarage[spot] = null;
    }

    static int FindVehicle(string regNumber)
    {
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i]?.Contains(regNumber) == true)
                return i;
        }
        return -1;
    }

    static void SearchVehicle()
    {
        string regNumber = GetInput("");
    }


    static void PrintParkingSpot()
    {
        int spot = int.Parse(GetInput("\nAnge parkeringsplatsnummer 1-100:")) - 1;
        if (IsValidSpot(spot))
        {
            Console.WriteLine(parkingGarage[spot] == null ? $"Plats {spot + 1} är tom." : $"Plats {spot + 1}: {parkingGarage[spot]}");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
        else
        {
            Console.WriteLine("\nOgiltig parkeringsnummer, försök igen.");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
    }

    static bool IsValidSpot(int spot)
    {
        return spot >= 0 && spot < parkingGarage.Length;
    }




    static string GetInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}



