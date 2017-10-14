UPDATE Bounties
  SET ClaimingPlayerId = @ClaimingPlayerId
    , ClaimedDate = @ClaimedDate
  WHERE TargetPlayerId = @TargetPlayerId
    AND ClaimingPlayerId IS NULL