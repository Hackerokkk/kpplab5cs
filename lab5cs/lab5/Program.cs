using System;
using System.Collections.Generic;
using System.IO;

// Class for representing route station data
[Serializable]
class RouteStation
{
    private string stationName;
    private string arrivalTime;
    private string departureTime;
    private int availableSeats;

    public RouteStation(string stationName, string arrivalTime, string departureTime, int availableSeats)
    {
        this.stationName = stationName;
        this.arrivalTime = arrivalTime;
        this.departureTime = departureTime;
        this.availableSeats = availableSeats;
    }

    public string StationName
    {
        get { return stationName; }
    }

    public string DepartureTime
    {
        get { return departureTime; }
    }

    // Other getters and setters

    public override string ToString()
    {
        return $"Station: {stationName} (Arrival: {arrivalTime}, Departure: {departureTime}, Available Seats: {availableSeats})";
    }
}

// Class for representing route data
[Serializable]
class Route
{
    private int routeNumber;
    private string daysOfWeek;
    private int totalSeats;
    private List<RouteStation> stations;

    public Route(int routeNumber, string daysOfWeek, int totalSeats)
    {
        this.routeNumber = routeNumber;
        this.daysOfWeek = daysOfWeek;
        this.totalSeats = totalSeats;
        this.stations = new List<RouteStation>();
    }

    public string DaysOfWeek
    {
        get { return daysOfWeek; }
    }

    public List<RouteStation> Stations
    {
        get { return stations; }
    }

    // Other getters and setters

    public void AddStation(RouteStation station)
    {
        stations.Add(station);
    }

    public override string ToString()
    {
        return $"Route: {routeNumber} (Days: {daysOfWeek}, Total Seats: {totalSeats})";
    }
}

// Generic container class
[Serializable]
class MyContainer<T>
{
    private List<T> elements;

    public MyContainer()
    {
        elements = new List<T>();
    }

    public void Add(T element)
    {
        elements.Add(element);
    }

    public void Remove(T element)
    {
        elements.Remove(element);
    }

    public void Clear()
    {
        elements.Clear();
    }

    public int Count
    {
        get { return elements.Count; }
    }

    public bool IsEmpty
    {
        get { return elements.Count == 0; }
    }

    public T Get(int index)
    {
        if (index >= 0 && index < elements.Count)
        {
            return elements[index];
        }
        return default(T);
    }

    public List<T> GetElements()
    {
        return elements;
    }
}

// Utility class for container processing
class ContainerUtil
{
    public static void ProcessContainer<T>(MyContainer<T> container)
    {
        List<T> elements = container.GetElements();
        foreach (T element in elements)
        {
            Console.WriteLine(element);
        }
    }
}

class MainClass
{
    public static void Main(string[] args)
    {
        MyContainer<Route> routeContainer = new MyContainer<Route>();

        // Adding routes to the container
        Route route1 = new Route(1, "Monday", 50);
        route1.AddStation(new RouteStation("Station A", "08:00", "08:30", 30));
        route1.AddStation(new RouteStation("Харків'", "18:00", "18:30", 20)); // Added a station with evening departure time
        routeContainer.Add(route1);

        Route route2 = new Route(2, "Tuesday", 40);
        route2.AddStation(new RouteStation("Station X", "10:00", "10:30", 15));
        route2.AddStation(new RouteStation("Station Y", "11:00", "11:30", 25));
        routeContainer.Add(route2);

        // Using foreach loop
        ContainerUtil.ProcessContainer(routeContainer);

        // Serialization
        using (FileStream outputStream = new FileStream("routes.ser", FileMode.Create))
        {
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(outputStream, routeContainer);
            Console.WriteLine("Route data saved to routes.ser");
        }

        // Deserialization
        using (FileStream inputStream = new FileStream("routes.ser", FileMode.Open))
        {
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MyContainer<Route> restoredContainer = (MyContainer<Route>)serializer.Deserialize(inputStream);
            Console.WriteLine("Route data restored from routes.ser");
            ContainerUtil.ProcessContainer(restoredContainer);
        }

        // Finding evening routes through Харків on Mondays and Fridays
        FindEveningRoutes(routeContainer);
    }

    private static void FindEveningRoutes(MyContainer<Route> container)
    {
        Console.WriteLine("Evening routes through Харків on Mondays and Fridays:");
        foreach (Route route in container.GetElements())
        {
            if (route.DaysOfWeek.Contains("Monday") || route.DaysOfWeek.Contains("Friday"))
            {
                bool hasKharkivTransit = false;
                foreach (RouteStation station in route.Stations)
                {
                    if (station.StationName.Equals("Харків'", StringComparison.OrdinalIgnoreCase) && station.DepartureTime.CompareTo("18:00") >= 0)
                    {
                        hasKharkivTransit = true;
                        break;
                    }
                }
                if (hasKharkivTransit)
                {
                    Console.WriteLine(route);
                }
            }
        }
    }
}
