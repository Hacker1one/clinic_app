﻿-- Admin analytics queries
-- (1) Number of registered users per day for the timespan of a month
DECLARE @Year INT = 2023; -- Specify the given year
DECLARE @Month INT = 1;  -- Specify the month (e.g., 1 for January)
SELECT 
    DAY(RegistrationDate) AS [Day],
    COUNT(RegistrationDate) AS NumUsersInDay
FROM 
    [user]
WHERE 
    RegistrationDate >= DATEFROMPARTS(@Year, @Month, 1) AND 
    RegistrationDate < DATEADD(MONTH, 1, DATEFROMPARTS(@Year, @Month, 1))
GROUP BY 
    RegistrationDate
ORDER BY 
    [Day];


-- (2) Number of appointments over the span of a specific month
DECLARE @Year INT = 2024; -- Specify the given year
DECLARE @Month INT = 11;  -- Specify the month (e.g., 11 for Novemeber)
SELECT 
    DAY(DatenTime) AS [Day],
    COUNT(DatenTime) AS NumAppInDay
FROM 
    Appointment
WHERE 
    DatenTime >= DATEFROMPARTS(@Year, @Month, 1) AND 
    DatenTime < DATEADD(MONTH, 1, DATEFROMPARTS(@Year, @Month, 1))
GROUP BY 
    DatenTime
ORDER BY 
    [Day];


-- (3) We can add additional filters to the code above,
-- Such as only show confirmed or finished appointments:
DECLARE @Year INT = 2024; -- Specify the given year
DECLARE @Month INT = 11;  -- Specify the month (e.g., 1 for January)
SELECT 
    DAY(DatenTime) AS [Day],
    COUNT(DatenTime) AS NumAppInDay
FROM 
    Appointment
WHERE 
    DatenTime >= DATEFROMPARTS(@Year, @Month, 1) AND 
    DatenTime < DATEADD(MONTH, 1, DATEFROMPARTS(@Year, @Month, 1)) AND
    IsConfirmed = 1 AND
    IsFinished = 1
GROUP BY 
    DatenTime
ORDER BY 
    [Day];


-- Doctor analytics
-- (1) Number of doctors we have in the app in a specific medical field.
-- an assumption was made here that we only want to view unbanned doctors.
Select FieldOfMedicine.FieldName, Count(Doctor.ID) as NumberOfDoctors
from Doctor JOIN FieldOfMedicine
on Doctor.FieldCode = FieldOfMedicine.FieldCode
where Doctor.Banned = 0
Group BY
FieldOfMedicine.FieldName


-- (2) Price Per appointment frequency
Select PricePA, Count(ID) as Frequency
from Doctor
group by PricePA


-- Login and sign up
-- (1) Login
DECLARE @inp_email VARCHAR(50) = 'john.doe@example.com'; -- Input email
DECLARE @inp_password VARCHAR(40) = 'password123'; -- Input password
-- The above declarations are just for the sake of demonstration
SELECT 
    CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM [user]
            WHERE [user].email = @inp_email 
              AND [user].[password] = @inp_password
        ) THEN 1
        ELSE 0
END AS UserExists;
-- This returns a boolean value 1/0 depending on if the email/password
-- match the users in our database or not.





-- (2) Signup
-- These declarations are generated by an LLM.
-- In the web app we'll get these values from the user
-- and preprocess them before beginning the transaction.

DECLARE @FName VARCHAR(20) = 'Yassin';
DECLARE @LName VARCHAR(20) = 'ElBedwihy';
DECLARE @SSN bigINT = 30507122105034;
DECLARE @RegistrationDate DATE = GETDATE(); 
DECLARE @Gender CHAR = 'M';
DECLARE @Password VARCHAR(40) = 'yasssooo';
DECLARE @BirthDate DATE = '2005-01-01';
DECLARE @City VARCHAR(30) = 'El Sheikh Zayed';
DECLARE @Governorate VARCHAR(30) = 'Giza';
DECLARE @Email VARCHAR(50) = 'yassin.elbedwihy@example.com';


BEGIN TRANSACTION;
-- Insert into [user] table
INSERT INTO [user] (
    FName, LName, SSN, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email
)
VALUES (
    @FName, @LName, @SSN, @RegistrationDate, @Gender, @Password, @BirthDate, @City, @Governorate, @Email
);

-- Retrieve the newly generated IDENTITY value for the [user] table
DECLARE @NewUserID INT = SCOPE_IDENTITY();

-- Insert into Patient table with PenaltyFees = 0, And SSNValidation = 0;
INSERT INTO Patient (ID, SSNValidation, PenaltyFees)
VALUES (@NewUserID, 0, 0);

COMMIT TRANSACTION;
-- END




