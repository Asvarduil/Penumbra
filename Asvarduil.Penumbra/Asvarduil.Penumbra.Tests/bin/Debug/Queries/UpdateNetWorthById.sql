UPDATE NetWorths
   SET Value = @Value
     , LastUpdatedDate = @LastUpdatedDate
  WHERE Id = @Id;