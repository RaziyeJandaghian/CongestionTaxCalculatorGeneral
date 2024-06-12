-- Vehicles Table
CREATE TABLE Vehicles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type NVARCHAR(50) NOT NULL
);

-- Toll Fees Table
CREATE TABLE TollFees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    City NVARCHAR(50) NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Fee INT NOT NULL
);

-- Toll Free Dates Table
CREATE TABLE TollFreeDates (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE NOT NULL,
    City NVARCHAR(50) NOT NULL
);

-- Sample Data for Gothenburg
INSERT INTO Vehicles (Type) VALUES ('Motorcycle'), ('Busses'), ('Emergency'), ('Diplomat'), ('Foreign'), ('Military'), ('Car');

INSERT INTO TollFees (City, StartTime, EndTime, Fee) VALUES 
('Gothenburg', '06:00', '06:29', 8),
('Gothenburg', '06:30', '06:59', 13),
('Gothenburg', '07:00', '07:59', 18),
('Gothenburg', '08:00', '08:29', 13),
('Gothenburg', '08:30', '14:59', 8),
('Gothenburg', '15:00', '15:29', 13),
('Gothenburg', '15:30', '16:59', 18),
('Gothenburg', '17:00', '17:59', 13),
('Gothenburg', '18:00', '18:29', 8),
('Gothenburg', '18:30', '05:59', 0);

INSERT INTO TollFreeDates (Date, City) VALUES 
('2013-01-01', 'Gothenburg'),
('2013-03-28', 'Gothenburg'),
('2013-03-29', 'Gothenburg'),
('2013-04-01', 'Gothenburg'),
('2013-04-30', 'Gothenburg'),
('2013-05-01', 'Gothenburg'),
('2013-05-08', 'Gothenburg'),
('2013-05-09', 'Gothenburg'),
('2013-06-05', 'Gothenburg'),
('2013-06-06', 'Gothenburg'),
('2013-06-21', 'Gothenburg'),
('2013-07-01', 'Gothenburg'),
('2013-11-01', 'Gothenburg'),
('2013-12-24', 'Gothenburg'),
('2013-12-25', 'Gothenburg'),
('2013-12-26', 'Gothenburg'),
('2013-12-31', 'Gothenburg');

;
