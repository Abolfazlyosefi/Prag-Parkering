using System;
using System.Text.RegularExpressions;


class PragueParking
{
    // Här skapar vi en endimisionell vektor för 100 parkeringsplatser
    static string[] parkingGarage = new string[100];


    static void Main(string[] args)
    {
        // Här skapar vi en main metod som loopar tills användaren avslutar programmet
        while (true)
        {
            //Visar huvudmeny
            ShowMenu();
            //Hanterar användarens val i menyn
            HandleMenuChoice();

        }
    }
    // Visar menyn för användaren
    static void ShowMenu()
    {
        Console.WriteLine("\nVälkommen till Prague parking! \n");
        Console.WriteLine("1. Parkera fordon");
        Console.WriteLine("2. Hämta fordon");
        Console.WriteLine("3. Flytta fordon");
        Console.WriteLine("4. Sök efter fordon");
        Console.WriteLine("5. Visa enskild parkeringsplats");
        Console.WriteLine("6. Visa hela parkeringshuset");
        Console.WriteLine("7. Avsluta");
    }
    // Läser in användarens val och därefter anropar en av metoderna
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
                PrintAllParkingSpots();
                break;

            case "7":
                EndProgram();
                break;

            default:
                Console.WriteLine("ogiltig val");
                break;
        }



        // Metod för att parkera fordon i garaget
        static void AddVehicle()
        {
            //Här får användaren mata in fordonstyp och regnummer
            string vehicleType = GetInput("\nAnge fordonstyp (CAR eller MC):").ToUpper();
            string regNumber = GetInput("\nAnge registreringsnummer: ").ToUpper();

            // Här kontrollerar vi att regnummret är mellan 6 till 10 tecken
            if (regNumber.Length < 6 || regNumber.Length > 10)
            {
                Console.WriteLine("\nRegistreringsnumret måste vara mellan 6 och 10 tecken långt.");
                Console.WriteLine("\n\n-------------------------\n\n");
                return;

            }

            //Kontrollerar att regnummret innehåller tillåtna tecken
            if (!Regex.IsMatch(regNumber, @"^[A-ZÅÄÖÜ0-9]+$"))
            {
                Console.WriteLine("\nRegistreringsnumret får endast innehålla bokstäver (A-Z, Å, Ä, Ö, Ü) och siffror.");
                Console.WriteLine("\n\n-------------------------\n\n");
                return;


            }

            // Kontrollerar om fordonstypen är CAR eller MC
            if (vehicleType == "CAR" || vehicleType == "MC")

            {
                //Hittar en ledig plats till fordnet och parkerar fordonet på platsen
                int freeSpot = FindFreeSpot(vehicleType);
                if (freeSpot != -1)
                {
                    ParkVehicle(freeSpot, vehicleType, regNumber);
                    Console.WriteLine($"\n{vehicleType} med registreringsnummer {regNumber} parkerades på plats {freeSpot + 1}");
                    Console.WriteLine("\n\n-------------------------\n\n");
                }
                else
                {
                    //Om ingen plats hittas
                    Console.WriteLine($"\nInga lediga för {vehicleType}.");
                    Console.WriteLine("\n\n-------------------------\n\n");

                }

            }
            else
            {
                //Om användaren skriver något annat än CAR eller MC
                Console.WriteLine("\nOgiltig fordonstyp");
                Console.WriteLine("\n\n-------------------------\n\n");

            }
        }

        //Parkerar fordon på en specifik plats
        static void ParkVehicle(int spot, string vehicleType, string regNumber)
        {
            //Om det finns en MC parkerad på en parkeringsplats så går det att parkera en till på samma plats
            if (vehicleType == "MC" && parkingGarage[spot]?.StartsWith("MC#") == true)
            {
                parkingGarage[spot] += $"|MC# {regNumber}";
            }
            else
            {
                //Parkerar ett nytt fordon på en ledig plats där typen av fordon anges plus regnummer
                parkingGarage[spot] = $"{vehicleType}#{regNumber}";
            }
        }


        //Hittar en ledig parkeringsplats
        static int FindFreeSpot(string vehicleType)
        {
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                //Om fordonet är en CAR så ska den parkeras på första lediga plats
                if (vehicleType == "CAR" && parkingGarage[i] == null)
                    return i;

                //Om det är en MC så ska den parkeras tillsammans med en annan MC, max 2 mc på en parkering
                if (vehicleType == "MC" && (parkingGarage[i] == null || (parkingGarage[i].StartsWith("MC#") && !parkingGarage[i].Contains("|"))))
                    return i;

            }
            return -1; // Ledig plats hittades inte
        }



    }

    //Flytta fordon från en till en annan parkeringsplats
    static void MoveVehicle()
    {
        //Frågar efter fordonets regnummer och letar efter den
        string regNumber = GetInput("\nAnge registreringsnummer på fordonet som ska flyttas:").ToUpper();
        int currentSpot = FindVehicle(regNumber);

        if (currentSpot != -1)
        {
            //Ber användaren att välja en ny P-plats
            int newSpot = int.Parse(GetInput($"Ange ny plats för fordonet (tillfälligt på plats {currentSpot + 1}): ")) - 1;

            //Om den nya platsen är giltig och ledig så flyttas fordonet annars utförs en av else metoderna
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

    //Tar bort ett fordon från garaget
    static void RemoveVehicle()
    {
        //Frågar efter regnummret på fordonet som ska tas bort
        string regNumber = GetInput("\nAnge registreringsnummer på fordonet som ska hämtas: ").ToUpper();
        int spot = FindVehicle(regNumber);

        if (spot != -1)
        {
            // Om det finns två MC parkerade så ska bara den angivna tas bort
            if (parkingGarage[spot].Contains("|"))
            {
                parkingGarage[spot] = RemoveMCFromSpot(spot, regNumber);
            }
            else
            {
                //Finns det bara ett fordon på platsen så ska platsen rensas
                ClearSpot(spot);
            }
            Console.WriteLine($"\nFordonet med registreringsnummer {regNumber} togs bort från plats {spot + 1}. ");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
        else
        {
            // Fordonet hittade inte
            Console.WriteLine("\nFordonet hittades inte. \n Var vänligen försök igen.");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
    }
    //Tar bort en specifik MC fårn en plats som har två MC
    static string RemoveMCFromSpot(int spot, string regNumber)
    {
        string[] Vehicles = parkingGarage[spot].Split('|');
        return string.Join("|", Array.FindAll(Vehicles, v => !v.Contains(regNumber)));

    }

    // Metod för att rensa parkeringsplats
    static void ClearSpot(int spot)
    {
        parkingGarage[spot] = null;
    }

    //letar efter fordon med hjälp av regnummer
    static int FindVehicle(string regNumber)
    {
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i]?.Contains(regNumber) == true)
                return i;
        }
        return -1;
    }

    //Metod för att hitta fordon 
    static void SearchVehicle()
    {
        string regNumber = GetInput("");
    }

    // Skriver ut om en specifik P-plats är full eller tom
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

    //Kollar om P-platsen är giltig
    static bool IsValidSpot(int spot)
    {
        return spot >= 0 && spot < parkingGarage.Length;
    }


    // Skriver ut info om alla P-platser
    static void PrintAllParkingSpots()
    {
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            Console.WriteLine(parkingGarage[i] == null ? $"Plats {i + 1} är tom. " : $"Plats {i + 1}: {parkingGarage[i]}");
            Console.WriteLine("\n\n-------------------------\n\n");


        }
    }

    //Här kollar vi en extra gån om användaren verkligen är klar och vill avsluta programmet
    static void EndProgram()
    {
        string shutdown = GetInput("Är du säker?\n");

        if (shutdown.Equals("nej", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("\n\n-------------------------\n\n");


        }
        else if (shutdown.Equals("ja", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("\nVälkommen åter, kör försiktigt!");
            Console.WriteLine("\n\n-------------------------\n\n");

            Environment.Exit(0);

        }
        else
        {
            Console.WriteLine("Ogiltigt val");
            Console.WriteLine("\n\n-------------------------\n\n");

        }
        return;
    }

    //Hämtar indata från användaren
    static string GetInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}