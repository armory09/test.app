USE hr_bak;
GO


CREATE PROCEDURE CheckEmployeeIdInclusiveDate(
	   @EmployeeId AS int, @DateFrom AS datetime, @DateTo AS datetime)
AS
BEGIN
	SELECT el.EmployeeLeaveId, el.Reason, el.DateCreated, el.DateFiled, el.DateFrom, el.DateTo, el.LeaveAvail, l.LeaveId, l.LeaveName, l.MaxValue, e.employee_id, e.firstname, e.lastname
	FROM EmployeeLeave AS el
		 INNER JOIN
		 Leave AS l
		 ON l.LeaveId = el.LeaveId
		 INNER JOIN
		 employee AS e
		 ON e.employee_id = el.EmployeeId
	WHERE el.EmployeeId = @EmployeeId AND el.DateFiled BETWEEN @DateFrom AND @DateTo
END; 
GO