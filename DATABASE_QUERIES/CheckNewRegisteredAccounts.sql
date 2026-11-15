-- Query to check new registered accounts with profile details
-- This query retrieves all new accounts with their contact information, location, and message

SELECT 
    Id AS UserId,
    UserName,
    Email,
    NormalizedEmail,
    PhoneNumber,
    ContactDetails,
    CurrentLocation,
    Message,
    CreatedDate,
    EmailConfirmed,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnabled,
    AccessFailedCount,
    ConcurrencyStamp
FROM AspNetUsers
ORDER BY CreatedDate DESC;

-- Alternative: Get only recent registrations (last 7 days)
/*
SELECT 
    Id AS UserId,
    UserName,
    Email,
    ContactDetails,
    CurrentLocation,
    Message,
    CreatedDate,
    EmailConfirmed
FROM AspNetUsers
WHERE CreatedDate >= DATEADD(DAY, -7, GETUTCDATE())
ORDER BY CreatedDate DESC;
*/

-- Alternative: Get unconfirmed email accounts
/*
SELECT 
    Id AS UserId,
    UserName,
    Email,
    ContactDetails,
    CurrentLocation,
    Message,
    CreatedDate,
    EmailConfirmed
FROM AspNetUsers
WHERE EmailConfirmed = 0
ORDER BY CreatedDate DESC;
*/

-- Alternative: Search by location
/*
SELECT 
    Id AS UserId,
    UserName,
    Email,
    ContactDetails,
    CurrentLocation,
    Message,
    CreatedDate
FROM AspNetUsers
WHERE CurrentLocation IS NOT NULL AND CurrentLocation LIKE '%YourLocationHere%'
ORDER BY CreatedDate DESC;
*/
