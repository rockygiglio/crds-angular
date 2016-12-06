use [MinistryPlatform]

create table #temp_ToDelete
(
   Contact_ID int,
   Household_ID int,
   Contact_Household_ID int
)

INSERT into #temp_ToDelete
select ch.contact_ID, ch.household_ID as 'Needs to be Deleted from Contact_Households', ch.contact_household_ID
FROM Contact_Households ch
where [Household_Type_ID] = 1


Delete from 
Contact_Households
where contact_household_ID in (
  select contact_household_id 
  FROM #temp_ToDelete
  )



If(OBJECT_ID('tempdb..#temp_ToDelete') Is Not Null)
Begin
    Drop Table #temp_ToDelete
End