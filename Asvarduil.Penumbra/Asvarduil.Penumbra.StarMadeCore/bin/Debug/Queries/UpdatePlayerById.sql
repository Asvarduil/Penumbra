UPDATE Players
    SET LastLoggedInDate = @LastLoggedInDate
      , IsAdmin = @IsAdmin
  WHERE Id = @Id;