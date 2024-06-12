# Congestion Tax Calculator General

## Overview

This Congestion Tax Calculator is a generic solution designed to calculate congestion tax for vehicles entering and exiting various cities. 
It provides flexibility to accommodate different tax rules and rates for different cities by using an external SQL database to store the tax rules and toll fees.

## Features
    Calculates congestion tax for vehicles entering and exiting cities based on their passes on a given day.
    Utilizes an external SQL database to store tax rules, toll fees, and toll-free dates for different cities.

## Usage
    Set up the SQL Server database using the provided SQL script.
    Customize the tax rules, toll fees, and toll-free dates in the database to match the requirements of the target city.
    Integrate the Congestion Tax Calculator into your application or system.
    Use the GetTax method to calculate the congestion tax for vehicles passing through the target city on a given day.
	
## Additional Notes
    This Congestion Tax Calculator is designed to be generic and adaptable to different cities with varying tax rules and rates.
    Ensure that the SQL Server database is properly set up and maintained to store accurate tax-related information for each city.
    Refer to the provided SQL script for setting up the required database tables and sample data. Update the data as needed for different cities.