UPDATE Reputation
  SET Reputation = @Reputation
    , UpdatedDate = @UpdatedDate
  WHERE PlayerId = @PlayerId;