USE InterviewDB
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE tbl_EmpDep
(
	EmployeeID INT NOT NULL PRIMARY KEY,
	EmployeeName VARCHAR(100) NOT NULL,
	DepartmentID INT NOT NULL,
	DepartmentName VARCHAR(100) NOT NULL,
	JoinDate DATETIME
)

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Insert 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO tbl_EmpDep 
(EmployeeID,EmployeeName,DepartmentID,DepartmentName,JoinDate)
VALUES 
(1, 'Ryan Giggs', 1, 'Department A', '2006/02/18'), 
(2, 'Paul Scholes', 1, 'Department A', '2007/03/07'), 
(3, 'Rio Ferdinand', 1, 'Department A', '2015/10/03'), 

(4, 'Wayne Rooney', 2, 'Department B', '2012/02/06'), 
(5, 'Robin van Persie', 2, 'Department B', '2014/09/29'), 
(6, 'Chris Smalling', 2, 'Department B', '2010/05/17'), 

(7, 'David de Gea', 3, 'Department C', '2013/07/04'), 
(8, 'Danny Welbeck', 3, 'Department C', '2011/02/09'), 
(9, 'Antonio Valencia', 3, 'Department C', '1996/09/16'), 

(10, 'Rafael', 4, 'Department D', '2006/12/10'), 
(11, 'Dimitar Berbatov', 4, 'Department D', '2015/01/18'), 
(12, 'Nani', 4, 'Department D', '1999/11/06');

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Select query using below business logic ...,
-- Get the employee list who joined lastly in every department(s).
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT * FROM tbl_EmpDep AS [data] 
WHERE JoinDate = (SELECT MAX(JoinDate) FROM tbl_EmpDep WHERE DepartmentID = [data].DepartmentID)

SELECT *
FROM (SELECT *, ROW_NUMBER() OVER(PARTITION BY DepartmentID ORDER BY JoinDate DESC) AS RowNumber
      FROM tbl_EmpDep) AS _tbl_EmpDep
WHERE RowNumber = 1 

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Select All 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT * FROM tbl_EmpDep 