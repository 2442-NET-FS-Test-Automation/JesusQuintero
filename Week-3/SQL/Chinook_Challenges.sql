USE Chinook_AutoIncrement;

--================================= BASIC CHALLENGES =================================
-- BASIC 1
SELECT FirstName + ' ' + LastName AS FullName, CustomerId, Country FROM dbo.Customer 
WHERE Country != 'USA';

-- BASIC 2
SELECT * FROM dbo.Customer 
WHERE Country = 'Brazil';

-- BASIC 3
SELECT * FROM dbo.Employee
WHERE title LIKE '%Sales%Agent%';

-- BASIC 4
SELECT BillingCountry FROM dbo.Invoice GROUP BY BillingCountry;

-- BASIC 5
SELECT COUNT(*) AS InvoiceNumber, SUM(Total) AS SalesTotal FROM dbo.Invoice
WHERE YEAR(InvoiceDate) = '2021';

-- BASIC 5 - CHALLENGE
SELECT YEAR(InvoiceDate) AS Year,COUNT(*) AS InvoiceNumber, SUM(Total) AS SalesTotal FROM dbo.Invoice
GROUP BY YEAR(InvoiceDate);

-- BASIC 6
SELECT InvoiceId, COUNT(*) AS LinesNumber FROM dbo.InvoiceLine
WHERE InvoiceId = 37 GROUP BY InvoiceId;

-- BASIC 7
SELECT BillingCountry, COUNT(BillingCountry) AS #_Of_Invoices  FROM dbo.Invoice GROUP BY BillingCountry;

-- BASIC 8
SELECT BillingCountry, SUM(Total) AS Sales_Per_Country FROM dbo.Invoice
GROUP BY BillingCountry ORDER BY Sales_Per_Country DESC;

--================================= JOIN CHALLENGES =================================

-- JOIN 1 -> Every Album by Artist
SELECT al.Title, ar.Name FROM dbo.Album AS al
LEFT JOIN Artist AS ar ON al.ArtistId = ar.ArtistId 
ORDER BY ar.Name;

-- JOIN 2 -> All songs of the rock genre
SELECT t.Name, g.Name FROM dbo.Track AS t
INNER JOIN dbo.Genre AS g ON t.GenreId = g.GenreId
WHERE g.Name = 'Rock';

-- JOIN 3 -> Show all invoices of customers from brazil (mailing address not billing)
SELECT c.FirstName, c.LastName, C.Country AS MailingCountry, i.InvoiceDate, i.Total
FROM dbo.Invoice AS i 
INNER JOIN dbo.Customer AS c ON c.CustomerId = i.CustomerId
WHERE c.Country = 'Brazil';

-- JOIN 4 -> Show all invoices together with the name of the sales agent for each one
SELECT e.FirstName + ' ' + e.LastName AS AgentName, c.FirstName + ' ' + c.LastName AS CustomerName, i.Total,
       i.InvoiceId, i.InvoiceDate, i.BillingAddress, i.BillingCity, i.BillingState, i.BillingCountry, i.BillingPostalCode
FROM dbo.Invoice AS i
INNER JOIN Customer AS c ON c.CustomerId = i.CustomerId
INNER JOIN Employee AS e ON e.EmployeeId = c.SupportRepId
ORDER BY AgentName;

-- JOIN 5 -> Which sales agent made the most sales in 2021?
SELECT TOP 1 e.FirstName + ' ' + e.LastName AS AgentName, COUNT(i.Total) AS TotalSales
FROM dbo.Invoice AS i
INNER JOIN Customer AS c ON c.CustomerId = i.CustomerId
INNER JOIN Employee AS e ON e.EmployeeId = c.SupportRepId
WHERE YEAR(i.InvoiceDate) = '2021'
GROUP BY e.FirstName + ' ' + e.LastName
ORDER BY TotalSales DESC;

-- JOIN 8 -> How many customers are assigned to each sales agent?
SELECT e.FirstName + ' ' + e.LastName AS AgentName, COUNT(c.SupportRepId) AS ClietsAssigned
FROM dbo.Employee AS e
INNER JOIN Customer AS c ON e.EmployeeId = c.SupportRepId
GROUP BY e.FirstName + ' ' + e.LastName
ORDER BY ClietsAssigned DESC;

-- JOIN 9 -> Which track was purchased the most in 2022?
SELECT TOP 1 t.Name, SUM(il.Quantity) AS TotalPurchases FROM dbo.Track AS t
INNER JOIN dbo.InvoiceLine AS il ON il.TrackId = t.TrackId
GROUP BY t.Name ORDER BY TotalPurchases DESC;

-- JOIN 10 -> Show the top three best selling artists.
Select TOP 3 t.Composer, SUM(il.Quantity * il.UnitPrice) AS TotalSelled FROM dbo.Track AS t
INNER JOIN dbo.InvoiceLine AS il ON t.TrackId = il.TrackId
WHERE t.Composer IS NOT NULL
GROUP BY t.Composer ORDER BY TotalSelled DESC;

-- JOIN 11 -> -- Which customers have the same initials as at least one other customer?
SELECT SUBSTRING(FirstName, 1, 1) AS Initial, FirstName + ' ' + LastName AS FullName
FROM dbo.Customer ORDER BY Initial;