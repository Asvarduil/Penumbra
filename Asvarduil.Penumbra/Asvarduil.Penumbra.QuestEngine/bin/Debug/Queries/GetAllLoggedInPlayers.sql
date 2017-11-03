SELECT *
  FROM Players
  WHERE LastLoggedInDate IS NOT NULL
    AND (LastLoggedOutDate IS NULL 
        OR LastLoggedInDate > LastLoggedOutDate);