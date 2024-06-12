using System;
using System.Collections.Generic;
using congestion.Vehicle;
using System.Data.SqlClient;

public class CongestionTaxCalculator
{


    private const int MaxDailyFee = 60; // Maximum daily fee
    private const int MaxTimeDifference = 60; // Maximum time difference in minutes
    private const string ConnectionString = "Server=congestiontax_server;Database=congestiontax_db;User Id=admin;Password=Calculator;"; // SQL Server connection string
    private Dictionary<string, HashSet<DateTime>> tollFreeDatesCache = new Dictionary<string, HashSet<DateTime>>(); // Dictionary to cache toll-free dates

    /**
         * Calculate the total toll fee for one day for a specific city
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @param city    - the city
         * @return - the total congestion tax for that day
         */

    public int GetTax(Vehicle vehicle, DateTime[] dates, string city)
    {
        if (dates == null || dates.Length == 0) return 0;

        Array.Sort(dates); // Ensure dates are sorted to apply single charge rule

        DateTime intervalStart = dates[0]; // Initialize the start of the interval

        int totalFee = 0; // Initialize the total fee
        
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle, city); // Get the toll fee for the current date and vehicle
            
            int tempFee = GetTollFee(intervalStart, vehicle, city); // Get the toll fee for the interval start date and vehicle

            double minutes = (date - intervalStart).TotalMinutes; // Calculate the time difference in minutes 

            // Check if the time difference is less than or equal to MaxTimeDifference
            if (minutes <= MaxTimeDifference)
            {
                // If there's already a fee, subtract the previous fee from the total fee
                if (totalFee > 0) totalFee -= tempFee;
                // If the next fee is higher than the current fee, update the current fee
                if (nextFee >= tempFee) tempFee = nextFee;
                // updated the total fee
                totalFee += tempFee;
            }
            else
            {
                // updated the total fee
                totalFee += nextFee;
                // Update the IntervalStart for the next iteration
                intervalStart = date;
            }

        }
        // Cap the total fee at MaxDailyFee
        return Math.Min(totalFee, MaxDailyFee);
    }

    /**
         * Check if the Vehicle is toll-free in a specific city
         *
         * @param vehicle - the vehicle
         * @param city    - the city
         * @return - the state of the vehicle
         */
    private bool IsTollFreeVehicle(Vehicle vehicle, string city)
    {
        if (vehicle == null) return false;

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT COUNT(*) FROM TaxRules WHERE City = @city AND VehicleType = @vehicleType AND IsTollFree = 1", connection);
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@vehicleType", vehicle.GetVehicleType());
            var count = (int)command.ExecuteScalar();
            connection.Close();
            return count > 0;
        }
    }


    /**
         * Get the toll fee for a specific date , vehicle, and city
         *
         * @param vehicle - the vehicle
         * @param date    - the date
         * @param city    - the city
         * @return - toll fee for a specific date and vehicle
         */
    public int GetTollFee(DateTime date, Vehicle vehicle, string city)
    {
        if (IsTollFreeDate(date, city) || IsTollFreeVehicle(vehicle, city)) return 0;

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT Fee FROM TollFees WHERE City = @city AND StartTime <= @time AND EndTime >= @time", connection);
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@time", date.TimeOfDay);
            var fee = (int)command.ExecuteScalar();
            connection.Close();
            return fee;
        }
    }
    /**
         * Check if the date is toll-free in a specific city
         *
         * @param date - the date
         * @param city - the city
         * @return - if it is a toll-free date
         */
    private bool IsTollFreeDate(DateTime date, string city)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (!tollFreeDatesCache.ContainsKey(city))
        {
            LoadTollFreeDates(city);
        }

        return tollFreeDatesCache[city].Contains(date.Date);
    }

    /**
         * Load toll-free dates for a specific city from the database
         *
         * @param city - the city
         */
    private void LoadTollFreeDates(string city)
    {
        var dates = new HashSet<DateTime>();
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT Date FROM TollFreeDates WHERE City = @city", connection);
            command.Parameters.AddWithValue("@city", city);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                dates.Add(reader.GetDateTime(0));
            }
            connection.Close();
        }
        tollFreeDatesCache[city] = dates;
    }
}