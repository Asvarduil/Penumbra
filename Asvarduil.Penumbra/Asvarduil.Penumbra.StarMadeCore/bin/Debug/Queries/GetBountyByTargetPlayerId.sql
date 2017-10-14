SELECT *
  FROM Bounties
  WHERE TargetPlayerId = @TargetPlayerId
    AND ClaimingPlayerId IS NULL;